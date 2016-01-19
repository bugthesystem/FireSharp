﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
using FireSharp.Interfaces;

namespace FireSharp.Response
{
    public class EventRootResponse<T>
    {
        private readonly ValueRootAddedEventHandler<T> _added;
        private readonly IRequestManager _requestManager;
        private readonly string _path;
        private readonly CancellationTokenSource _cancel;
        private readonly Task _pollingTask;

        internal EventRootResponse(HttpResponseMessage httpResponse, ValueRootAddedEventHandler<T> added, IRequestManager requestManager, string path)
        {
            _added = added;
            _requestManager = requestManager;
            _path = path;

            _cancel = new CancellationTokenSource();
            _pollingTask = ReadLoop(httpResponse, _cancel.Token);
        }

        private async Task ReadLoop(HttpResponseMessage httpResponse, CancellationToken token)
        {
            Debug.WriteLine("ReadLoop");
            await Task.Factory.StartNew(async () =>
            {
                using (httpResponse)
                {
                    using (var content = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        using (var sr = new StreamReader(content))
                        {
                            string eventName = null;

                            while (true)
                            {
                                try
                                {
                                    _cancel.Token.ThrowIfCancellationRequested();
                                    var read = await sr.ReadLineAsync().ConfigureAwait(false);
                                    //Debug.WriteLine(read);
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
                                        // Every change on child, will get entire object again.
                                        var request = await _requestManager.RequestAsync(HttpMethod.Get, _path);
                                        var jsonStr = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                                        _added(this, jsonStr.ReadAs<T>());
                                    }

                                    // start over
                                    eventName = null;
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine(e);
                                }
                            }
                        }
                    }
                }

            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Cancel()
        {
            _cancel.Cancel();
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
