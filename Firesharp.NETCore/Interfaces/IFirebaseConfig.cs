using System;

namespace FireSharp.NETCore.Interfaces
{
    public interface IFirebaseConfig
    {
        string BasePath { get; set; }
        string Host { get; set; }
        string AuthSecret { get; set; }
        TimeSpan? RequestTimeout { get; set; }
        ISerializer Serializer { get; set; }
    }
}