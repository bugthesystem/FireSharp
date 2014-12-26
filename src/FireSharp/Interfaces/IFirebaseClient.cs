using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Response;

namespace FireSharp.Interfaces
{
    public interface IFirebaseClient
    {
        FirebaseResponse Get(string path);
        SetResponse Set<T>(string path, T data);
        PushResponse Push<T>(string path, T data);
        DeleteResponse Delete(string path);
        FirebaseResponse Update<T>(string path, T data);

        FirebaseResponse Listen(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null);

        Task<FirebaseResponse> GetTaskAsync(string path);
        Task<SetResponse> SetTaskAsync<T>(string path, T data);
        Task<PushResponse> PushTaskAsync<T>(string path, T data);
        Task<DeleteResponse> DeleteTaskAsync(string path);
        Task<FirebaseResponse> UpdateTaskAsync<T>(string path, T data);

        Task<FirebaseResponse> ListenAsync(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null);
    }
}