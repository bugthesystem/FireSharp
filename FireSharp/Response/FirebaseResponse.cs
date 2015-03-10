using System.Net.Http;
using FireSharp.Extensions;

namespace FireSharp.Response
{
    public class FirebaseResponse
    {
        protected readonly HttpResponseMessage HttpResponse;

        public FirebaseResponse(HttpResponseMessage httpResponse)
        {
            HttpResponse = httpResponse;
        }
        public string Body
        {
            get { return HttpResponse.Content.ReadAsStringAsync().Result; }
        }

        public virtual T ResultAs<T>()
        {
            return HttpResponse.ReadAs<T>();
        }
    }
}
