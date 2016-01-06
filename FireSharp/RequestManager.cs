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
        internal static readonly HttpMethod Patch = new HttpMethod("PATCH");

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

        public async Task<HttpResponseMessage> ListenAsync(string path)
        {
            HttpRequestMessage request;
            var client = PrepareEventStreamRequest(path, string.Empty, out request);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> ListenAsync(string path, string query)
        {
            HttpRequestMessage request;
            var client = PrepareEventStreamRequest(path, query, out request);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, object payload)
        {
            try
            {
                var request = PrepareRequest(method, path, string.Empty, payload);

                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, method), ex);
            }
        }

        public Task<HttpResponseMessage> RequestApiAsync(HttpMethod method, string path, string query, object payload)
        {
            try
            {
                var request = PrepareApiRequest(method, path, query, payload);
                
                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, method), ex);
            }
        }

        public Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, string query, object payload)
        {
            try
            {
                var request = PrepareRequest(method, path, query, payload);

                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(
                    string.Format("An error occured while execute request. Path : {0} , Method : {1}", path, method), ex);
            }
        }

        private HttpClient PrepareEventStreamRequest(string path, string query, out HttpRequestMessage request)
        {
            var client = GetClient(new HttpClientHandler { AllowAutoRedirect = true });
            var uri = PrepareUri(path,query);

            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            return client;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, string path, string query, object payload)
        {
            var uri = PrepareUri(path,query);

            var request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                var json = _config.Serializer.Serialize(payload);
                request.Content = new StringContent(json);
            }

            return request;
        }

        private HttpRequestMessage PrepareApiRequest(HttpMethod method, string path, string query, object payload)
        {
            var uri = PrepareApiUri(path,query);

            var request = new HttpRequestMessage(method, uri);

            if (payload != null)
            {
                var json = _config.Serializer.Serialize(payload);
                request.Content = new StringContent(json);
            }

            return request;
        }

        private Uri PrepareUri(string path, string query)
        {
            var authToken = !string.IsNullOrWhiteSpace(_config.AuthSecret)
                ? string.Format("{0}.json?auth={1}", path, _config.AuthSecret)
                : string.Format("{0}.json", path);
            string addl = string.Empty;
            if (!string.IsNullOrEmpty(query))
            {
                addl = "&" + query;
            }
            var url = string.Format("{0}{1}{2}", _config.BasePath, authToken, addl);

            return new Uri(url);
        }

        private Uri PrepareApiUri(string path, string query)
        {
            var url = string.Format("{0}/{1}/{2}?{3}", "https://auth.firebase.com/v2",_config.Host,path,query);
            
            return new Uri(url);
        }
    }
}