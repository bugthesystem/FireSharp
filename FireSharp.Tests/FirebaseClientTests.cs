using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Tests.Models;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FireSharp.Tests
{
    public class FirebaseClientTests : FiresharpTestBase
    {
        private Todo _expected;
        private HttpResponseMessage _expectedResponse;
        private IFirebaseClient _firebaseClient;
        private Mock<IFirebaseRequestManager> _firebaseRequestManagerMock;

        protected override void FinalizeSetUp()
        {
            _expected = new Todo
            {
                name = "Do your homework!",
                priority = 1
            };

            _firebaseRequestManagerMock = MockFor<IFirebaseRequestManager>();

           /* _expectedResponse = FixtureRepository.Build<HttpResponseMessage>()
                .With(response => response.Content, new StringContent(_expected.ToJson()))
                .With(response => response.StatusCode, HttpStatusCode.OK)
                //Ignore request field because it has no public constructor, is an abstract or non-public type
                .Create();*/
          _expectedResponse =new HttpResponseMessage{
            Content = new StringContent(_expected.ToJson()),
            StatusCode = HttpStatusCode.OK
          };
            _firebaseClient = new FirebaseClient(_firebaseRequestManagerMock.Object);
        }

        [Test]
        public void Push()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Post("todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = _firebaseClient.Push("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Set()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Put("todos", _expected))
                .Returns(Task.FromResult(_expectedResponse));

            var response = _firebaseClient.Set("todos", _expected);
            Assert.NotNull(response);
            Assert.AreEqual(response.Body, _expected.ToJson());
        }

        [Test]
        public void Get()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Get("todos"))
                .Returns(Task.FromResult(_expectedResponse));

            var firebaseResponse = _firebaseClient.Get("todos");
            Assert.NotNull(firebaseResponse);
            Assert.AreEqual(firebaseResponse.Body, _expected.ToJson());
        }

        [Test]
        public void Delete()
        {
            _firebaseRequestManagerMock.Setup(firebaseRequestManager => firebaseRequestManager.Delete("todos"))
                .Returns(Task.FromResult(_expectedResponse));

            var response = _firebaseClient.Delete("todos");
            Assert.NotNull(response);
            Assert.AreEqual(response.Success, true);
        }

        [Test]
        public void Added_Event_Stream()
        {
        }
    }
}