using System.Configuration;

namespace FireSharp.Config.ConfigSection
{
    public class FirebaseConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string SectionName = "firebase";

        private const string ConnectionPropertyName = "connectionFactory";

        [ConfigurationProperty(ConnectionPropertyName)]
        public FirebaseConnectionElement FirebaseConnectionElement { get { return (FirebaseConnectionElement)base[ConnectionPropertyName]; } }

        public FirebaseConnectionElement FirebaseTokenElement { get { return (FirebaseConnectionElement)base[ConnectionPropertyName]; } }
    }
}
