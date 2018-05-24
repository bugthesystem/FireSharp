using System.Net;

namespace FireSharp.Core.Response
{
    public class SetResponse : FirebaseResponse
    {
        public SetResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }
    }
}
