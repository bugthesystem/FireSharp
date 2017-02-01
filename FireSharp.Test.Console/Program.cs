using System;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace FireSharp.Test.Console
{
    public class Program
    {
        protected const string BasePath = "https://firesharp.firebaseio.com/";
        protected const string FirebaseSecret = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        private static FirebaseClient _client;

        private static void Main()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };

            _client = new FirebaseClient(config); //Uses JsonNet default

            EventStreaming();
            //Crud();

            System.Console.Read();
        }

        private static async void Crud()
        {
            var setResponse = await _client.SetAsync("todos", new {name = "SET CALL"});
            System.Console.WriteLine(setResponse.Body);
        }

        private static async void EventStreaming()
        {
            await _client.DeleteAsync("chat");

            await _client.OnAsync("chat",
                async (sender, args, context) =>
                {
                    System.Console.WriteLine(args.Data + "-> 1\n");
                    await _client.PushAsync("chat/", new
                    {
                        name = "someone",
                        text = "Console 1:" + DateTime.Now.ToString("f")
                    });
                },
                (sender, args, context) => { System.Console.WriteLine(args.Data); },
                (sender, args, context) => { System.Console.WriteLine(args.Path); });

            var response = await _client.OnAsync("chat",
                (sender, args, context) => { System.Console.WriteLine(args.Data + " -> 2\n"); },
                (sender, args, context) => { System.Console.WriteLine(args.Data); },
                (sender, args, context) => { System.Console.WriteLine(args.Path); });

            //Call dispose to stop listening for events
            //response.Dispose();
        }
    }
}