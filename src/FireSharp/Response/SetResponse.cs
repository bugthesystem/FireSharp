namespace FireSharp.Response {
    using RestSharp;

    public class SetResponse : FirebaseResponse {
        public SetResponse(IRestResponse response)
            : base(response) {

        }

        public SetResponse() {
        }
    }
}