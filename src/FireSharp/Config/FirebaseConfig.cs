namespace FireSharp.Config
{
    using RestSharp.Serializers;

    public class FirebaseConfig : IFirebaseConfig
    {
        private ISerializer _serializer;

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new JsonSerializer()); }
            set { _serializer = value ?? new JsonSerializer(); }
        }
    }
}
