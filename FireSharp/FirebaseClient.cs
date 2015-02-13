using System;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.EventStreaming;
using FireSharp.Exceptions;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace FireSharp
{
    public class FirebaseClient : IFirebaseClient, IDisposable
    {
        private readonly IRequestManager _requestManager;

        public FirebaseClient(IFirebaseConfig config)
            : this(new RequestManager(config))
        {
        }

        internal FirebaseClient(IRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public void Dispose()
        {
            using (_requestManager) { }
        }

        public async Task<FirebaseResponse> GetAsync(string path)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.GetAsync(path));
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<SetResponse> SetAsync<T>(string path, T data)
        {
            SetResponse response;
            try
            {
                response = new SetResponse(await _requestManager.PutAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new SetResponse { Exception = ex };
            }
            return response;
        }

        public async Task<PushResponse> PushAsync<T>(string path, T data)
        {
            PushResponse response;
            try
            {
                response = new PushResponse(await _requestManager.PostAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new PushResponse { Exception = ex };
            }
            return response;
        }

        public async Task<DeleteResponse> DeleteAsync(string path)
        {
            DeleteResponse response;
            try
            {
                response = new DeleteResponse(await _requestManager.DeleteAsync(path));
            }
            catch (FirebaseException ex)
            {
                response = new DeleteResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> UpdateAsync<T>(string path, T data)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.PatchAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse { Exception = ex };
            }
            return response;
        }

        public async Task<FirebaseResponse> ListenAsync(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.ListenAsync(path), added, changed, removed);
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse { Exception = ex };
            }

            return response;
        }
    }
}