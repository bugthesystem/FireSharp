using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Exceptions;
using FireSharp.Interfaces;

namespace FireSharp
{
    internal class RequestManager : IRequestManager
    {
        private readonly IFirebaseConfig _config;

        internal RequestManager(IFirebaseConfig config)
        {
            _config = config;
        }

        private HttpClient GetClient(HttpClientHandler handler = null)
        {
            var client = handler == null ? new HttpClient() : new HttpClient(handler, true);

            client.BaseAddress = new Uri(_config.BasePath);

            if (_config.RequestTimeout.HasValue)
            {
                client.Timeout = _config.RequestTimeout.Value;
            }

            return client;
        }

        public void Dispose()
        {
        }

        public Task<HttpResponseMessage> GetAsync(string path)
        {
            return ProcessRequestAsync(HttpMethod.Get, path, null);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string path, T data)
        {
            return ProcessRequestAsync(HttpMethod.Put, path, data);
        }

        public Task<HttpResponseMessage> PostAsync<T>(string path, T data)
        {
            return ProcessRequestAsync(HttpMethod.Post, path, data);
        }

        public Task<HttpResponseMessage> DeleteAsync(string path)
        {
            return ProcessRequestAsync(HttpMethod.Delete, path, null);
        }

        public Task<HttpResponseMessage> PatchAsync<T>(string path, T data)
        {
            return ProcessRequestAsync(new HttpMethod("PATCH"), path, data);
        }

        public async Task<HttpResponseMessage> ListenAsync(string path)
        {
            HttpRequestMessage request;
            var client = PrepareEventStreamRequest(path, out request);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        private Task<HttpResponseMessage> ProcessRequestAsync(HttpMethod method, string path, object payload, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            try
            {
                var request = PrepareRequest(method, path, payload);

                return GetClient().SendAsync(request, httpCompletionOption);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, method), ex);
            }
        }

        private HttpClient PrepareEventStreamRequest(string path, out HttpRequestMessage request)
        {
            var client = GetClient(new HttpClientHandler { AllowAutoRedirect = true });
            var uri = PrepareUri(path);

            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            return client;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, string path, object payload)
        {
            var uri = PrepareUri(path);

            var request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                var json = _config.Serializer.Serialize(payload);
                request.Content = new StringContent(json);
            }

            return request;
        }

        private Uri PrepareUri(string path)
        {
            var authToken = !string.IsNullOrWhiteSpace(_config.AuthSecret)
                ? string.Format("{0}.json?auth={1}", path, _config.AuthSecret)
                : string.Format("{0}.json", path);

            var url = string.Format("{0}{1}", _config.BasePath, authToken);

            return new Uri(url);
        }
    }
}