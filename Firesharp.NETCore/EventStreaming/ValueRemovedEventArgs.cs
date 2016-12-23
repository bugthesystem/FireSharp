using System;

namespace FireSharp.NETCore.EventStreaming
{
    public class ValueRemovedEventArgs : EventArgs
    {
        public ValueRemovedEventArgs(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}