using System.Net;
using System.Net.Http;

namespace FireSharp.Response
{
    public class DeleteResponse : FirebaseResponse
    {
        public DeleteResponse(string body, HttpStatusCode statusCode, HttpResponseMessage httpResponse)
            : base(body, statusCode, httpResponse)
        {
        }

        public DeleteResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }

        public bool Success
        {
            get
            {
                return StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.NoContent;
            }
        }
    }
}