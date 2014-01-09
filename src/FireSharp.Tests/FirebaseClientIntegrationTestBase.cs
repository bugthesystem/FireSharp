using FireSharp.Config;
using FireSharp.Interfaces;
using NUnit.Framework;
using RestSharp.Serializers;

namespace FireSharp.Tests
{
    [TestFixture]
    public class FirebaseClientIntegrationTestBase
    {
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "**your firebase secret here**";

        public IFirebaseClient GetClientWithSerializer(ISerializer serializer, IFirebaseConfig config)
        {
            config.Serializer = serializer;
            return new FirebaseClient(config);
        }
    }
}