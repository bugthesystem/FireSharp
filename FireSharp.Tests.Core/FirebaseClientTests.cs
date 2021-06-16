using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FireSharp.Core;
using FireSharp.Core.Exceptions;
using FireSharp.Core.Extensions;
using FireSharp.Core.Interfaces;
using FireSharp.Core.Response;
using FireSharp.Tests.Core.Commom;
using FireSharp.Tests.Core.Models;
using Moq;
using NUnit.Framework;

namespace FireSharp.Tests.Core
{

    [TestFixture]
    public class FirebaseClientTests : TestBase
    {
        private Todo expected;
        private HttpResponseMessage expectedResponse;
        private HttpResponseMessage failureResponse;
        private FirebaseClient firebaseClient;
        private Mock<IRequestManager> firebaseRequestManagerMock;

        protected override void FinalizeSetUp()
        {
            expected = new Todo
            {
                name = "Do your homework!",
                priority = 1
            };

            firebaseRequestManagerMock = MockFor<IRequestManager>();

            expectedResponse = new HttpResponseMessage
            {
                Content = new StringContent(expected.ToJson()),
                StatusCode = HttpStatusCode.OK
            };
            failureResponse = new HttpResponseMessage
            {
                Content = new StringContent("error"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            firebaseClient = new FirebaseClient(firebaseRequestManagerMock.Object);
        }

        [Test]
        public async Task DeleteAsync()
        {
            firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Delete, "TODOs", (object)null))
                .Returns(Task.FromResult(expectedResponse));

            FirebaseResponse response = await firebaseClient.DeleteAsync("TODOs");
            Assert.NotNull(response);
        }

        [Test]
        public void DeleteAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Delete, "TODOs", (object)null)),
                failureResponse);

            Assert.ThrowsAsync<FirebaseException>(async () => await firebaseClient.DeleteAsync("TODOs"));
        }

        [Test]
        public async Task GetAsync()
        {
            firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Get, "TODOs", (object)null))
                .Returns(Task.FromResult(expectedResponse));

            FirebaseResponse firebaseResponse = await firebaseClient.GetAsync("TODOs");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, expected.ToJson());
        }

        [Test]
        public void GetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "TODOs", (object)null)),
                failureResponse);

            Assert.ThrowsAsync<FirebaseException>(async () => await firebaseClient.GetAsync("TODOs"));
        }

        [Test]
        public void GetFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "TODOs", (object)null)),
                failureResponse);

            Assert.Throws<FirebaseException>(() => firebaseClient.Get("TODOs"));
        }

        [Test]
        public async Task PushAsync()
        {
            firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "TODOs", expected))
                .Returns(Task.FromResult(expectedResponse));

            PushResponse response = await firebaseClient.PushAsync("TODOs", expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, expected.ToJson());
        }

        [Test]
        public void PushAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "TODOs", expected)),
                failureResponse);

            Assert.ThrowsAsync<FirebaseException>(async () => await firebaseClient.PushAsync("TODOs", expected));
        }

        [Test]
        public async Task SetAsync()
        {
            firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "TODOs", expected))
                .Returns(Task.FromResult(expectedResponse));

            SetResponse response = await firebaseClient.SetAsync("TODOs", expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, expected.ToJson());
        }

        [Test]
        public void SetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "TODOs", expected)),
                failureResponse);

            Assert.ThrowsAsync<FirebaseException>(async () => await firebaseClient.SetAsync("TODOs", expected));
        }

        [Test]
        public async Task UpdateAsync()
        {
            firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "TODOs", expected))
                .Returns(Task.FromResult(expectedResponse));

            FirebaseResponse response = await firebaseClient.UpdateAsync("TODOs", expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, expected.ToJson());
        }

        [Test]
        public void UpdateAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "TODOs", expected)),
                failureResponse);

            Assert.ThrowsAsync<FirebaseException>(async () => await firebaseClient.UpdateAsync("TODOs", expected));
        }
    }
}