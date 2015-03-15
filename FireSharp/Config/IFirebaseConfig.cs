using System;
using FireSharp.Interfaces;

namespace FireSharp.Config
{
    public interface IFirebaseConfig
    {
        string BasePath { get; set; }
        string AuthSecret { get; set; }
        TimeSpan? RequestTimeout { get; set; }
        ISerializer Serializer { get; set; }
    }
}