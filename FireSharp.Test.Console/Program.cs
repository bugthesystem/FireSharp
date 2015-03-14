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
            //Crud();

            System.Console.Read();
        }

        private static async void Crud()
        {
            var setResponse = await _client.SetAsync("todos", new { name = "SET CALL" });
            System.Console.WriteLine(setResponse.Body);
        }

        private static void EventStreaming()
        {
            _client.OnAsync("chat",
                added: (sender, args) => { System.Console.WriteLine(args.Data); },
                changed: (sender, args) => { System.Console.WriteLine(args.Data); },
                removed: (sender, args) => { System.Console.WriteLine(args.Path); })
                .Wait();

            _client.OnAsync("chat",
                   added: (sender, args) => { System.Console.WriteLine(args.Data); },
                   changed: (sender, args) => { System.Console.WriteLine(args.Data); },
                   removed: (sender, args) => { System.Console.WriteLine(args.Path); })
                   .Wait();
        }

    }
}