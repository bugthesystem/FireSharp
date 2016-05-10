using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireSharp.Interfaces
{
    internal interface IRequestManager : IDisposable
    {
        Task<HttpResponseMessage> ListenAsync(string path);
        Task<HttpResponseMessage> ListenAsync(string path, FirebaseQuery query);
        Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, object payload = null);
        Task<HttpResponseMessage> RequestAsync(HttpMethod method, string path, FirebaseQuery query, object payload = null);
        Task<HttpResponseMessage> RequestApiAsync(HttpMethod method, string path, FirebaseQuery query, object payload = null);
    }
}