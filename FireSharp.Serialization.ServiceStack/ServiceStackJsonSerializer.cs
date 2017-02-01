using FireSharp.Interfaces;
using ServiceStack.Text;

namespace FireSharp.Serialization.ServiceStack
{
    /// <summary>
    ///     ServiceSatck JSON serializer for request bodies
    ///     ServiceStack.Text supports DataContract,DataMember attributes
    /// </summary>
    public class ServiceStackJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.DeserializeFromString<T>(json);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToString(value);
        }
    }
}