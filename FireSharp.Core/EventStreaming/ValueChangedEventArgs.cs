using System;

namespace FireSharp.Core.EventStreaming
{
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(string path, string data, string oldData)
        {
            Path = path;
            Data = data;
            OldData = oldData;
        }

        public string Path { get; private set; }
        public string Data { get; private set; }
        public string OldData { get; private set; }
    }
}