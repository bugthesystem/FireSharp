using System.Configuration;

namespace FireSharp.Config.ConfigSection
{
    public class FirebaseTokenElement : ConfigurationElement
    {
        private const string TokenPropertyName = "token";

        [ConfigurationProperty(TokenPropertyName, IsRequired = true)]
        public string Token
        {
            get { return (string)this[TokenPropertyName]; }
            set { this[TokenPropertyName] = value; }
        }

    }
}