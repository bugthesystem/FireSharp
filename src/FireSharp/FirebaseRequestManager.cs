namespace FireSharp {
    using System;
    using System.Threading.Tasks;
    using Exceptions;
    using Interfaces;
    using RestSharp;

    internal class FirebaseRequestManager : IFirebaseRequestManager {
        private readonly string _basePath;
        private readonly string _authSecret;

        public FirebaseRequestManager(string basePath, string authSecret) {
            _basePath = basePath;
            _authSecret = authSecret;
        }

        public IRestResponse Get(string path) {
            return ProcessRequest(Method.GET, path, null);
        }

        public IRestResponse Put<T>(string path, T data) {
            return ProcessRequest(Method.PUT, path, data);
        }

        public IRestResponse Post<T>(string path, T data) {
            return ProcessRequest(Method.POST, path, data);
        }

        public IRestResponse Delete(string path) {
            return ProcessRequest(Method.DELETE, path, null);
        }

        public IRestResponse Patch<T>(string path, T data) {
            return ProcessRequest(Method.PATCH, path, data);
        }

        public async Task<IRestResponse> GetTaskAsync(string path) {
            return await ProcessRequestAsync(Method.GET, path, null);
        }

        public async Task<IRestResponse> PutTaskAsync<T>(string path, T data) {
            return await ProcessRequestAsync(Method.PATCH, path, data);
        }

        public async Task<IRestResponse> PostTaskAsync<T>(string path, T data) {
            return await ProcessRequestAsync(Method.POST, path, data);
        }

        public async Task<IRestResponse> DeleteTaskAsync(string path) {
            return await ProcessRequestAsync(Method.DELETE, path, null);
        }

        public async Task<IRestResponse> PatchTaskAsync<T>(string path, T data) {
            return await ProcessRequestAsync(Method.PATCH, path, data);
        }

        private IRestResponse ProcessRequest(Method requestMethod, string path, object data) {
            try {
                string authToken = !string.IsNullOrWhiteSpace(_authSecret) 
                    ? string.Format("{0}.json?auth={1}", path, _authSecret)
                    : string.Format("{0}.json", path);

                string url = string.Format("{0}{1}", _basePath, authToken);

                var client = new RestClient(url);
                var request = new RestRequest(requestMethod) { RequestFormat = DataFormat.Json };
                if (data != null) {
                    request.AddBody(data);
                }
                return client.Execute(request);

            } catch (Exception ex) {
                throw new FirebaseException("An error occured while execution request.", ex);
            }
        }

        private async Task<IRestResponse> ProcessRequestAsync(Method requestMethod, string path, object data) {
            try {
                var client = new RestClient(_basePath + string.Format("{0}.json?auth={1}", path, _authSecret));
                var request = new RestRequest(requestMethod) { RequestFormat = DataFormat.Json };
                if (data != null) {
                    request.AddBody(data);
                }
                return await client.ExecuteTaskAsync(request);
            } catch (Exception ex) {
                throw new FirebaseException("An error occured while execution request.", ex); ;
            }
        }

    }
}