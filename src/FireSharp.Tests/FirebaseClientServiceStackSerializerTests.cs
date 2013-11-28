using System.Runtime.Serialization;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Serialization.ServiceStack;
using NUnit.Framework;

namespace FireSharp.Tests {

    public class FirebaseClientServiceStackSerializerTests : FirebaseClientIntegrationTestBase {
        IFirebaseClient _client;

        [SetUp]
        public void Setup() {
            IFirebaseConfig config = new FirebaseConfig {
                AuthSecret = FIREBASE_SECRET, BasePath = BASE_PATH
            };
            _client = GetClientWithSerializer(new ServiceStackJsonSerializer(), config);
        }

        [Test, Category("SERVICESTACK")]
        public void Set() {
            var todo = new TodoAttributed {
                Name = "Execute SET (ServiceStack)",
                Priority = 2
            };
            SetResponse response = _client.Set("todos/set", todo);
            TodoAttributed result = response.ResultAs<TodoAttributed>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.Name, result.Name);
        }
    }

    //ServiceStack.Text supports DataContract and DataMemeber attributes, you can use these parameters to serilize fields given names
    [DataContract]
    public class TodoAttributed {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "priority")]
        public int Priority { get; set; }
    }
}