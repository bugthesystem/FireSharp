using System.Net.Http;
using System.Threading.Tasks;

namespace FireSharp.Interfaces
{
    internal interface IFirebaseRequestManager
    {
        Task<HttpResponseMessage> Get(string path);
        Task<HttpResponseMessage> Put<T>(string path, T data);
        Task<HttpResponseMessage> Post<T>(string path, T data);
        Task<HttpResponseMessage> Delete(string path);
        Task<HttpResponseMessage> Patch<T>(string path, T data);
        Task<HttpResponseMessage> GetStreaming(string path);
        Task<HttpResponseMessage> GetTaskAsync(string path);
        Task<HttpResponseMessage> PutTaskAsync<T>(string path, T data);
        Task<HttpResponseMessage> PostTaskAsync<T>(string path, T data);
        Task<HttpResponseMessage> DeleteTaskAsync(string path);
        Task<HttpResponseMessage> PatchTaskAsync<T>(string path, T data);
    }
}