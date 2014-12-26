using System.Net.Http;
using Newtonsoft.Json;

namespace FireSharp.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static T ReadAs<T>(this HttpResponseMessage response)
        {
            var task = response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(task.Result);
        }
    }
}