using FireSharp.Extensions;

namespace FireSharp.Response {
    using System.Net;
    using Exceptions;
    using RestSharp;

    public class FirebaseResponse {
        private readonly IRestResponse _response;

        public FirebaseResponse(IRestResponse response) {
            _response = response;
        }

        public FirebaseResponse() {
            _response = null;
        }

        public string Body {
            get {
                return _response.Content;
            }
        }

        public bool Success {
            get {
                return _response.StatusCode == HttpStatusCode.OK || _response.StatusCode == HttpStatusCode.NoContent;

            }
        }

        public T ReadAs<T>() {
            return _response.ToTyped<T>();
        }

        public FirebaseException Exception { get; set; }
    }
}