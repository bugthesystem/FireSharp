using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.Core.EventStreaming;
using FireSharp.Core.Extensions;
using FireSharp.Core.Interfaces;

namespace FireSharp.Core.Response
{
    public class EventRootResponse<T> : IDisposable
    {
        private readonly ValueRootAddedEventHandler<T> _added;
        private readonly CancellationTokenSource _cancel;
        private readonly string _path;
        private readonly Task _pollingTask;
        private readonly IRequestManager _requestManager;

        internal EventRootResponse(HttpResponseMessage httpResponse, ValueRootAddedEventHandler<T> added,
            IRequestManager requestManager, string path)
        {
            _added = added;
            _requestManager = requestManager;
            _path = path;

            _cancel = new CancellationTokenSource();
            _pollingTask = ReadLoop(httpResponse, _cancel.Token);
        }

        ~EventRootResponse()
        {
            Dispose(false);    
        }

        private async Task ReadLoop(HttpResponseMessage httpResponse, CancellationToken token)
        {
            await Task.Factory.StartNew(async () =>
            {
                using (httpResponse)
                using (var content = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(content))
                {
                    string eventName = null;

                    while (true)
                    {
                        _cancel.Token.ThrowIfCancellationRequested();

                        var read = await sr.ReadLineAsync().ConfigureAwait(false);

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
                                throw new InvalidOperationException(
                                    "Payload data was received but an event did not preceed it.");
                            }

                            // Every change on child, will get entire object again.
                            var request = await _requestManager.RequestAsync(HttpMethod.Get, _path);
                            var jsonStr = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

                            _added(this, jsonStr.ReadAs<T>());
                        }

                        // start over
                        eventName = null;
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
        }

        public void Cancel()
        {
            _cancel.Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Cancel();

            if (disposing)
            {
                _cancel.Dispose();
            }
        }
    }
}
