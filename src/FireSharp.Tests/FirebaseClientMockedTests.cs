using System.Net;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace FireSharp.Tests {

    [TestFixture]
    public class FirebaseClientMockedTests {

        Todo _expected;
        MockRepository _repository;
        Mock<IFirebaseClient> _firebaseClientMock;
        FirebaseResponse _expectedResponse;
        PushResponse _expectedPushResponse;
        DeleteResponse _expectedDeleteResponse;
        SetResponse _expectedSetResponse;

        [SetUp]
        public void Setup() {

            _expected = new Todo {
                name = "Do your homework!",
                priority = 1
            };
            _repository = new MockRepository(MockBehavior.Strict);
            _firebaseClientMock = _repository.Create<IFirebaseClient>(MockBehavior.Strict);

            _expectedResponse = new FirebaseResponse(new RestResponse { Content = _expected.ToJson(), StatusCode = HttpStatusCode.OK });
            _expectedPushResponse = new PushResponse(new RestResponse { Content = _expected.ToJson(), StatusCode = HttpStatusCode.OK });
            _expectedSetResponse = new SetResponse(new RestResponse { Content = _expected.ToJson(), StatusCode = HttpStatusCode.OK });
            _expectedDeleteResponse = new DeleteResponse(new RestResponse { StatusCode = HttpStatusCode.OK });
        }

        [Test]
        public void Push() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Push("todos", _expected)).Returns(_expectedPushResponse);

            PushResponse response = _firebaseClientMock.Object.Push("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Set() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Set("todos", _expected)).Returns(_expectedSetResponse);

            SetResponse response = _firebaseClientMock.Object.Set("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Get() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Get("todos")).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClientMock.Object.Get("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Delete() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Delete("todos")).Returns(_expectedDeleteResponse);

            DeleteResponse response = _firebaseClientMock.Object.Delete("todos");
            Assert.NotNull(response);
            Assert.AreEqual(response.Success, true);
        }

        [TearDown]
        public void TearDown() {
            _repository.VerifyAll();
        }
    }
}