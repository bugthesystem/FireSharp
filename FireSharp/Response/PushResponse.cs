using System.Net;
using System.Net.Http;

namespace FireSharp.Response
{
    public class PushResponse : FirebaseResponse
    {
        public PushResponse(string body, HttpStatusCode statusCode, HttpResponseMessage httpResponse)
            : base(body, statusCode, httpResponse)
        {
        }

        public PushResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }

        public PushResult Result
        {
            get { return ResultAs<PushResult>(); }
        }
    }

    public class PushResult
    {
        public string Name { get; set; }
    }
}