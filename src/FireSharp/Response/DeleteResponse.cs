namespace FireSharp.Response
{
    using System.Net;
    using RestSharp;

    public class DeleteResponse : FirebaseResponse
    {
        public DeleteResponse(IRestResponse response)
            : base(response)
        {
        }

        public DeleteResponse()
        {
        }

        public bool Success
        {
            get { return Response.StatusCode == HttpStatusCode.OK || Response.StatusCode == HttpStatusCode.NoContent; }
        }
    }
}