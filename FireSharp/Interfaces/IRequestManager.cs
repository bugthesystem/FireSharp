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
    }
}