using System;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Tests.Models;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public partial class FirebaseClientIntegrationTests : FirebaseClientIntegrationTestBase
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
            _client = new FirebaseClient(config); //Uses RestSharp JsonSerializer as default
            _client.Delete("todos");
        }

        [Test, Category("INTEGRATION")]
        public void Delete()
        {
            _client.Push("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            DeleteResponse response = _client.Delete("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Success);
        }


        [Test, Category("INTEGRATION")]
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

        [Test, Category("INTEGRATION")]
        public void Push()
        {
            var todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            PushResponse response = _client.Push("todos/push", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, Category("INTEGRATION")]
        public void Get()
        {
            _client.Push("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            var response = _client.Get("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("INTEGRATION")]
        public void Update()
        {
            _client.Set("todos/set", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            var todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            FirebaseResponse response = _client.Update("todos/set", todoToUpdate);
            Assert.NotNull(response);
            Todo actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }
    }
}