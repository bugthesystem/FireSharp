namespace FireSharp
{
    using System;
    using System.Threading.Tasks;
    using Exceptions;
    using Interfaces;
    using Config;
    using RestSharp;

    internal class FirebaseRequestManager : IFirebaseRequestManager
    {
        private readonly IFirebaseConfig _config;

        internal FirebaseRequestManager(IFirebaseConfig config)
        {
            _config = config;
        }

        public IRestResponse Get(string path)
        {
            return ProcessRequest(Method.GET, path, null);
        }

        public IRestResponse Put<T>(string path, T data)
        {
            return ProcessRequest(Method.PUT, path, data);
        }

        public IRestResponse Post<T>(string path, T data)
        {
            return ProcessRequest(Method.POST, path, data);
        }

        public IRestResponse Delete(string path)
        {
            return ProcessRequest(Method.DELETE, path, null);
        }

        public IRestResponse Patch<T>(string path, T data)
        {
            return ProcessRequest(Method.PATCH, path, data);
        }

        public async Task<IRestResponse> GetTaskAsync(string path)
        {
            return await ProcessRequestTaskAsync(Method.GET, path, null);
        }

        public async Task<IRestResponse> PutTaskAsync<T>(string path, T data)
        {
            return await ProcessRequestTaskAsync(Method.PATCH, path, data);
        }

        public async Task<IRestResponse> PostTaskAsync<T>(string path, T data)
        {
            return await ProcessRequestTaskAsync(Method.POST, path, data);
        }

        public async Task<IRestResponse> DeleteTaskAsync(string path)
        {
            return await ProcessRequestTaskAsync(Method.DELETE, path, null);
        }

        public async Task<IRestResponse> PatchTaskAsync<T>(string path, T data)
        {
            return await ProcessRequestTaskAsync(Method.PATCH, path, data);
        }

        private IRestResponse ProcessRequest(Method requestMethod, string path, object data)
        {
            try
            {
                RestRequest request;
                IRestClient client = PrepareClient(requestMethod, path, data, out request);
                return client.Execute(request);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(string.Format("An error occured while execution request. Path : {0} , Method : {1}", path, requestMethod), ex);
            }
        }

        private async Task<IRestResponse> ProcessRequestTaskAsync(Method requestMethod, string path, object data)
        {
            try
            {
                RestRequest request;
                IRestClient client = PrepareClient(requestMethod, path, data, out request);
                return await client.ExecuteTaskAsync(request);
            }
            catch (Exception ex)
            {
                throw new FirebaseException(string.Format("An error occured while execution request. Path : {0} , Method : {1}", path, requestMethod), ex);
            }
        }

        private IRestClient PrepareClient(Method requestMethod, string path, object data, out RestRequest request)
        {
            string authToken = !string.IsNullOrWhiteSpace(_config.AuthSecret)
                ? string.Format("{0}.json?auth={1}", path, _config.AuthSecret)
                : string.Format("{0}.json", path);

            string url = string.Format("{0}{1}", _config.BasePath, authToken);

            var client = new RestClient(url);
            request = new RestRequest(requestMethod)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _config.Serializer
            };
            if (data != null)
            {
                request.AddBody(data);
            }

            return client;
        }
    }
}