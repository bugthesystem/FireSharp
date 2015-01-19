using FireSharp.Config;
using FireSharp.Interfaces;
using NUnit.Framework;

namespace FireSharp.Tests
{
    [TestFixture]
    public class FiresharpTestBase : TestBase
    {
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";

        public IFirebaseClient GetClientWithSerializer(IFirebaseConfig config)
        {
            return new FirebaseClient(config);
        }
    }
}