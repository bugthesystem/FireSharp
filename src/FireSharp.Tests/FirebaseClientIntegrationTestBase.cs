using FireSharp.Config;
using FireSharp.Interfaces;
using NUnit.Framework;
using RestSharp.Serializers;

namespace FireSharp.Tests
{
    [TestFixture]
    public class FirebaseClientIntegrationTestBase
    {
        

        public IFirebaseClient GetClientWithSerializer(ISerializer serializer, IFirebaseConfig config)
        {
            config.Serializer = serializer;
            return new FirebaseClient(config);
        }
    }
}