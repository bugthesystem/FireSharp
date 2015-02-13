using FireSharp.Interfaces;

namespace FireSharp.Config
{
    public interface IFirebaseConfig
    {
        string BasePath { get; set; }
        string AuthSecret { get; set; }
        int TimeoutInMinute { get; set; }
        ISerializer Serializer { get; set; }
    }
}