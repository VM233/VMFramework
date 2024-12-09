using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public sealed class StringPathTree<TData> : IChildrenProvider<StringPathTreeNode<TData>>, IEnumerable<StringPathTreeNode<TData>>
    {
        public StringPathTreeNode<TData> Root { get; } = new(string.Empty);
        
        public string Separator { get; init; } = "/";

        public void Add(string path, TData data)
        {
            var parts = path.Split(Separator);
            
            var current = Root;

            foreach (var part in parts)
            {
                if (part.IsNullOrEmpty())
                {
                    continue;
                }

                if (current.Children.TryGetValue(part, out var child))
                {
                    current = child;
                    continue;
                }
                
                child = new StringPathTreeNode<TData>(part, current);
                current = child;
            }
            
            current.data = data;
        }

        public void Remove(string path)
        {
            var parts = path.Split(Separator);

            var current = Root;

            foreach (var part in parts)
            {
                if (part.IsNullOrEmpty())
                {
                    continue;
                }

                if (current.Children.TryGetValue(part, out var child) == false)
                {
                    return;
                }
                
                current = child;
            }
            
            current.Parent.RemoveChild(current.PathPart);
        }

        public IEnumerable<StringPathTreeNode<TData>> GetChildren()
        {
            return Root.GetChildren();
        }

        public IEnumerator<StringPathTreeNode<TData>> GetEnumerator()
        {
            return Root.GetAllLeaves(true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
