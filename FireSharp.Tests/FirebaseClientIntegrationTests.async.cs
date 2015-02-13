using FireSharp.Response;
using FireSharp.Tests.Models;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public partial class FirebaseClientIntegrationTests
    {
        private const string TODOS_ASYNC_PATH = "todos/async";

        [Test, Category("INTEGRATION_ASYNC")]
        public async void PushAsyncTest()
        {
            var todo = new Todo
            {
                name = "Do your homework",
                priority = 1
            };

            PushResponse response = await _client.PushTaskAsync(string.Format("{0}", TODOS_ASYNC_PATH), todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("INTEGRATION_ASYNC")]
        public async void PushChatAsyncTest()
        {
            PushResponse response = await _client.PushTaskAsync("chat", new {name = "ziyasal", text = "Hello there"});
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }
    }
}