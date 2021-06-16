using FireSharp.Core.EventStreaming;
using FireSharp.Core.Extensions;
using FireSharp.Core.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FireSharp.Core.Response
{
    public class EventRootResponse<T> : IDisposable
    {
        private readonly ValueRootAddedEventHandler<T> added;
        private readonly CancellationTokenSource cancel;
        private readonly string path;
        private readonly Task pollingTask;
        private readonly IRequestManager requestManager;

        internal EventRootResponse(HttpResponseMessage httpResponse, ValueRootAddedEventHandler<T> added,
            IRequestManager requestManager, string path)
        {
            this.added = added;
            this.requestManager = requestManager;
            this.path = path;

            cancel = new CancellationTokenSource();
            pollingTask = ReadLoop(httpResponse, cancel.Token);
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
                await using (Stream content = await httpResponse.Content.ReadAsStreamAsync(token).ConfigureAwait(false))
                using (StreamReader sr = new(content))
                {
                    string eventName = null;

                    while (true)
                    {
                        cancel.Token.ThrowIfCancellationRequested();

                        string read = await sr.ReadLineAsync().ConfigureAwait(false);

                        Debug.WriteLine(read);

                        if (read != null && read.StartsWith("event: "))
                        {
                            eventName = read[7..];
                            continue;
                        }

                        if (read != null && read.StartsWith("data: "))
                        {
                            if (string.IsNullOrEmpty(eventName))
                            {
                                throw new InvalidOperationException(
                                    "Payload data was received but an event did not proceed it.");
                            }

                            // Every change on child, will get entire object again.
                            HttpResponseMessage request = await requestManager.RequestAsync(HttpMethod.Get, path);
                            string jsonStr = await request.Content.ReadAsStringAsync(token).ConfigureAwait(false);

                            added(this, jsonStr.ReadAs<T>());
                        }

                        // start over
                        eventName = null;
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
        }

        public void Cancel()
        {
            cancel.Cancel();
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
                cancel.Dispose();
            }
        }
    }
}
