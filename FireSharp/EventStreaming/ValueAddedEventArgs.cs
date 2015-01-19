using System;

namespace FireSharp.EventStreaming
{
    public class ValueAddedEventArgs : EventArgs
    {
        public ValueAddedEventArgs(string path, string data)
        {
            Path = path;
            Data = data;
        }

        public string Path { get; private set; }
        public string Data { get; private set; }
    }
}