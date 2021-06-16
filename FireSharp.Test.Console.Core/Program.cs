using System;
using FireSharp.Core;
using FireSharp.Core.Config;
using FireSharp.Core.Interfaces;
using FireSharp.Core.Response;

namespace FireSharp.Test.Console.Core
{
    internal class Program
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
            Crud();

            System.Console.Read();
        }

        private static async void Crud()
        {
            SetResponse setResponse = await _client.SetAsync("TODOs", new { name = "SET CALL" });
            System.Console.WriteLine(setResponse.Body);
        }

        private static async void EventStreaming()
        {
            //await _client.DeleteAsync("chat");

            await _client.OnAsync("chat",
                async (_, args, _) =>
                {
                    System.Console.WriteLine(args.Data + "-> 1\n");
                    await _client.PushAsync("chat/", new
                    {
                        name = "someone",
                        text = "Console 1:" + DateTime.Now.ToString("f")
                    });
                },
                (_, args, _) => { System.Console.WriteLine(args.Data); },
                (_, args, _) => { System.Console.WriteLine(args.Path); });

            EventStreamResponse response = await _client.OnAsync("chat",
                (_, args, _) => { System.Console.WriteLine(args.Data + " -> 2\n"); },
                (_, args, _) => { System.Console.WriteLine(args.Data); },
                (_, args, _) => { System.Console.WriteLine(args.Path); });

            //Call dispose to stop listening for events
            response.Dispose();
        }
    }
}
