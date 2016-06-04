using System.Net;

namespace FireSharp.Response
{
    public class PushResponse : FirebaseResponse
    {
        public PushResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }

        public PushResult Result => ResultAs<PushResult>();
    }

    public class PushResult
    {
        public string name { get; set; }
    }
}
