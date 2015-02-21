using System.Net;
using System.Net.Http;

namespace FireSharp.Response
{
    public class DeleteResponse : FirebaseResponse
    {
        public DeleteResponse(HttpResponseMessage httpResponse)
            : base(httpResponse)
        {
        }

        public bool Success
        {
            get
            {
                return HttpResponse.StatusCode == HttpStatusCode.OK ||
                       HttpResponse.StatusCode == HttpStatusCode.NoContent;
            }
        }
    }
}