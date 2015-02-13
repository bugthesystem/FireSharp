using FireSharp.Interfaces;

namespace FireSharp.Config
{
    public class FirebaseConfig : IFirebaseConfig
    {
        public FirebaseConfig()
        {
            TimeoutInMinute = 1;
            Serializer = new JsonSerializerWrapper();
        }

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }
        public int TimeoutInMinute { get; set; }

        public ISerializer Serializer { get; set; }
    }
}
