using FireSharp.Config;
using FireSharp.Interfaces;
using NUnit.Framework;

namespace FireSharp.Tests {
    [TestFixture]
    public partial class FirebaseClientTests {
        private const string BASE_PATH = "https://firesharp.firebaseio.com/";
        private const string FIREBASE_SECRET = "**your firebase secret here**";
        IFirebaseClient _client;

        [SetUp]
        public void Setup() {
            IFirebaseConfig config = new FirebaseConfig {
                AuthSecret = FIREBASE_SECRET, BasePath = BASE_PATH
            };
            _client = new FirebaseClient(config);
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
                name = "Do your homework2",
                priority = 3
            };
            var response = _client.Push("todos", todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test]
        public void Get_Test() {
            var response = _client.Get("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }
        
        [Test]
        public void Update_Test() {
            var todo = new Todo {
                name = "Do your homework now!",
                priority = 1
            };

            var response = _client.Update("todos/-J8HaQrYd9xMUZLF5NxI",todo);
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }
    }
}