using FireSharp.Config;

namespace FireSharp.Test.Console
{
    public class Program
    {
        protected const string BASE_PATH = "https://firesharp.firebaseio.com/";
        protected const string FIREBASE_SECRET = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        private static FirebaseClient _client;

        private static void Main()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FIREBASE_SECRET,
                BasePath = BASE_PATH
            };

            _client = new FirebaseClient(config); //Uses JsonNet default

            EventStreaming();
            Crud();

            System.Console.Read();
        }

        private static async void Crud()
        {
            var setResponse = await _client.SetAsync("todos", new {name = "SET CALL"});
            System.Console.WriteLine(setResponse.Body);
        }

        private static async void EventStreaming()
        {
            await _client.ListenAsync("chat", (sender, args) => { System.Console.WriteLine(args.Data); });
        }
    }
}