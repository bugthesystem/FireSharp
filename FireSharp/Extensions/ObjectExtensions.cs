using System.Net.Http;
using FireSharp.Interfaces;

namespace FireSharp.Extensions
{
    public static class ObjectExtensions
    {
        private static ISerializer _serializer;

        static ObjectExtensions()
        {
            _serializer = new JsonSerializerWrapper();
        }

        public static ISerializer Serializer
        {
            get { return _serializer; }
            set { _serializer = value; }
        }


        public static string ToJson(this object @object)
        {
            return _serializer.Serialize(@object);
        }

        public static T ReadAs<T>(this HttpResponseMessage response)
        {
            var task = response.Content.ReadAsStringAsync();
            return _serializer.Deserialize<T>(task.Result);
        }
    }
}