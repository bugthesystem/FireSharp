using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FireSharp.Exceptions;
using FireSharp.Interfaces;

namespace FireSharp
{
    internal class RequestManager : IRequestManager
    {
        internal static readonly HttpMethod Patch = new HttpMethod("PATCH");

        private readonly IFirebaseConfig _config;

        internal RequestManager(IFirebaseConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
        }

        public async Task<HttpResponseMessage> ListenAsync(string path)
        {
            HttpRequestMessage request;
            var client = PrepareEventStreamRequest(path, null, out request);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> ListenAsync(string path, QueryBuilder queryBuilder)
        {
            HttpRequestMessage request;
            var client = PrepareEventStreamRequest(path, queryBuilder, out request);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, object payload)
        {
            return RequestAsync(method, path, null, payload);
        }

        public Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, QueryBuilder queryBuilder, object payload = null)
        {
            try
            {
                var uri = PrepareUri(path, queryBuilder);
                var request = PrepareRequest(method, uri, payload);

                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    $"An error occured while execute request. Path : {path} , Method : {method}", ex);
            }
        }

        public Task<HttpResponseMessage> RequestApiAsync(HttpMethod method, string path, QueryBuilder queryBuilder, object payload = null)
        {
            try
            {
                var uri = PrepareApiUri(path, queryBuilder);
                var request = PrepareRequest(method, uri, payload);

                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    $"An error occured while execute request. Path : {path} , Method : {method}", ex);
            }
        }

        private HttpClient GetClient(HttpClientHandler handler = null)
        {
            var client = handler == null ? new HttpClient() : new HttpClient(handler, true);

            var basePath = _config.BasePath.EndsWith("/") ? _config.BasePath : _config.BasePath + "/";
            client.BaseAddress = new Uri(basePath);

            if (_config.RequestTimeout.HasValue)
            {
                client.Timeout = _config.RequestTimeout.Value;
            }

            return client;
        }

        private HttpClient PrepareEventStreamRequest(string path, QueryBuilder queryBuilder, out HttpRequestMessage request)
        {
            var client = GetClient(new HttpClientHandler { AllowAutoRedirect = true });
            var uri = PrepareUri(path, queryBuilder);

            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            return client;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, Uri uri, object payload)
        {
            var request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                var json = _config.Serializer.Serialize(payload);
                request.Content = new StringContent(json);
            }

            return request;
        }

        private Uri PrepareUri(string path, QueryBuilder queryBuilder)
        {
            var authToken = !string.IsNullOrWhiteSpace(_config.AuthSecret)
                ? $"{path}.json?auth={_config.AuthSecret}"
                : $"{path}.json";

            var queryStr = string.Empty;
            if (queryBuilder != null)
            {
                queryStr = $"&{queryBuilder.ToQueryString()}";
            }

            var url = $"{_config.BasePath}{authToken}{queryStr}";

            return new Uri(url);
        }

        private Uri PrepareApiUri(string path, QueryBuilder queryBuilder)
        {
            string uriString = $"https://auth.firebase.com/v2/{_config.Host}/{path}?{queryBuilder.ToQueryString()}";
            return new Uri(uriString);
        }
    }
}