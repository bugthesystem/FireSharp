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
        private readonly IFirebaseConfig _config;

        private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
        {
            if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
            {
                throw new FirebaseException(statusCode, body);
            }
        };


        public FirebaseClient(IFirebaseConfig config)
            : this(new RequestManager(config))
        {
            _config = config;
        }

        internal FirebaseClient(IRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public void Dispose()
        {
            using (_requestManager) { }
        }

        public FirebaseResponse Get(string path)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(HttpMethod.Get, path).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse Get(string path, string query)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(HttpMethod.Get, path, query).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public SetResponse Set<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(HttpMethod.Put, path, data).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new SetResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public PushResponse Push<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(HttpMethod.Post, path, data).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new PushResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse Delete(string path)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(HttpMethod.Delete, path).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse Update<T>(string path, T data)
        {
            try
            {
                HttpResponseMessage response = _requestManager.RequestAsync(RequestManager.Patch, path, data).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<FirebaseResponse> GetAsync(string path, string query)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.RequestAsync(HttpMethod.Get, path, query).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<FirebaseResponse> GetAsync(string path)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.RequestAsync(HttpMethod.Get, path).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
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
                HttpResponseMessage response = await _requestManager.RequestAsync(HttpMethod.Put, path, data).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new SetResponse(content, response.StatusCode);
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
                HttpResponseMessage response = await _requestManager.RequestAsync(HttpMethod.Post, path, data).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new PushResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse ResetPassword(string email, string password)
        {
            /*
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com/password?&email=tedwards%40hachisoft.com&_method=POST&v=node-2.3.2&transport=json&suppress_status_codes=true
             * */
            try
            {
                string query = string.Format(@"&email={0}&_method=POST&transport=json&suppress_status_codes=true", Uri.EscapeDataString(email));
                string path = string.Format("users/{0}/password", email);

                HttpResponseMessage response = _requestManager.RequestApiAsync(HttpMethod.Post, path, query, null).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse ChangePassword(string email, string oldPassword, string newPassword)
        {
            /*
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com/password?&email=tedwards%40hachisoft.com&oldPassword=oldPassword&newPassword=newPassword&_method=PUT&password=newPassword&v=node-2.3.2&transport=json&suppress_status_codes=true
             * */
            try
            {
                string query = string.Format(@"&email={0}&oldPassword={1}&newPassword={2}&_method=PUT&password={2}&v=node-2.3.2&transport=json&suppress_status_codes=true", Uri.EscapeDataString(email), Uri.EscapeDataString(oldPassword), Uri.EscapeDataString(newPassword));
                string path = string.Format("users/{0}/password",email);

                HttpResponseMessage response = _requestManager.RequestApiAsync(HttpMethod.Put, path, query, null).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse CreateUser(string email, string password)
        {
            try
            {
                string query = string.Format(@"&email={0}&password={1}&_method=POST&v=node-2.3.2&transport=json&suppress_status_codes=true", Uri.EscapeDataString(email),Uri.EscapeDataString(password));
                string path = "users";

                HttpResponseMessage response = _requestManager.RequestApiAsync(HttpMethod.Post, path,query, null).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse RemoveUser(string email, string password)
        {
            /*
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com?&email=tedwards%40hachisoft.com&password=password&_method=DELETE&v=node-2.3.2&transport=json&suppress_status_codes=true
             * */
            try
            {
                string query = string.Format(@"&email={0}&password={1}&_method=DELETE&transport=json&suppress_status_codes=true", Uri.EscapeDataString(email), Uri.EscapeDataString(password));
                string path = string.Format("users/{0}", email);

                HttpResponseMessage response = _requestManager.RequestApiAsync(HttpMethod.Delete, path, query, null).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse ChangeEmail(string oldEmail, string password, string newEmail)
        {
            /*
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com/email?&oldEmail=tedwards%40hachisoft.com&password=password&newEmail=tedwards%40hachisoft.com&_method=PUT&email=tedwards%40hachisoft.com&v=node-2.3.2&transport=json&suppress_status_codes=true
             * */
            try
            {
                string query = string.Format(@"&oldEmail={0}&password={1}&newEmail={2}&_method=PUT&email={2}&v=node-2.3.2&transport=json&suppress_status_codes=true", Uri.EscapeDataString(oldEmail), Uri.EscapeDataString(password), Uri.EscapeDataString(newEmail));
                string path = string.Format("users/{0}/email", oldEmail);

                HttpResponseMessage response = _requestManager.RequestApiAsync(HttpMethod.Put, path, query, null).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        

        public async Task<FirebaseResponse> DeleteAsync(string path)
        {
            try
            {
                HttpResponseMessage response = await _requestManager.RequestAsync(HttpMethod.Delete, path).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
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
                HttpResponseMessage response = await _requestManager.RequestAsync(RequestManager.Patch, path, data).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new FirebaseResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        [Obsolete("This method is obsolete use OnAsync instead.")]
        public async Task<EventStreamResponse> ListenAsync(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return new EventStreamResponse(await _requestManager.ListenAsync(path).ConfigureAwait(false), added, changed, removed);
        }

        public async Task<EventRootResponse<T>> OnChangeGetAsync<T>(string path, ValueRootAddedEventHandler<T> added = null)
        {
            return new EventRootResponse<T>(await _requestManager.ListenAsync(path).ConfigureAwait(false), added, _requestManager, path);
        }

        public async Task<EventStreamResponse> OnAsync(string path, ValueAddedEventHandler added = null, ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return new EventStreamResponse(await _requestManager.ListenAsync(path).ConfigureAwait(false), added, changed, removed);
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler = null)
        {
            if (errorHandler != null)
            {
                errorHandler(statusCode, content);
            }
            else
            {
                _defaultErrorHandler(statusCode, content);
            }
        }
    }
}