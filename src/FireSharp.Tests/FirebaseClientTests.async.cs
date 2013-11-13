using FireSharp.Interfaces;
using NUnit.Framework;

namespace FireSharp.Tests {
    [TestFixture]
    public partial class FirebaseClientTests {

        [Test]
        public async void Push_Async_Test() {
            var todo = new Todo {
                name = "Do your homewrok",
                priority = 1
            };
            var response = await _client.PushTaskAsync("todos", todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Raw.Contains("name"));
        }
    }
}