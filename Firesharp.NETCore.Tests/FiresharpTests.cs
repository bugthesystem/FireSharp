using FireSharp.NETCore.Config;
using FireSharp.NETCore.Exceptions;
using FireSharp.NETCore.Interfaces;
using FireSharp.NETCore.Tests.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FireSharp.NETCore.Tests
{
    public class FiresharpTests
    {
        private readonly string _basePath = "https://firesharp.firebaseio.com/";
        private readonly string _basePathWithoutSlash = "https://firesharp.firebaseio.com";
        private readonly string _firebaseSecret = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";

        private IFirebaseClient _client { get; set; }

        public FiresharpTests()
        {
            TestSetUp().Wait();
        }

        private async Task TestSetUp()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = _firebaseSecret,
                BasePath = _basePath
            };

            _client = new FirebaseClient(config); //Uses Newtonsoft.Json Json Serializer

            var task1 = _client.DeleteAsync("todos");
            var task2 = _client.DeleteAsync("fakepath");

            await Task.WhenAll(task1, task2);
        }

        [Fact]
        public void Delete()
        {
            _client.Push("todos/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            var response = _client.Delete("todos/push");
            Assert.NotNull(response);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _client.PushAsync("todos/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            }).ConfigureAwait(false);

            var response = await _client.DeleteAsync("todos/pushAsync").ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get()
        {
            _client.Push("todos/gettest/push", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            });

            await Task.Delay(400).ConfigureAwait(false);

            var response = _client.Get("todos/gettest");
            Assert.NotNull(response);
            Assert.True(response.Body.Contains("name"));
            Assert.True(response.Body.Contains("Execute PUSH4GET"));
        }

        [Fact]
        public async Task GetAsync()
        {
            await _client.PushAsync("todos/get/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            }).ConfigureAwait(false);

            var response = await _client.GetAsync("todos/get/").ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.True(response.Body.Contains("name"));
        }

        [Fact]
        public async Task GetListAsync()
        {
            var expected = new List<Todo>
            {
                new Todo {name = "Execute PUSH4GET1", priority = 2},
                new Todo {name = "Execute PUSH4GET2", priority = 2},
                new Todo {name = "Execute PUSH4GET3", priority = 2},
                new Todo {name = "Execute PUSH4GET4", priority = 2},
                new Todo {name = "Execute PUSH4GET5", priority = 2}
            };

            var pushResponse = await _client.PushAsync("todos/list/pushAsync", expected).ConfigureAwait(false);
            var id = pushResponse.Result.Name;

#pragma warning disable 618 // Point of the test
            Assert.Equal(pushResponse.Result.name, pushResponse.Result.Name);
#pragma warning restore 618

            await Task.Delay(400).ConfigureAwait(false);

            var getResponse = await _client.GetAsync(string.Format("todos/list/pushAsync/{0}", id)).ConfigureAwait(false);

            var actual = getResponse.ResultAs<List<Todo>>();

            Assert.NotNull(pushResponse);
            Assert.NotNull(getResponse);
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
        }

        [Fact]
        public async Task OnChangeGetAsync()
        {
            var expected = new Todo { name = "Execute PUSH4GET1", priority = 2 };
            var changes = 0;
            var observer = _client.OnChangeGetAsync<Todo>("fakepath/OnGetAsync/", (events, arg) =>
            {
                Interlocked.Increment(ref changes);
                Assert.NotNull(arg);
                Assert.Equal(expected.name, arg.name);
            });

            await _client.SetAsync("fakepath/OnGetAsync/", expected).ConfigureAwait(false);

            await Task.Delay(2000).ConfigureAwait(false);

            await _client.SetAsync("fakepath/OnGetAsync/name", "PUSH4GET1").ConfigureAwait(false);

            await Task.Delay(2000).ConfigureAwait(false);

            try
            {
                if (changes == 3)
                {
                    // not sure what to do here? throw an exception perhaps?
                    //Assert.Inconclusive();
                }

                Assert.Equal(2, changes);
            }
            finally
            {
                observer.Result.Cancel();
            }
        }

        [Fact]
        public void Push()
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

        [Fact]
        public async Task PushAsync()
        {
            var todo = new Todo
            {
                name = "Execute PUSH4",
                priority = 2
            };

            var response = await _client.PushAsync("todos/push/pushAsync", todo).ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.NotNull(response.Result.Name); /*Returns pushed data name like -J8LR7PDCdz_i9H41kf7*/
            Console.WriteLine(response.Result.Name);
        }

        [Fact]
        public async Task SecondConnectionWithoutSlash()
        {
            // This integration test will write from _config but read from a second Firebase connection to
            // the same DB, but with a BasePath which does not contain the unnecessary trailing slash.
            var secondClientToTest = new FirebaseClient(new FirebaseConfig
            {
                AuthSecret = _firebaseSecret,
                BasePath = _basePathWithoutSlash
            });

            await _client.PushAsync("todos/get/pushAsync", new Todo
            {
                name = "SecondConnectionWithoutSlash",
                priority = 3
            }).ConfigureAwait(false);

            await Task.Delay(400).ConfigureAwait(false);

            var response = await secondClientToTest.GetAsync("todos/get/").ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.True(response.Body.Contains("name"));
            Assert.True(response.Body.Contains("SecondConnectionWithoutSlash"));
        }

        [Fact]
        public void Set()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            var response = _client.Set("todos/set", todo);
            var result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.Equal(todo.name, result.name);

            // overwrite the todo we just set
            response = _client.Set("todos", todo);
            var getResponse = _client.Get("/todos/set");
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Fact]
        public async Task SetAsync()
        {
            var todo = new Todo
            {
                name = "Execute SET",
                priority = 2
            };
            var response = await _client.SetAsync("todos/setAsync", todo).ConfigureAwait(false);
            var result = response.ResultAs<Todo>();
            Assert.NotNull(response);
            Assert.Equal(todo.name, result.name);

            // overwrite the todo we just set
            response = await _client.SetAsync("todos", todo).ConfigureAwait(false);
            var getResponse = await _client.GetAsync("/todos/setAsync").ConfigureAwait(false);
            result = getResponse.ResultAs<Todo>();
            Assert.Null(result);
        }

        [Fact]
        public void Update()
        {
            _client.Set("todos/updatetest/set", new Todo
            {
                name = "Execute SET",
                priority = 2
            });

            var todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            var response = _client.Update("todos/updatetest/set", todoToUpdate);
            Assert.NotNull(response);
            var actual = response.ResultAs<Todo>();
            Assert.Equal(todoToUpdate.name, actual.name);
            Assert.Equal(todoToUpdate.priority, actual.priority);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            await _client.SetAsync("todos/set/setAsync", new Todo
            {
                name = "Execute SET",
                priority = 2
            }).ConfigureAwait(false);

            var todoToUpdate = new Todo
            {
                name = "Execute UPDATE!",
                priority = 1
            };

            var response = await _client.UpdateAsync("todos/set/setAsync", todoToUpdate).ConfigureAwait(false);
            Assert.NotNull(response);
            var actual = response.ResultAs<Todo>();
            Assert.Equal(todoToUpdate.name, actual.name);
            Assert.Equal(todoToUpdate.priority, actual.priority);
        }

        [Fact]
        public void UpdateFailure()
        {
            Assert.Throws<FirebaseException>(() => _client.Update("todos", true));
        }

        [Fact]
        public async Task UpdateFailureAsync()
        {
            await Assert.ThrowsAsync<FirebaseException>(async () =>
            {
                var response = await _client.UpdateAsync("todos", true);
            });
        }

        public async Task GetWithQueryAsync()
        {
            await _client.PushAsync("todos/get/pushAsync", new Todo
            {
                name = "Execute PUSH4GET",
                priority = 2
            }).ConfigureAwait(false);

            await _client.PushAsync("todos/get/pushAsync", new Todo
            {
                name = "You PUSH4GET",
                priority = 2
            }).ConfigureAwait(false);

            await Task.Delay(400).ConfigureAwait(false);

            var response = await _client.GetAsync("todos", QueryBuilder.New().OrderBy("$key").StartAt("Exe")).ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.True(response.Body.Contains("name"));
        }
    }
}