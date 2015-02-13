using FireSharp.Interfaces;
using Newtonsoft.Json;

namespace FireSharp.Serialization.JsonNet
{
    internal class JsonSerializerWrapper : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}