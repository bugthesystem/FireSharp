using System.Net.Http;

namespace FireSharp.Response
{
    public class SetResponse : FirebaseResponse
    {
        public SetResponse(HttpResponseMessage httpResponse)
            : base(httpResponse)
        {
        }
    }
}