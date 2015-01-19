namespace FireSharp.Config
{
    public class FirebaseConfig : IFirebaseConfig
    {
        public FirebaseConfig()
        {
            TimeoutInMinute = 1;
        }

        public string BasePath { get; set; }
        public string AuthSecret { get; set; }
        public int TimeoutInMinute { get; set; }
    }
}