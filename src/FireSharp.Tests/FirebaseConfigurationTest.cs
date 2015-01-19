using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using NUnit.Framework;

namespace FireSharp.Tests
{
    [TestFixture]
    class FirebaseConfigurationTest : FirebaseClientIntegrationTestBase
    {
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";

        [Test, Category("CONFIGURATION")]
        public void ShouldGetAuthKeyFromAppConfig()
        {
            var config = new FirebaseConfig();
            Assert.AreEqual(config.AuthSecret, FIREBASE_SECRET);
        }


        [Test, Category("CONFIGURATION")]
        public void ShouldGetBasePathFromAppConfig()
        {
            var config = new FirebaseConfig();
            Assert.AreEqual(config.BasePath, BASE_PATH);
        }


    }
}
