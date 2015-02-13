using System.Net.Http;

namespace FireSharp.Response
{
    public class PushResponse : FirebaseResponse
    {
        public PushResponse(HttpResponseMessage httpResponse)
            : base(httpResponse)
        {
        }

        public PushResponse()
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