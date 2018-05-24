using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FireSharp.Core.EventStreaming
{
    internal sealed class TemporaryCache : IDisposable
    {
        private readonly LinkedList<SimpleCacheItem> _pathFromRootList = new LinkedList<SimpleCacheItem>();
        private readonly char[] _seperator = {'/'};
        private readonly object _treeLock = new object();

        public object Context = null;

        public TemporaryCache()
        {
            Root.Name = string.Empty;
            Root.Created = false;
            Root.Parent = null;
            Root.Name = null;
        }

        ~TemporaryCache()
        {
            Dispose(false);
        }

        internal SimpleCacheItem Root { get; } = new SimpleCacheItem();

        public void Replace(string path, JsonReader data)
        {
            lock (_treeLock)
            {
                var root = FindRoot(path);
                Replace(root, data);
            }
        }

        public void Update(string path, JsonReader data)
        {
            lock (_treeLock)
            {
                var root = FindRoot(path);
                UpdateChildren(root, data);
            }
        }

        private SimpleCacheItem FindRoot(string path)
        {
            var segments = path.Split(_seperator, StringSplitOptions.RemoveEmptyEntries);

            return segments.Aggregate(Root, GetNamedChild);
        }

        private static SimpleCacheItem GetNamedChild(SimpleCacheItem root, string segment)
        {
            var newRoot = root.Children.FirstOrDefault(c => c.Name == segment);

            if (newRoot == null)
            {
                newRoot = new SimpleCacheItem {Name = segment, Parent = root, Created = true};
                root.Children.Add(newRoot);
            }

            return newRoot;
        }

        private void Replace(SimpleCacheItem root, JsonReader reader)
        {
            UpdateChildren(root, reader, true);
        }

        private void UpdateChildren(SimpleCacheItem root, JsonReader reader, bool replace = false)
        {
            if (replace)
            {
                DeleteChild(root);

                root.Parent?.Children.Add(root);
            }

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        UpdateChildren(GetNamedChild(root, reader.Value.ToString()), reader);
                        break;
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Integer:
                    case JsonToken.String:
                        if (root.Created)
                        {
                            root.Value = reader.Value.ToString();
                            OnAdded(new ValueAddedEventArgs(PathFromRoot(root), reader.Value.ToString()));
                            root.Created = false;
                        }
                        else
                        {
                            var oldData = root.Value;
                            root.Value = reader.Value.ToString();
                            OnUpdated(new ValueChangedEventArgs(PathFromRoot(root), root.Value, oldData));
                        }

                        return;
                    case JsonToken.Null:
                        DeleteChild(root);
                        return;
                    case JsonToken.EndObject: return;
                }
            }
        }

        private void DeleteChild(SimpleCacheItem root)
        {
            if (root.Parent != null)
            {
                if (RemoveChildFromParent(root))
                {
                    OnRemoved(new ValueRemovedEventArgs(PathFromRoot(root)));
                }
            }
            else
            {
                foreach (var child in root.Children.ToArray())
                {
                    RemoveChildFromParent(child);
                    OnRemoved(new ValueRemovedEventArgs(PathFromRoot(child)));
                }
            }
        }

        private bool RemoveChildFromParent(SimpleCacheItem child)
        {
            if (child.Parent != null)
            {
                return child.Parent.Children.Remove(child);
            }

            return false;
        }

        private string PathFromRoot(SimpleCacheItem root)
        {
            var size = 1;

            while (root.Name != null)
            {
                size += root.Name.Length + 1;
                _pathFromRootList.AddFirst(root);
                root = root.Parent;
            }

            if (_pathFromRootList.Count == 0)
            {
                return "/";
            }

            var sb = new StringBuilder(size);
            foreach (var d in _pathFromRootList)
            {
                sb.Append($"/{d.Name}");
            }

            _pathFromRootList.Clear();

            return sb.ToString();
        }

        private void OnAdded(ValueAddedEventArgs args)
        {
            Added?.Invoke(this, args, Context);
        }

        private void OnUpdated(ValueChangedEventArgs args)
        {
            Changed?.Invoke(this, args, Context);
        }

        private void OnRemoved(ValueRemovedEventArgs args)
        {
            Removed?.Invoke(this, args, Context);
        }

        public event ValueAddedEventHandler Added;
        public event ValueChangedEventHandler Changed;
        public event ValueRemovedEventHandler Removed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Added = null;
                Changed = null;
                Removed = null;
            }
        }
    }
}
