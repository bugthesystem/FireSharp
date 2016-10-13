using System;

namespace FireSharp.NETCore.EventStreaming
{
    public class ValueAddedEventArgs : EventArgs
    {
        public ValueAddedEventArgs(string path, string data)
        {
            Path = path;
            Data = data;
        }

        public string Path { get; }
        public string Data { get; }
    }
}