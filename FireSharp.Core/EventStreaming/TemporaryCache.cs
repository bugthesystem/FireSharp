using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FireSharp.Core.EventStreaming
{
    internal sealed class TemporaryCache : IDisposable
    {
        private readonly LinkedList<SimpleCacheItem> pathFromRootList = new();
        private readonly char[] separator = {'/'};
        private readonly object treeLock = new();

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

        internal SimpleCacheItem Root { get; } = new();

        public void Replace(string path, JsonReader data)
        {
            lock (treeLock)
            {
                SimpleCacheItem root = FindRoot(path);
                Replace(root, data);
            }
        }

        public void Update(string path, JsonReader data)
        {
            lock (treeLock)
            {
                SimpleCacheItem root = FindRoot(path);
                UpdateChildren(root, data);
            }
        }

        private SimpleCacheItem FindRoot(string path)
        {
            string[] segments = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return segments.Aggregate(Root, GetNamedChild);
        }

        private static SimpleCacheItem GetNamedChild(SimpleCacheItem root, string segment)
        {
            SimpleCacheItem newRoot = root.Children.FirstOrDefault(c => c.Name == segment);

            if (newRoot != null) return newRoot;
            newRoot = new SimpleCacheItem {Name = segment, Parent = root, Created = true};
            root.Children.Add(newRoot);

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
                        if (reader.Value != null) UpdateChildren(GetNamedChild(root, reader.Value.ToString()), reader);
                        break;
                    case JsonToken.Boolean:
                    case JsonToken.Bytes:
                    case JsonToken.Date:
                    case JsonToken.Float:
                    case JsonToken.Integer:
                    case JsonToken.String:
                        if (root.Created)
                        {
                            if (reader.Value != null)
                            {
                                root.Value = reader.Value.ToString();
                                OnAdded(new ValueAddedEventArgs(PathFromRoot(root), reader.Value.ToString()));
                            }

                            root.Created = false;
                        }
                        else
                        {
                            string oldData = root.Value;
                            if (reader.Value != null) root.Value = reader.Value.ToString();
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
                foreach (SimpleCacheItem child in root.Children.ToArray())
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
            int size = 1;

            while (root.Name != null)
            {
                size += root.Name.Length + 1;
                pathFromRootList.AddFirst(root);
                root = root.Parent;
            }

            if (pathFromRootList.Count == 0)
            {
                return "/";
            }

            StringBuilder sb = new(size);
            foreach (SimpleCacheItem d in pathFromRootList)
            {
                sb.Append($"/{d.Name}");
            }

            pathFromRootList.Clear();

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
