namespace FireSharp {
    using System.Threading.Tasks;
    using Exceptions;
    using Interfaces;
    using Response;
    using Config;

    public class FirebaseClient : IFirebaseClient {
        private readonly IFirebaseRequestManager _requestManager;

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

        public SetResponse Set<T>(string path, T data) {
            SetResponse response;
            try {
                response = new SetResponse(_requestManager.Put(path, data));
            } catch (FirebaseException ex) {
                response = new SetResponse { Exception = ex };
            }
            return response;
        }

        public PushResponse Push<T>(string path, T data) {
            PushResponse response;
            try {
                response = new PushResponse(_requestManager.Post(path, data));
            } catch (FirebaseException ex) {
                response = new PushResponse { Exception = ex };
            }
            return response;
        }

        public DeleteResponse Delete(string path) {
            DeleteResponse response;
            try {
                response = new DeleteResponse(_requestManager.Delete(path));
            } catch (FirebaseException ex) {
                response = new DeleteResponse { Exception = ex };
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

        public async Task<SetResponse> SetTaskAsync<T>(string path, T data) {
            SetResponse response;
            try {
                response = new SetResponse(await _requestManager.PutTaskAsync(path, data));
            } catch (FirebaseException ex) {
                response = new SetResponse { Exception = ex };
            }
            return response;
        }

        public async Task<PushResponse> PushTaskAsync<T>(string path, T data) {
            PushResponse response;
            try {
                response = new PushResponse(await _requestManager.PostTaskAsync(path, data));
            } catch (FirebaseException ex) {
                response = new PushResponse { Exception = ex };
            }
            return response;
        }

        public async Task<DeleteResponse> DeleteTaskAsync(string path) {
            DeleteResponse response;
            try {
                response = new DeleteResponse(await _requestManager.DeleteTaskAsync(path));
            } catch (FirebaseException ex) {
                response = new DeleteResponse { Exception = ex };
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