namespace FireSharp.Config {
    using RestSharp.Serializers;
    using System;

    public class FirebaseConfig : IFirebaseConfig {
        private ISerializer _serializer;

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }

        public ISerializer Serializer {
            get { return _serializer ?? (_serializer = new JsonSerializer()); }
            set {
                if (_serializer != null) _serializer = value;
                else { throw new ArgumentNullException("value"); }
            }
        }
    }
}