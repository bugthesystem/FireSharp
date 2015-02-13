using System.Configuration;

namespace FireSharp.Config.ConfigSection
{
    public class FirebaseConnectionElement : ConfigurationElement
    {
        private const string BasePathPropertyName = "basePath";
        private const string AuthSecretPropertyName = "authSecret";

        [ConfigurationProperty(BasePathPropertyName, IsRequired = true)]
        public string BasePath
        {
            get { return (string)this[BasePathPropertyName]; }
            set { this[BasePathPropertyName] = value; }
        }

        [ConfigurationProperty(AuthSecretPropertyName, IsRequired = false)]
        public string AuthSecret
        {
            get { return (string)this[AuthSecretPropertyName]; }
            set { this[AuthSecretPropertyName] = value; }
        }

    }
}