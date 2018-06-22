using System;
using System.Threading.Tasks;
using FireSharp.Core.EventStreaming;
using FireSharp.Core.Response;

namespace FireSharp.Core.Interfaces
{
    public interface IFirebaseClient: IDisposable
    {
        Task<FirebaseResponse> GetAsync(string path);
        Task<FirebaseResponse> GetAsync(string path, QueryBuilder queryBuilder);

        Task<EventRootResponse<T>> OnChangeGetAsync<T>(string path, ValueRootAddedEventHandler<T> added = null);
        Task<SetResponse> SetAsync<T>(string path, T data);
        Task<SetResponse> SetAsync<T>(string path, T data, string print);
        Task<PushResponse> PushAsync<T>(string path, T data);
        Task<FirebaseResponse> DeleteAsync(string path);
        Task<FirebaseResponse> UpdateAsync<T>(string path, T data);
        Task<FirebaseResponse> UpdateAsync<T>(string path, T data, string print);
        FirebaseResponse Get(string path, QueryBuilder queryBuilder);
        FirebaseResponse Get(string path);
        SetResponse Set<T>(string path, T data);
        SetResponse Set<T>(string path, T data, string print);
        PushResponse Push<T>(string path, T data);
        FirebaseResponse Delete(string path);
        FirebaseResponse Update<T>(string path, T data);
        FirebaseResponse Update<T>(string path, T data, string print);
        FirebaseResponse CreateUser(string email, string password);
        FirebaseResponse ChangeEmail(string oldEmail, string password, string newEmail);
        FirebaseResponse RemoveUser(string email, string password);
        FirebaseResponse ResetPassword(string email, string password);
        FirebaseResponse ChangePassword(string email, string oldPassword, string newPassword);

        [Obsolete("This method is obsolete use OnAsync instead.")]
        Task<EventStreamResponse> ListenAsync(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null);

        Task<EventStreamResponse> OnAsync(string path,
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null,
            object context = null);
    }
}