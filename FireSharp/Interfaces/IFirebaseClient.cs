using System;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Response;

namespace FireSharp.Interfaces
{
    public interface IFirebaseClient
    {
        Task<FirebaseResponse> GetAsync(string path);

        Task<EventRootResponse<T>> OnChangeGetAsync<T>(string path, ValueRootAddedEventHandler<T> added = null);
        Task<SetResponse> SetAsync<T>(string path, T data);
        Task<PushResponse> PushAsync<T>(string path, T data);
        Task<DeleteResponse> DeleteAsync(string path);
        Task<FirebaseResponse> UpdateAsync<T>(string path, T data);
        FirebaseResponse Get(string path);
        SetResponse Set<T>(string path, T data);
        PushResponse Push<T>(string path, T data);
        DeleteResponse Delete(string path);
        FirebaseResponse Update<T>(string path, T data);

        [Obsolete("This method is obsolete use OnAsync instead.")]
        Task<FirebaseResponse> ListenAsync(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null);

        Task<FirebaseResponse> OnAsync(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null);
    }
}