using System.Net;
using System.Net.Http;
using FireSharp.Extensions;

namespace FireSharp.Response
{
    public class FirebaseResponse
    {
        private readonly HttpStatusCode _statusCode;

        protected readonly HttpResponseMessage HttpResponse;
        private readonly string _body;

        public FirebaseResponse(string body, HttpStatusCode statusCode, HttpResponseMessage httpResponse)
        {
            _statusCode = statusCode;
            _body = body;
            HttpResponse = httpResponse;
        }

        public FirebaseResponse(string body, HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            _body = body;
        }

        public string Body
        {
            get { return _body; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        public virtual T ResultAs<T>()
        {
            return Body.ReadAs<T>();
        }
    }
}
