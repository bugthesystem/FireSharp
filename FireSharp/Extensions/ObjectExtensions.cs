using System.Net.Http;
using FireSharp.Interfaces;

namespace FireSharp.Extensions
{
    public static class ObjectExtensions
    {
        private static ISerializer _serializer;

        static ObjectExtensions()
        {
            _serializer = new JsonNetSerializer();
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
            var json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return _serializer.Deserialize<T>(json);
        }

        public static T ReadAs<T>(this string content)
        {
            return _serializer.Deserialize<T>(content);
        }
    }
}