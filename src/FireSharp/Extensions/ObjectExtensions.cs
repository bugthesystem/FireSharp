using RestSharp;
using RestSharp.Deserializers;

namespace FireSharp.Extensions {
    using RestSharp.Serializers;

    public static class ObjectExtensions {
        public static string ToJson(this object @object) {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Serialize(@object);
        }

        public static T ToTyped<T>(this IRestResponse response) {
            JsonDeserializer serializer = new JsonDeserializer();
            return serializer.Deserialize<T>(response);
        }
    }
}