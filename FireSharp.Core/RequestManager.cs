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
        internal static readonly HttpMethod Patch = new("PATCH");

        private readonly IFirebaseConfig config;
        private readonly HttpClient httpClient;

        internal RequestManager(IFirebaseConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            httpClient = new HttpClient(new AutoRedirectHttpClientHandler());

            string basePath = this.config.BasePath.EndsWith("/") ? this.config.BasePath : this.config.BasePath + "/";
            httpClient.BaseAddress = new Uri(basePath);

            if (this.config.RequestTimeout.HasValue)
            {
                httpClient.Timeout = this.config.RequestTimeout.Value;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public async Task<HttpResponseMessage> ListenAsync(string path)
        {
            HttpRequestMessage request;
            HttpClient client = PrepareEventStreamRequest(path, null, out request);

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> ListenAsync(string path, QueryBuilder queryBuilder)
        {
            HttpClient client = PrepareEventStreamRequest(path, queryBuilder, out HttpRequestMessage request);

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
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
                Uri uri = PrepareUri(path, queryBuilder);
                HttpRequestMessage request = PrepareRequest(method, uri, payload);

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
                Uri uri = PrepareApiUri(path, queryBuilder);
                HttpRequestMessage request = PrepareRequest(method, uri, payload);

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
            return httpClient;
        }

        private HttpClient PrepareEventStreamRequest(string path, QueryBuilder queryBuilder, out HttpRequestMessage request)
        {
            HttpClient client = GetClient();
            Uri uri = PrepareUri(path, queryBuilder);

            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            if (!string.IsNullOrEmpty(config.AccessToken)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.AccessToken);
            }

            return client;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, Uri uri, object payload)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                request.Content = new StringContent(payload as string ?? config.Serializer.Serialize(payload));
            }

            if (!string.IsNullOrEmpty(config.AccessToken)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.AccessToken);
            }

            return request;
        }

        private Uri PrepareUri(string path, QueryBuilder queryBuilder)
        {
            string authToken = !string.IsNullOrWhiteSpace(config.AuthSecret)
                ? $"{path}.json?auth={config.AuthSecret}"
                : $"{path}.json?";

            string queryStr = string.Empty;
            if (queryBuilder != null)
            {
                queryStr = $"&{queryBuilder.ToQueryString()}";
            }

            string url = $"{config.BasePath}{authToken}{queryStr}";

            return new Uri(url);
        }

        private Uri PrepareApiUri(string path, QueryBuilder queryBuilder)
        {
            string uriString = $"https://auth.firebase.com/v2/{config.Host}/{path}?{queryBuilder.ToQueryString()}";
            return new Uri(uriString);
        }
    }
}
