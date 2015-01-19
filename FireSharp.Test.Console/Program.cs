using FireSharp.Config;

namespace FireSharp.Test.Console
{
    public class Program
    {
        private static FirebaseClient _client;
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        static void Main()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FIREBASE_SECRET,
                BasePath = BASE_PATH
            };

            _client = new FirebaseClient(config); //Uses RestSharp JsonSerializer as default

            _client.Listen("chat", added: (sender, args) =>
            {
                System.Console.WriteLine(args.Data);
            });

            System.Console.Read();
        }
    }
}
