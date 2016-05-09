using System.Net;
using System.Net.Http;
using FireSharp.Extensions;

namespace FireSharp.Response
{
    public class FirebaseResponse
    {
        protected readonly HttpResponseMessage HttpResponse;

        public FirebaseResponse(string body, HttpStatusCode statusCode, HttpResponseMessage httpResponse)
        {
            StatusCode = statusCode;
            Body = body;
            HttpResponse = httpResponse;
        }

        public FirebaseResponse(string body, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public string Body { get; }

        public HttpStatusCode StatusCode { get; }

        public virtual T ResultAs<T>()
        {
            return Body.ReadAs<T>();
        }
    }
}