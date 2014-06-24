namespace FireSharp.Response
{
    using Exceptions;
    using RestSharp;
    using Extensions;

    public class FirebaseResponse
    {
        protected readonly IRestResponse Response;

        public FirebaseResponse(IRestResponse response)
        {
            Response = response;
        }

        public FirebaseResponse()
        {
            Response = null;
        }

        public string Body
        {
            get { return Response.Content; }
        }

        public FirebaseException Exception { get; set; }

        public virtual T ReadAs<T>()
        {
            return Response.ReadAs<T>();
        }
    }
}