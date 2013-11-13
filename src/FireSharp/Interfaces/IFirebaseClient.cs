namespace FireSharp.Interfaces {
    using System.Threading.Tasks;
    using Response;

    public interface IFirebaseClient {
        FirebaseResponse Get(string path);
        FirebaseResponse Set<T>(string path, T data);
        FirebaseResponse Push<T>(string path, T data);
        FirebaseResponse Delete(string path);
        FirebaseResponse Update<T>(string path, T data);

        Task<FirebaseResponse> GetTaskAsync(string path);
        Task<FirebaseResponse> SetTaskAsync<T>(string path, T data);
        Task<FirebaseResponse> PushTaskAsync<T>(string path, T data);
        Task<FirebaseResponse> DeleteTaskAsync(string path);
        Task<FirebaseResponse> UpdateTaskAsync<T>(string path, T data);
    }
}