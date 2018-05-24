using FireSharp.Core.Interfaces;
using Newtonsoft.Json;

namespace FireSharp.Serialization.JsonNet.Core
{
    internal class JsonSerializerWrapper : ISerializer
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}