using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Exceptions;
using FireSharp.Interfaces;
using FireSharp.Response;
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

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task DeleteAsync()
        {
            await _client.PushAsync("todos/pushAync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            var response = await _client.DeleteAsync("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async void SetAsync()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            var response = await _client.SetAsync("todos/setAsync", todo);
            var result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);

            // overwrite the todo we just set
            response = await _client.SetAsync("todos", todo);
            var getResponse = await _client.GetAsync("/todos/setAsync");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async void PushAsync()
        {
            var todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            var response = await _client.PushAsync("todos/pushAsync", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, ExpectedException(typeof(FirebaseException)), Category("INTEGRATION"), Category("ASYNC")]
        public async void UpdateFailureAsync()
        {
            var response = await _client.UpdateAsync("todos", true);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async void GetAsync()
        {
            await _client.PushAsync("todos/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            var response = await _client.GetAsync("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public async void Update()
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

            var response = _client.Update("todos/set", todoToUpdate);
            Assert.NotNull(response);
            var actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }


        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async void UpdateAsync()
        {
            await _client.SetAsync("todos/setAsync", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            var todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            var response = await _client.UpdateAsync("todos/setAsync", todoToUpdate);
            Assert.NotNull(response);
            var actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }


        [Test, Category("INTEGRATION")]
        public void Delete()
        {
            _client.Push("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            var response = _client.Delete("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Success);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public async void Set()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            var response = _client.Set("todos/set", todo);
            var result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);

            // overwrite the todo we just set
            response = _client.Set("todos", todo);
            var getResponse = _client.Get("/todos/set");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public async void Push()
        {
            var todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            var response = _client.Push("todos/push", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, ExpectedException(typeof(FirebaseException)), Category("INTEGRATION"), Category("SYNC")]
        public async void UpdateFailure()
        {
            var response = _client.Update("todos", true);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public async void Get()
        {
            _client.Push("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            var response = _client.Get("todos");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }


        [Test, Category("INTEGRATION")]
        public async void Get_List()
        {
            List<Todo> expected = new List<Todo>
            {
                new Todo {name = "Execute PUSH4GET1", priority = 2},
                new Todo {name = "Execute PUSH4GET2", priority = 2},
                new Todo {name = "Execute PUSH4GET3", priority = 2},
                new Todo {name = "Execute PUSH4GET4", priority = 2},
                new Todo {name = "Execute PUSH4GET5", priority = 2}
            };

            PushResponse pushResponse = await _client.PushAsync("todos/list", expected);
            string id = pushResponse.Result.Name;

            Thread.Sleep(400);

            var getResponse = await _client.GetAsync(string.Format("todos/list/{0}", id));

            List<Todo> actual = getResponse.ResultAs<List<Todo>>();

            Assert.NotNull(pushResponse);
            Assert.NotNull(getResponse);
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
        }
    }
}
