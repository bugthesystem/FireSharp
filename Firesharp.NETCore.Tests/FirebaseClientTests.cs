using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FireSharp.NETCore.Exceptions;
using FireSharp.NETCore.Extensions;
using FireSharp.NETCore.Interfaces;
using FireSharp.NETCore.Tests.Models;
using Moq;
using System;

namespace FireSharp.NETCore.Tests
{
    //public class FirebaseClientTestsFixture 
    //{
    //    public Todo Expected;
    //    public HttpResponseMessage ExpectedResponse;
    //    public HttpResponseMessage FailureResponse;
    //    public FirebaseClient FirebaseClient;

    //    public FirebaseClientTestsFixture()
    //    {
    //        _expected = new Todo
    //        {
    //            name = "Do your homework!",
    //            priority = 1
    //        };

    //        _firebaseRequestManagerMock = new Mock<IRequestManager>();

    //        ExpectedResponse = new HttpResponseMessage
    //        {
    //            Content = new StringContent(Expected.ToJson()),
    //            StatusCode = HttpStatusCode.OK
    //        };

    //        FailureResponse = new HttpResponseMessage
    //        {
    //            Content = new StringContent("error"),
    //            StatusCode = HttpStatusCode.InternalServerError
    //        };

    //        FirebaseClient = new FirebaseClient(FirebaseRequestManagerMock.Object);
    //    }

    //    public void Dispose()
    //    {
    //        // do any cleanup
    //    }
    //}

    public class FirebaseClientTests //: IClassFixture<FirebaseClientTestsFixture>
    {
        //private FirebaseClientTestsFixture _fixture;
        private Todo _expected;
        private HttpResponseMessage _expectedResponse;
        private HttpResponseMessage _failureResponse;
        private FirebaseClient _firebaseClient;
        private Mock<IRequestManager> _firebaseRequestManagerMock;

        public FirebaseClientTests()
        {
            _expected = new Todo
            {
                name = "Do your homework!",
                priority = 1
            };

            _firebaseRequestManagerMock = new Mock<IRequestManager>();

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

        [Fact]
        public async Task DeleteAsync()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Delete, "todos", (object) null))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.DeleteAsync("todos").ConfigureAwait(false);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task DeleteAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Delete, "todos", (object) null)),
                _failureResponse);

            await Assert.ThrowsAsync<FirebaseException>(async () => await _firebaseClient.DeleteAsync("todos").ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetAsync()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager =>
                firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null))
                .Returns(Task.FromResult(_expectedResponse));

            var firebaseResponse = await _firebaseClient.GetAsync("todos").ConfigureAwait(false);
            Assert.NotNull(firebaseResponse);
            Assert.Equal(firebaseResponse.Body, _expected.ToJson());
        }

        [Fact]
        public async Task GetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null)),
                _failureResponse);

            await Assert.ThrowsAsync<FirebaseException>(async () => await _firebaseClient.GetAsync("todos").ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public void GetFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Get, "todos", (object) null)),
                _failureResponse);

            Assert.Throws<FirebaseException>(() => _firebaseClient.Get("todos"));
        }

        [Fact]
        public async Task PushAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.PushAsync("todos", _expected).ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.Equal(response.Body, _expected.ToJson());
        }

        [Fact]
        public async Task PushAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Post, "todos", _expected)),
                _failureResponse);

            await Assert.ThrowsAsync<FirebaseException>(async () => await _firebaseClient.PushAsync("todos", _expected).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task SetAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.SetAsync("todos", _expected).ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.Equal(response.Body, _expected.ToJson());
        }

        [Fact]
        public async Task SetAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(HttpMethod.Put, "todos", _expected)),
                _failureResponse);

            await Assert.ThrowsAsync<FirebaseException>(async () => await _firebaseClient.SetAsync("todos", _expected).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            _firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = await _firebaseClient.UpdateAsync("todos", _expected).ConfigureAwait(false);
            Assert.NotNull(response);
            Assert.Equal(response.Body, _expected.ToJson());
        }

        [Fact]
        public async Task UpdateAsyncFailure()
        {
            ReturnsExtensions.ReturnsAsync(_firebaseRequestManagerMock.Setup(
                firebaseRequestManager => firebaseRequestManager.RequestAsync(RequestManager.Patch, "todos", _expected)),
                _failureResponse);

            await Assert.ThrowsAsync<FirebaseException>(async () => await _firebaseClient.UpdateAsync("todos", _expected).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}