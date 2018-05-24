using FireSharp.Core.Interfaces;

namespace FireSharp.Core.Extensions
{
    public static class ObjectExtensions
    {
        static ObjectExtensions()
        {
            Serializer = new JsonNetSerializer();
        }

        public static ISerializer Serializer { get; set; }


        public static string ToJson(this object @object)
        {
            return Serializer.Serialize(@object);
        }

        public static T ReadAs<T>(this string content)
        {
            return Serializer.Deserialize<T>(content);
        }
    }
}
