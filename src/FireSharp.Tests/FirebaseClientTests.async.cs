using FireSharp.Response;
using NUnit.Framework;

namespace FireSharp.Tests {

    public partial class FirebaseClientTests {
        private const string TODOS_ASYNC_PATH = "todos/async";

        [Test, Category("ASYNC")]
        public async void PushAsyncTest() {
            var todo = new Todo {
                name = "Do your homewrok",
                priority = 1
            };

            PushResponse response = await _client.PushTaskAsync(string.Format("{0}", TODOS_ASYNC_PATH), todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("ASYNC")]
        public async void PushChatAsyncTest() {
            PushResponse response = await _client.PushTaskAsync("chat", new { name = "ziyasal", text = "Hello vogu" });
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }
    }
}