using FireSharp.Core.Interfaces;
using Newtonsoft.Json;

namespace FireSharp.Serialization.JsonNet.Core
{
    internal class JsonSerializerWrapper : ISerializer
    {
        private static readonly JsonSerializerSettings Settings = new() { DateParseHandling = DateParseHandling.None };

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }
    }
}