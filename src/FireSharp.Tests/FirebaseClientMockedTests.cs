using System.Net;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using RestSharp;

namespace FireSharp.Tests {

    [TestFixture]
    public class FirebaseClientMockedTests {

        Todo _expected;
        MockRepository _repository;
        Mock<IFirebaseRequestManager> _firebaseRequestManagerMock;
        IRestResponse _expectedResponse;
        IFirebaseClient _firebaseClient;
        IFixture _fixtureRepository;

        [SetUp]
        public void Setup() {

            _expected = new Todo {
                name = "Do your homework!",
                priority = 1
            };
            _repository = new MockRepository(MockBehavior.Strict);
            _fixtureRepository = new Fixture();
            _firebaseRequestManagerMock = _repository.Create<IFirebaseRequestManager>();

            _expectedResponse = _fixtureRepository.Build<RestResponse>()
                .With(response => response.Content, _expected.ToJson())
                .With(response => response.StatusCode, HttpStatusCode.OK)
                .Without(response => response.Request) /*Ignore request field because it has no public constructor, is an abstract or non-public type*/
                .Create();

            _firebaseClient = new FirebaseClient(_firebaseRequestManagerMock.Object);
        }

        [Test]
        public void Push() {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Post("todos", _expected)).Returns(_expectedResponse);

            PushResponse response = _firebaseClient.Push("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Set() {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Put("todos", _expected)).Returns(_expectedResponse);

            SetResponse response = _firebaseClient.Set("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Get() {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Get("todos")).Returns(_expectedResponse);

            FirebaseResponse firebaseResponse = _firebaseClient.Get("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Delete() {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Delete("todos")).Returns(_expectedResponse);

            DeleteResponse response = _firebaseClient.Delete("todos");
            Assert.NotNull(response);
            Assert.AreEqual(response.Success, true);
        }

        [TearDown]
        public void TearDown() {
            _repository.VerifyAll();
        }
    }
}