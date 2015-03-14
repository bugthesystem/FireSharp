using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireSharp.Interfaces
{
    internal interface IRequestManager :IDisposable
    {
        Task<HttpResponseMessage> ListenAsync(string path);
        Task<HttpResponseMessage> GetAsync(string path);
        Task<HttpResponseMessage> PutAsync<T>(string path, T data);
        Task<HttpResponseMessage> PostAsync<T>(string path, T data);
        Task<HttpResponseMessage> DeleteAsync(string path);
        Task<HttpResponseMessage> PatchAsync<T>(string path, T data);
        HttpResponseMessage Listen(string path);
        HttpResponseMessage Get(string path);
        HttpResponseMessage Put<T>(string path, T data);
        HttpResponseMessage Post<T>(string path, T data);
        HttpResponseMessage Delete(string path);
        HttpResponseMessage Patch<T>(string path, T data);
    }
}