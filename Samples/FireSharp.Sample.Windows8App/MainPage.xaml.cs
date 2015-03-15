using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FireSharp.Config;
using FireSharp.Response;

namespace FireSharp.Sample.Windows8App
{
    public class ChatMessage
    {
        public string name { get; set; }
        public string text { get; set; }
    }
    public sealed partial class MainPage : Page
    {
        private const string BasePath = "https://firesharp.firebaseio.com/";
        private const string FirebaseSecret = "fubr9j2Kany9KU3SHCIHBLm142anWCzvlBs1D977";
        private static FirebaseClient _client;

        public MainPage()
        {
            InitializeComponent();
        }

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            PushResponse response = await _client.PushAsync("chat/", new ChatMessage
            {
                name = "Win8",
                text = TextBoxMessage.Text + DateTime.Now.ToString("f")
            });
        }

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };

            _client = new FirebaseClient(config);

            await _client.OnAsync("chat",
                added: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox1.Text = args.Data + " -> 1(ADDED)";
                    });
                },
                changed: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox2.Text = string.Format("Data :{0}, Path: {1} -> 1(CHANGED)", args.Data, args.Path);
                    });
                },
                removed: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox3.Text = args.Path + " -> 1(REMOVED)";
                    });
                });

            await _client.OnAsync("chat",
                added: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox4.Text = args.Data + " -> 2(ADDED)";
                    });
                },
                changed: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox5.Text = string.Format("Data :{0}, Path: {1} -> 2(CHANGED)", args.Data, args.Path);
                    });
                },
                removed: (s, args) =>
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBox6.Text = args.Path + " -> 2(REMOVED)";
                    });
                });
        }
    }
}
