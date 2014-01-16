namespace FireSharp.Config
{
    using RestSharp.Serializers;
    using System;

    public class FirebaseConfig : IFirebaseConfig
    {
        private ISerializer _serializer;

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }

        public ISerializer Serializer
        {
            get { return _serializer; }
            set {_serializer = value != null ? value : new JsonSerializer(); }
        }
    }
}
