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

        [SetUp]
        public void Setup() {

            _expected = new Todo {
                name = "Call mom!",
                priority = 1
            };
            _repository = new MockRepository(MockBehavior.Strict);
            _firebaseClientMock = _repository.Create<IFirebaseClient>(MockBehavior.Strict);
            _expectedResponse = new FirebaseResponse(new RestResponse { Content = _expected.ToJson(), StatusCode = HttpStatusCode.OK });
        }

        [Test]
        public void Push_Test() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Push("todos", _expected)).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClientMock.Object.Push("todos", _expected);
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Set_Test() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Set("todos", _expected)).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClientMock.Object.Set("todos", _expected);
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Get_Test() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Get("todos")).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClientMock.Object.Get("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Delete_Test() {
            _firebaseClientMock.Setup(firebaseClient => firebaseClient.Delete("todos")).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClientMock.Object.Get("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Success, true);
        }

        [TearDown]
        public void TearDown() {
            _repository.VerifyAll();
        }
    }
}