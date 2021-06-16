using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FireSharp.Core;
using FireSharp.Core.Config;
using FireSharp.Core.Exceptions;
using FireSharp.Core.Interfaces;
using FireSharp.Core.Response;
using FireSharp.Tests.Core.Commom;
using FireSharp.Tests.Core.Models;
using NUnit.Framework;

namespace FireSharp.Tests.Core
{
    [TestFixture]
    public class FiresharpTests : TestBase
    {
        protected const string BasePath = "https://firesharp.firebaseio.com/";
        protected const string BasePathWithoutSlash = "https://firesharp.firebaseio.com";
        protected const string FirebaseSecret = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        private IFirebaseClient _client;

        [SetUp]
        public async Task TestFixtureSetUp()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            _client = new FirebaseClient(config); //Uses Newtonsoft.Json Json Serializer

            Task<FirebaseResponse> task1 = _client.DeleteAsync("TODOs");
            Task<FirebaseResponse> task2 = _client.DeleteAsync("fakepath");

            await Task.WhenAll(task1, task2);
        }

        protected override void FinalizeSetUp()
        {
        }

        [Test, Category("INTEGRATION")]
        public void Delete()
        {
            _client.Push("TODOs/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            FirebaseResponse response = _client.Delete("TODOs/push");
            Assert.NotNull(response);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task DeleteAsync()
        {
            await _client.PushAsync("TODOs/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            FirebaseResponse response = await _client.DeleteAsync("TODOs/pushAsync");
            Assert.NotNull(response);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public void Get()
        {
            _client.Push("TODOs/gettest/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            FirebaseResponse response = _client.Get("TODOs/gettest");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
            Assert.IsTrue(response.Body.Contains("Execute PUSH4GET"));
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task GetAsync()
        {
            await _client.PushAsync("TODOs/get/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            FirebaseResponse response = await _client.GetAsync("TODOs/get/");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }

        [Test, Category("INTEGRATION")]
        public async Task GetListAsync()
        {
            List<Todo> expected = new List<Todo>
            {
                new Todo {name = "Execute PUSH4GET1", priority = 2},
                new Todo {name = "Execute PUSH4GET2", priority = 2},
                new Todo {name = "Execute PUSH4GET3", priority = 2},
                new Todo {name = "Execute PUSH4GET4", priority = 2},
                new Todo {name = "Execute PUSH4GET5", priority = 2}
            };

            PushResponse pushResponse = await _client.PushAsync("TODOs/list/pushAsync", expected);
            string id = pushResponse.Result.Name;


#pragma warning disable 618 // Point of the test
            Assert.AreEqual(pushResponse.Result.name, pushResponse.Result.Name);
#pragma warning restore 618

            Thread.Sleep(400);

            FirebaseResponse getResponse = await _client.GetAsync($"TODOs/list/pushAsync/{id}");

            List<Todo> actual = getResponse.ResultAs<List<Todo>>();

            Assert.NotNull(pushResponse);
            Assert.NotNull(getResponse);
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [Test, Category("INTEGRATION")]
        public async Task OnChangeGetAsync()
        {
            Todo expected = new Todo { name = "PUSH4GET1", priority = 2 };
            int changes = 0;
            Task<EventRootResponse<Todo>> observer = _client.OnChangeGetAsync<Todo>("fakepath/OnGetAsync/", (events, arg) =>
            {
                Interlocked.Increment(ref changes);
                Assert.NotNull(arg);
                Assert.AreEqual(expected.name, arg.name);
            });

            await _client.SetAsync("fakepath/OnGetAsync/", expected);

            await Task.Delay(2000);

            await _client.SetAsync("fakepath/OnGetAsync", new Todo { name = "PUSH4GET1", priority = 2 });

            await Task.Delay(2000);

            try
            {
                if (changes == 3)
                {
                    Assert.Inconclusive();
                }

                Assert.AreEqual(2, changes);
            }
            finally
            {
                observer.Result.Cancel();
            }
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public void Push()
        {
            Todo todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            PushResponse response = _client.Push("TODOs/push", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task PushAsync()
        {
            Todo todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            PushResponse response = await _client.PushAsync("TODOs/push/pushAsync", todo);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task SecondConnectionWithoutSlash()
        {
            // This integration test will write from _config but read from a second Firebase connection to
            // the same DB, but with a BasePath which does not contain the unnecessary trailing slash.
            FirebaseClient secondClientToTest = new FirebaseClient(new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePathWithoutSlash
            });

            await _client.PushAsync("TODOs/get/pushAsync", new Todo
            {
                name = "SecondConnectionWithoutSlash",
                priority = 3
            });

            Thread.Sleep(400);

            FirebaseResponse response = await secondClientToTest.GetAsync("TODOs/get/");
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
            Assert.IsTrue(response.Body.Contains("SecondConnectionWithoutSlash"));
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public void Set()
        {
            Todo todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            SetResponse response = _client.Set("TODOs/set", todo);
            Todo result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);

            // overwrite the todo we just set
            response = _client.Set("TODOs", todo);
            FirebaseResponse getResponse = _client.Get("/TODOs/set");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task SetAsync()
        {
            Todo todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            SetResponse response = await _client.SetAsync("TODOs/setAsync", todo);
            Todo result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.AreEqual(todo.name, result.name);

            // overwrite the todo we just set
            response = await _client.SetAsync("TODOs", todo);
            FirebaseResponse getResponse = await _client.GetAsync("/TODOs/setAsync");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public void Update()
        {
            _client.Set("TODOs/updatetest/set", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            Todo todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            FirebaseResponse response = _client.Update("TODOs/updatetest/set", todoToUpdate);
            Assert.NotNull(response);
            Todo actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task UpdateAsync()
        {
            await _client.SetAsync("TODOs/set/setAsync", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            Todo todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            FirebaseResponse response = await _client.UpdateAsync("TODOs/set/setAsync", todoToUpdate);
            Assert.NotNull(response);
            Todo actual = response.ResultAs<Todo>();
            Assert.AreEqual(todoToUpdate.name, actual.name);
            Assert.AreEqual(todoToUpdate.priority, actual.priority);
        }

        [Test, Category("INTEGRATION"), Category("SYNC")]
        public void UpdateFailure()
        {
            Assert.Throws<FirebaseException>(() =>
           {
               FirebaseResponse response = _client.Update("TODOs", true);
           });

        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task UpdateFailureAsync()
        {
            await AssertExtensions.ThrowsAsync<FirebaseException>(async () =>
            {
                FirebaseResponse response = await _client.UpdateAsync("TODOs", true);
            });
        }

        [Test, Category("INTEGRATION"), Category("ASYNC")]
        public async Task GetWithQueryAsync()
        {
            await _client.PushAsync("TODOs/get/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            await _client.PushAsync("TODOs/get/pushAsync", new Todo
            {
                name = "You PUSH4GET",
                priority = 2
            });

            Thread.Sleep(400);

            FirebaseResponse response = await _client.GetAsync("TODOs", QueryBuilder.New().OrderBy("$key").StartAt("Exe"));
            Assert.NotNull(response);
            Assert.IsTrue(response.Body.Contains("name"));
        }
    }
}