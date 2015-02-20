using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        private IFirebaseClient _firebaseClient;
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
            _failureResponse = new HttpResponseMessage {
                Content = new StringContent("error"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            _firebaseClient = new FirebaseClient(_firebaseRequestManagerMock.Object);
        }

        [Test]
        public async void Push()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PostAsync("todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.PushAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void PushFailure()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PostAsync("todos", _expected))
                .ReturnsAsync(_failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.PushAsync("todos", _expected));
        }

        [Test]
        public async void Set()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PutAsync("todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.SetAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void SetFailure()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PutAsync("todos", _expected))
                .ReturnsAsync(_failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.SetAsync("todos", _expected));
        }

        [Test]
        public async void Get()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.GetAsync("todos"))
                .Returns(Task.FromResult(_expectedResponse));

            var firebaseResponse = await _firebaseClient.GetAsync("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void GetFailure()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.GetAsync("todos"))
                .ReturnsAsync(_failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.GetAsync("todos"));
        }

        [Test]
        public async void Delete()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.DeleteAsync("todos"))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.DeleteAsync("todos");
            Assert.NotNull(response);
            Assert.AreEqual(response.Success, true);
        }

        [Test]
        public void DeleteFailure()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.DeleteAsync("todos"))
                .ReturnsAsync(_failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.DeleteAsync("todos"));
        }

        [Test]
        public async void Update()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PatchAsync("todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.UpdateAsync("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void UpdateFailure()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.PatchAsync("todos", _expected))
                .ReturnsAsync(_failureResponse);

            Assert.Throws<FirebaseException>(async () => await _firebaseClient.UpdateAsync("todos", _expected));
        }
    }
}