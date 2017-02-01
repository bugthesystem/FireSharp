using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Testing.NUnit;
using FireSharp.Exceptions;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Tests.Models;
using Moq;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public class FirebaseClientTests : TestBase
    {
        private Todo _expected;
        private HttpResponseMessage _expectedResponse;
        private HttpResponseMessage _failureResponse;
        private FirebaseClient _firebaseClient;
        private Mock<IRequestManager> _firebaseRequestManagerMock;

        protected override void FinalizeSetUp()
        {
            _expected = new Todo
            {
                name = "Do your homework!",
                priority = 1
            };

            _firebaseRequestManagerMock = MockFor<IRequestManager>();

            _expectedResponse = new HttpResponseMessage
            {
                Content = new StringContent(_expected.ToJson()),
                StatusCode = HttpStatusCode.OK
            };
            _failureResponse = new HttpResponseMessage
            {
                Content = new StringContent("error"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            _firebaseClient = new FirebaseClient(_firebaseRequestManagerMock.Object);
        }

        [Test]
        public async Task DeleteAsync()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Delete, "todos", (object) null))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.DeleteAsync("todos");
            Assert.NotNull(response);
        }

        [Test]
        public void DeleteAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Delete, "todos", (object) null)),
                _failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.DeleteAsync("todos"));
        }

        [Test]
        public async Task GetAsync()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null))
                .Returns(Task.FromResult(_expectedResponse));

            var firebaseResponse = await _firebaseClient.GetAsync("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void GetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null)),
                _failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.GetAsync("todos"));
        }

        [Test]
        public void GetFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null)),
                _failureResponse);

            Assert.Throws<FirebaseException>(() => _firebaseClient.Get("todos"));
        }

        [Test]
        public async void PushAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.PushAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void PushAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "todos", _expected)),
                _failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.PushAsync("todos", _expected));
        }

        [Test]
        public async void SetAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.SetAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void SetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "todos", _expected)),
                _failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.SetAsync("todos", _expected));
        }

        [Test]
        public async Task UpdateAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.UpdateAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void UpdateAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "todos", _expected)),
                _failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.UpdateAsync("todos", _expected));
        }
    }
}