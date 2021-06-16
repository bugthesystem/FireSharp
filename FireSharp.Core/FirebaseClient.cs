using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FireSharp.Core.EventStreaming;
using FireSharp.Core.Exceptions;
using FireSharp.Core.Interfaces;
using FireSharp.Core.Response;

namespace FireSharp.Core
{
    public class FirebaseClient : IFirebaseClient
    {
        private readonly Action<HttpStatusCode, string> defaultErrorHandler = (statusCode, body) =>
        {
            if (statusCode is < HttpStatusCode.OK or >= HttpStatusCode.BadRequest)
            {
                throw new FirebaseException(statusCode, body);
            }
        };

        private readonly IRequestManager requestManager;


        public FirebaseClient(IFirebaseConfig config)
            : this(new RequestManager(config))
        {
        }

        ~FirebaseClient()
        {
            Dispose(false);
        }

        internal FirebaseClient(IRequestManager requestManager)
        {
            this.requestManager = requestManager;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                requestManager.Dispose();
            }
        }

        public FirebaseResponse Get(string path)
        {
            try
            {
                using (HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Get, path).Result)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse Get(string path, QueryBuilder queryBuilder)
        {
            try
            {
                using (HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Get, path, queryBuilder).Result)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
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
                HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Put, path, data).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                HandleIfErrorResponse(response.StatusCode, content);
                return new SetResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public SetResponse Set<T>(string path, T data, string print)
        {
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New().Print(print);
                HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Put, path, queryBuilder, data).Result;
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
                HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Post, path, data).Result;
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
                HttpResponseMessage response = requestManager.RequestAsync(HttpMethod.Delete, path).Result;
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
                using (HttpResponseMessage response = requestManager.RequestAsync(RequestManager.Patch, path, data).Result)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public FirebaseResponse Update<T>(string path, T data, string print)
        {
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New().Print(print);
                using (HttpResponseMessage response = requestManager.RequestAsync(RequestManager.Patch, path, queryBuilder, data).Result)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<FirebaseResponse> GetAsync(string path, QueryBuilder queryBuilder)
        {
            try
            {
                using (HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Get, path, queryBuilder).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
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
                using (HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Get, path).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
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
                HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Put, path, data).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                return new SetResponse(content, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<SetResponse> SetAsync<T>(string path, T data, string print)
        {
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New().Print(print);
                HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Put, path, queryBuilder, data).ConfigureAwait(false);
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
                using (HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Post, path, data).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new PushResponse(content, response.StatusCode);
                }
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
                QueryBuilder queryBuilder = QueryBuilder.New($@"&email={Uri.EscapeDataString(email)}&_method=POST&transport=json&suppress_status_codes=true")
                    ;
                string path = $"users/{email}/password";

                HttpResponseMessage response = requestManager.RequestApiAsync(HttpMethod.Post, path, queryBuilder).Result;
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
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com/password?&email=tedwards%40hachisoft.com&oldPassword=oldPassword
            &newPassword=newPassword&_method=PUT&password=newPassword&v=node-2.3.2&transport=json&suppress_status_codes=true
             * */
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New(
                    $@"&email={Uri.EscapeDataString(email)}&oldPassword={Uri.EscapeDataString(oldPassword)}&newPassword={Uri
                        .EscapeDataString(newPassword)}&_method=PUT&password={Uri.EscapeDataString(newPassword)}&v=node-2.3.2&transport=json&suppress_status_codes=true");
                string path = $"users/{email}/password";

                HttpResponseMessage response = requestManager.RequestApiAsync(HttpMethod.Put, path, queryBuilder).Result;
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
                QueryBuilder queryBuilder = QueryBuilder.New(
                $@"&email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}&_method=POST&v=node-2.3.2&transport=json&suppress_status_codes=true");
                string path = "users";

                HttpResponseMessage response = requestManager.RequestApiAsync(HttpMethod.Post, path, queryBuilder).Result;
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
                QueryBuilder queryBuilder = QueryBuilder.New($@"&email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}&_method=DELETE&transport=json&suppress_status_codes=true");
                string path = $"users/{email}";

                HttpResponseMessage response = requestManager.RequestApiAsync(HttpMethod.Delete, path, queryBuilder).Result;
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
            /v2/luminous-heat-7934/users/tedwards@hachisoft.com/email?&oldEmail=tedwards%40hachisoft.com&password=password
            &newEmail=tedwards%40hachisoft.com&_method=PUT&email=tedwards%40hachisoft.com&v=node-2.3.2
            &transport=json&suppress_status_codes=true
             * */
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New(string.Format(
                        @"&oldEmail={0}&password={1}&newEmail={2}&_method=PUT&email={2}&v=node-2.3.2&transport=json&suppress_status_codes=true",
                        Uri.EscapeDataString(oldEmail), Uri.EscapeDataString(password), Uri.EscapeDataString(newEmail)));
                string path = $"users/{oldEmail}/email";

                HttpResponseMessage response = requestManager.RequestApiAsync(HttpMethod.Put, path, queryBuilder).Result;
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
                using (HttpResponseMessage response = await requestManager.RequestAsync(HttpMethod.Delete, path).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
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
                using (HttpResponseMessage response = await requestManager.RequestAsync(RequestManager.Patch, path, data).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new FirebaseException(ex);
            }
        }

        public async Task<FirebaseResponse> UpdateAsync<T>(string path, T data, string print)
        {
            try
            {
                QueryBuilder queryBuilder = QueryBuilder.New().Print(print);
                using (HttpResponseMessage response = await requestManager.RequestAsync(RequestManager.Patch, path, queryBuilder, data).ConfigureAwait(false))
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    return new FirebaseResponse(content, response.StatusCode);
                }
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
            return new EventStreamResponse(await requestManager.ListenAsync(path).ConfigureAwait(false), added, changed,
                removed);
        }

        public async Task<EventRootResponse<T>> OnChangeGetAsync<T>(string path,
            ValueRootAddedEventHandler<T> added = null)
        {
            return new EventRootResponse<T>(await requestManager.ListenAsync(path).ConfigureAwait(false), added,
                requestManager, path);
        }

        public async Task<EventStreamResponse> OnAsync(string path, ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null, object context = null)
        {
            return new EventStreamResponse(await requestManager.ListenAsync(path).ConfigureAwait(false), added, changed,
                removed, context);
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content,
            Action<HttpStatusCode, string> errorHandler = null)
        {
            if (errorHandler != null)
            {
                errorHandler(statusCode, content);
            }
            else
            {
                defaultErrorHandler(statusCode, content);
            }
        }
    }
}