using System;
using FireSharp.Core.Interfaces;

namespace FireSharp.Core.Config
{
    public class FirebaseConfig : IFirebaseConfig
    {
        private string _basePath;

        public FirebaseConfig()
        {
            Serializer = new JsonNetSerializer();
        }

        public string BasePath
        {
            get
            {
                return _basePath.EndsWith("/") ? _basePath : $"{_basePath}/";
            }
            set { _basePath = value; }
        }

        public string Host { get; set; }
        public string AuthSecret { get; set; }
        public string AccessToken { get; set; }

        public TimeSpan? RequestTimeout { get; set; }

        public ISerializer Serializer { get; set; }
    }
}
