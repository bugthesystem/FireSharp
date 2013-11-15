using RestSharp.Serializers;

namespace FireSharp.Config {
    public interface IFirebaseConfig {
        string BasePath { get; set; }
        string AuthSecret { get; set; }
        ISerializer Serializer { get; set; }
    }
}