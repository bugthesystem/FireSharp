using FireSharp.Interfaces;
using NUnit.Framework;

namespace FireSharp.Tests {
    [TestFixture]
    public partial class FirebaseClientTests {
        private const string BASEASE_URI = "https://firesharp.firebaseio.com/";
        private const string FIREBASE_SECRET = "**your firebase secret here**";
        IFirebaseClient _client;

        [SetUp]
        public void Setup() {
            _client = new FirebaseClient(BASEASE_URI, FIREBASE_SECRET);
        }

        [Test]
        public void Delete_Test() {
            var response = _client.Delete("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test]
        public void Push_Test() {
            var todo = new Todo {
                name = "Clean room",
                priority = 2
            };
            var response = _client.Push("todos", todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Raw.Contains("name"));
        }

        [Test]
        public void Get_Test() {
            var response = _client.Get("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Raw.Contains("name"));
        }
    }
}