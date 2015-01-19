using System.Collections.Generic;

namespace FireSharp.EventStreaming
{
    internal class CacheItem
    {
        private List<CacheItem> _children;
        public string Name { get; set; }
        public string Value { get; set; }
        public CacheItem Parent { get; set; }
        public bool Created { get; set; }

        public List<CacheItem> Children
        {
            get { return _children ?? (_children = new List<CacheItem>()); }
        }
    }
}