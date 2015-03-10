using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using Newtonsoft.Json;

namespace FireSharp.Response
{
    public class EventStreamResponse : FirebaseResponse
    {
        private readonly TemporaryCache _cache;
        private readonly CancellationTokenSource _cancel;
        private readonly Task _pollingTask;

        public EventStreamResponse(HttpResponseMessage httpResponse)
            : base(httpResponse)
        {
        }

        internal EventStreamResponse(HttpResponseMessage httpResponse,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
            : this(httpResponse)
        {
            _cancel = new CancellationTokenSource();

            _cache = new TemporaryCache();

            if (added != null)
            {
                _cache.Added += added;
            }
            if (changed != null)
            {
                _cache.Changed += changed;
            }
            if (removed != null)
            {
                _cache.Removed += removed;
            }

            _pollingTask = ReadLoop(httpResponse, _cancel.Token);
        }

        private async Task ReadLoop(HttpResponseMessage httpResponse, CancellationToken token)
        {
            await Task.Factory.StartNew(async () =>
            {
                using (httpResponse)
                using (var content = await httpResponse.Content.ReadAsStreamAsync())
                using (var sr = new StreamReader(content))
                {
                    string eventName = null;

                    while (true)
                    {
                        _cancel.Token.ThrowIfCancellationRequested();
                        var read = await sr.ReadLineAsync();
                        Debug.WriteLine(read);
                        if (read.StartsWith("event: "))
                        {
                            eventName = read.Substring(7);
                            continue;
                        }

                        if (read.StartsWith("data: "))
                        {
                            if (string.IsNullOrEmpty(eventName))
                            {
                                throw new InvalidOperationException("Payload data was received but an event did not preceed it.");
                            }

                            Update(eventName, read.Substring(6));
                        }

                        // start over
                        eventName = null;
                    }
                }

            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }


        public void Cancel()
        {
            _cancel.Cancel();
        }

        private void Update(string eventName, string p)
        {
            switch (eventName)
            {
                case "put":
                case "patch":
                    using (var r = new StringReader(p))
                    using (JsonReader reader = new JsonTextReader(r))
                    {
                        ReadToNamedPropertyValue(reader, "path");
                        reader.Read();
                        var path = reader.Value.ToString();

                        if (eventName == "put")
                        {
                            _cache.Replace(path, ReadToNamedPropertyValue(reader, "data"));
                        }
                        else
                        {
                            _cache.Update(path, ReadToNamedPropertyValue(reader, "data"));
                        }
                    }
                    break;
            }
        }

        private JsonReader ReadToNamedPropertyValue(JsonReader reader, string property)
        {
            while (reader.Read() && reader.TokenType != JsonToken.PropertyName)
            {
                // skip the property
            }

            var prop = reader.Value.ToString();
            if (property != prop)
            {
                throw new InvalidOperationException("Error parsing response.  Expected json property named: " + property);
            }

            return reader;
        }

        public void Dispose()
        {
            Cancel();
            using (_cancel)
            {
            }
        }
    }
}