namespace FireSharp.Extensions {
    using RestSharp.Serializers;

    public static class ObjectExtensions {
        public static string ToJson(this object @object) {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Serialize(@object);
        }
    }
}