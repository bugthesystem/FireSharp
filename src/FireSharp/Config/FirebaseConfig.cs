using System.Configuration;
using FireSharp.Config.ConfigSection;
using FireSharp.Exceptions;

namespace FireSharp.Config
{
    using RestSharp.Serializers;

    public class FirebaseConfig : IFirebaseConfig
    {
        public FirebaseConfig()
        {
            TryGetConfigurationFromAppConfig();
        }

        private void TryGetConfigurationFromAppConfig()
        {
            var connectionSection = ConfigurationManager.GetSection(FirebaseConfigurationSection.SectionName) as FirebaseConfigurationSection;
            if (connectionSection == null || connectionSection.FirebaseConnectionElement == null) return;

            this.AuthSecret = connectionSection.FirebaseConnectionElement.AuthSecret;

            this.SetBasePath(connectionSection);
            
        }

        private void SetBasePath(FirebaseConfigurationSection connectionSection)
        {
            var path = connectionSection.FirebaseConnectionElement.BasePath;
            if (path.Equals("**your firebase path**"))
                throw  new FirebaseException("Change your basepath in web.config");

            this.BasePath = connectionSection.FirebaseConnectionElement.BasePath;
        }

        private ISerializer _serializer;

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new JsonSerializer()); }
            set { _serializer = value ?? new JsonSerializer(); }
        }
    }
}
