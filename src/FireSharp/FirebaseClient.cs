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
        private readonly IFirebaseRequestManager _requestManager;

        public FirebaseClient(IFirebaseConfig config)
            : this(new FirebaseRequestManager(config))
        {
        }

        internal FirebaseClient(IFirebaseRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public void Dispose()
        {
        }

        public FirebaseResponse Get(string path)
        {
            FirebaseResponse response;
            try
            {
                var task = _requestManager.Get(path);
                response = new FirebaseResponse(task.Result);
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse {Exception = ex};
            }
            return response;
        }

        public SetResponse Set<T>(string path, T data)
        {
            SetResponse response;
            try
            {
                var task = _requestManager.Put(path, data);
                response = new SetResponse(task.Result);
            }
            catch (FirebaseException ex)
            {
                response = new SetResponse {Exception = ex};
            }
            return response;
        }

        public PushResponse Push<T>(string path, T data)
        {
            PushResponse response;
            try
            {
                var task = _requestManager.Post(path, data);
                response = new PushResponse(task.Result);
            }
            catch (FirebaseException ex)
            {
                response = new PushResponse {Exception = ex};
            }
            return response;
        }

        public DeleteResponse Delete(string path)
        {
            DeleteResponse response;
            try
            {
                var task = _requestManager.Delete(path);
                response = new DeleteResponse(task.Result);
            }
            catch (FirebaseException ex)
            {
                response = new DeleteResponse {Exception = ex};
            }
            return response;
        }

        public FirebaseResponse Update<T>(string path, T data)
        {
            FirebaseResponse response;
            try
            {
                var task = _requestManager.Patch(path, data);
                response = new FirebaseResponse(task.Result);
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse {Exception = ex};
            }
            return response;
        }

        public FirebaseResponse GetStreaming(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return GetStreamingAsync(path, added, changed, removed).Result;
        }

        public async Task<FirebaseResponse> GetTaskAsync(string path)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.GetTaskAsync(path));
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse {Exception = ex};
            }
            return response;
        }

        public async Task<SetResponse> SetTaskAsync<T>(string path, T data)
        {
            SetResponse response;
            try
            {
                response = new SetResponse(await _requestManager.PutTaskAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new SetResponse {Exception = ex};
            }
            return response;
        }

        public async Task<PushResponse> PushTaskAsync<T>(string path, T data)
        {
            PushResponse response;
            try
            {
                response = new PushResponse(await _requestManager.PostTaskAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new PushResponse {Exception = ex};
            }
            return response;
        }

        public async Task<DeleteResponse> DeleteTaskAsync(string path)
        {
            DeleteResponse response;
            try
            {
                response = new DeleteResponse(await _requestManager.DeleteTaskAsync(path));
            }
            catch (FirebaseException ex)
            {
                response = new DeleteResponse {Exception = ex};
            }
            return response;
        }

        public async Task<FirebaseResponse> UpdateTaskAsync<T>(string path, T data)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.PatchTaskAsync(path, data));
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse {Exception = ex};
            }
            return response;
        }

        public async Task<FirebaseResponse> GetStreamingAsync(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            FirebaseResponse response;
            try
            {
                response = new FirebaseResponse(await _requestManager.GetStreaming(path), added, changed, removed);
            }
            catch (FirebaseException ex)
            {
                response = new FirebaseResponse {Exception = ex};
            }

            return response;
        }
    }
}