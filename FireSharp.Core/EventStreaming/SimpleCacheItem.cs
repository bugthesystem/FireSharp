using System.Collections.Generic;

namespace FireSharp.Core.EventStreaming
{
    internal class SimpleCacheItem
    {
        private List<SimpleCacheItem> _children;
        public string Name { get; set; }
        public string Value { get; set; }
        public SimpleCacheItem Parent { get; set; }
        public bool Created { get; set; }

        public List<SimpleCacheItem> Children => _children ?? (_children = new List<SimpleCacheItem>());
    }
}