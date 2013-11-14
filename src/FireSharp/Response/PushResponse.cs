namespace FireSharp.Response {
    using RestSharp;

    public class PushResponse : FirebaseResponse {
        public PushResponse(IRestResponse response)
            : base(response) {

        }

        public PushResponse() {
        }

        public PushResult Result {
            get {
                return ResultAs<PushResult>();
            }
        }
    }

    public class PushResult {
        public string Name { get; set; }
    }
}