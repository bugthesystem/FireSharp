using System;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Exceptions;
using FireSharp.Interfaces;
using FireSharp.Tests.Models;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public class FiresharpTests : TestBase
    {
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        private IFirebaseClient _client;

        protected override void FinalizeSetUp()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FIREBASE_SECRET,
                BasePath = BASE_PATH
            };
            _client = new FirebaseClient(config); //Uses RestSharp JsonSerializer as default
            _client.DeleteAsync("todos").Wait();
        }

        [Test, Category("INTEGRATION")]
        public async Task Delete()
        {
            await _client.PushAsync("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            var response = await _client.DeleteAsync("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test, Category("INTEGRATION")]
        public async void Set()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            var response = await _client.SetAsync("todos/set", todo);
            var result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);

            // overwrite the todo we just set
            response = await _client.SetAsync("todos", todo);
            var getResponse = await _client.GetAsync("/todos/set");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Test, Category("INTEGRATION")]
        public async void Push()
        {
            var todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            var response = await _client.PushAsync("todos/push", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, ExpectedException(typeof(FirebaseException)), Category("INTEGRATION")]
        public async void UpdateFailure()
        {
            var response = await _client.UpdateAsync("todos", true);
        }

        [Test, Category("INTEGRATION")]
        public async void Get()
        {
            await _client.PushAsync("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            var response = await _client.GetAsync("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("INTEGRATION")]
        public async void Update()
        {
            await _client.SetAsync("todos/set", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            var todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            var response = await _client.UpdateAsync("todos/set", todoToUpdate);
            Assert.NotNull(response);
            var actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }
    }
}