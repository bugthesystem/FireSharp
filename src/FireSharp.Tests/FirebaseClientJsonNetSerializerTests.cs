using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Serialization.JsonNet;
using FireSharp.Tests.Models;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public class FirebaseClientJsonNetSerializerTests : FirebaseClientIntegrationTestBase
    {
        private IFirebaseClient _client;

        [SetUp]
        public void Setup()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FIREBASE_SECRET,
                BasePath = BASE_PATH
            };
            _client = GetClientWithSerializer(new JsonNetSerializer(), config);
        }

        [Test, Category("JSONNET")]
        public void Set()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            SetResponse response = _client.Set("todos/set", todo);
            Todo result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);
        }
    }
}