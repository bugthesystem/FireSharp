using System;
using System.Net;
using System.Net.Http;
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
            try
            {
                HttpResponseMessage response = await _requestManager.GetAsync(path);
                VerifyResponse(response);
                return new FirebaseResponse(response);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<SetResponse> SetAsync<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.PutAsync(path, data);
                VerifyResponse(response);
                return new SetResponse(response);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<PushResponse> PushAsync<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.PostAsync(path, data);
                VerifyResponse(response);
                return new PushResponse(response);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }   

        public async Task<DeleteResponse> DeleteAsync(string path)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.DeleteAsync(path);
                VerifyResponse(response);
                return new DeleteResponse(response);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<FirebaseResponse> UpdateAsync<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.PatchAsync(path, data);
                VerifyResponse(response);
                return new FirebaseResponse(response);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        [Obsolete("This method is obsolete use OnAsync instead.")]
        public async Task<FirebaseResponse> ListenAsync(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return new FirebaseResponse(await _requestManager.ListenAsync(path), added, changed, removed);
        }

        private void VerifyResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                return;

            string body = response.Content.ReadAsStringAsync().Result ?? "";
            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new FirebaseException("Bad Request: " + body);

            throw new FirebaseException(String.Format("Request failed, status code: {0} {1}",
                response.StatusCode, body));
        }

        public async Task<FirebaseResponse> OnAsync(string path, ValueAddedEventHandler added = null, ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return new FirebaseResponse(await _requestManager.ListenAsync(path), added, changed, removed);
        }
    }
}