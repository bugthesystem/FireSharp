using System.Collections.Generic;

namespace FireSharp.Core.EventStreaming
{
    internal class SimpleCacheItem
    {
        private List<SimpleCacheItem> children;
        public string Name { get; set; }
        public string Value { get; set; }
        public SimpleCacheItem Parent { get; set; }
        public bool Created { get; set; }

        public List<SimpleCacheItem> Children => children ??= new List<SimpleCacheItem>();
    }
}