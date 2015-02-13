using Newtonsoft.Json;

namespace FireSharp.Interfaces
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