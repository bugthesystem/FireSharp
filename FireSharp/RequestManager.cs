using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Exceptions;
using FireSharp.Interfaces;

namespace FireSharp
{
    internal class RequestManager : IRequestManager, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IFirebaseConfig _config;

        internal RequestManager(IFirebaseConfig config)
        {
            _config = config;
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            };

            _client = new HttpClient(handler, true)
            {
                BaseAddress = new Uri(_config.BasePath),
                Timeout = TimeSpan.FromMinutes(_config.TimeoutInMinute)
            };
        }

        public void Dispose()
        {
            using (_client)
            {
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            return await ProcessRequest(HttpMethod.Get, path, null);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string path, T data)
        {
            return await ProcessRequest(HttpMethod.Put, path, data);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string path, T data)
        {
            return await ProcessRequest(HttpMethod.Post, path, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path)
        {
            return await ProcessRequest(HttpMethod.Delete, path, null);
        }

        public async Task<HttpResponseMessage> PatchAsync<T>(string path, T data)
        {
            return await ProcessRequest(new HttpMethod("PATCH"), path, data);
        }

        public async Task<HttpResponseMessage> ListenAsync(string path)
        {
            var uri = PrepareUri(path);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        private Task<HttpResponseMessage> ProcessRequest(HttpMethod method, string path, object payload)
        {
            try
            {
                var uri = PrepareUri(path);

                var request = new HttpRequestMessage(method, uri);

                if (payload != null)
                {
                    var json = _config.Serializer.Serialize(payload);
                    request.Content = new StringContent(json);
                }

                return _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, method), ex);
            }
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