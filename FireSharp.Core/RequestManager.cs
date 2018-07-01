using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FireSharp.Core.Exceptions;
using FireSharp.Core.Interfaces;

namespace FireSharp.Core
{
    internal class RequestManager : IRequestManager
    {
        internal static readonly HttpMethod Patch = new HttpMethod("PATCH");

        private readonly IFirebaseConfig _config;
        private readonly HttpClient _httpClient;

        internal RequestManager(IFirebaseConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;

            _httpClient = new HttpClient(new AutoRedirectHttpClientHandler());

            var basePath = _config.BasePath.EndsWith("/") ? _config.BasePath : _config.BasePath + "/";
            _httpClient.BaseAddress = new Uri(basePath);

            if (_config.RequestTimeout.HasValue)
            {
                _httpClient.Timeout = _config.RequestTimeout.Value;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
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

        private HttpClient GetClient()
        {
            return _httpClient;
        }

        private HttpClient PrepareEventStreamRequest(string path, QueryBuilder queryBuilder, out HttpRequestMessage request)
        {
            var client = GetClient();
            var uri = PrepareUri(path, queryBuilder);

            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            if (!string.IsNullOrEmpty(_config.AccessToken)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.AccessToken);
            }

            return client;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, Uri uri, object payload)
        {
            var request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                request.Content = new StringContent(payload as string ?? _config.Serializer.Serialize(payload));
            }

            if (!string.IsNullOrEmpty(_config.AccessToken)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.AccessToken);
            }

            return request;
        }

        private Uri PrepareUri(string path, QueryBuilder queryBuilder)
        {
            var authToken = !string.IsNullOrWhiteSpace(_config.AuthSecret)
                ? $"{path}.json?auth={_config.AuthSecret}"
                : $"{path}.json?";

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
