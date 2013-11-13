namespace FireSharp {
    using System.Threading.Tasks;
    using Exceptions;
    using Interfaces;
    using Response;
    using Config;

    public class FirebaseClient : IFirebaseClient {
        private readonly IFirebaseRequestManager _requestManager;

        public FirebaseClient(string basePath, string authSecret)
            : this(new FirebaseRequestManager(basePath, authSecret)) {
        }
        public FirebaseClient(IFirebaseConfig config)
            : this(new FirebaseRequestManager(config)) {
        }

        internal FirebaseClient(IFirebaseRequestManager requestManager) {
            _requestManager = requestManager;
        }

        public FirebaseResponse Get(string path) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(_requestManager.Get(path));
            } catch (FirebaseException ex) {

                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public FirebaseResponse Set<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(_requestManager.Put(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public FirebaseResponse Push<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(_requestManager.Post(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public FirebaseResponse Delete(string path) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(_requestManager.Delete(path));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public FirebaseResponse Update<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(_requestManager.Patch(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> GetTaskAsync(string path) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(await _requestManager.GetTaskAsync(path));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> SetTaskAsync<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(await _requestManager.PutTaskAsync(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> PushTaskAsync<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(await _requestManager.PostTaskAsync(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> DeleteTaskAsync(string path) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(await _requestManager.DeleteTaskAsync(path));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> UpdateTaskAsync<T>(string path, T data) {
            FirebaseResponse response;
            try {
                response = new FirebaseResponse(await _requestManager.PatchTaskAsync(path, data));
            } catch (FirebaseException ex) {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }
    }
}