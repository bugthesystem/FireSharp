namespace FireSharp.Interfaces {
    using System.Threading.Tasks;
    using RestSharp;

    internal interface IFirebaseRequestManager {
        IRestResponse Get(string path);
        IRestResponse Put<T>(string path, T data);
        IRestResponse Post<T>(string path, T data);
        IRestResponse Delete(string path);
        IRestResponse Patch<T>(string path, T data);

        Task<IRestResponse> GetTaskAsync(string path);
        Task<IRestResponse> PutTaskAsync<T>(string path, T data);
        Task<IRestResponse> PostTaskAsync<T>(string path, T data);
        Task<IRestResponse> DeleteTaskAsync(string path);
        Task<IRestResponse> PatchTaskAsync<T>(string path, T data);
    }
}