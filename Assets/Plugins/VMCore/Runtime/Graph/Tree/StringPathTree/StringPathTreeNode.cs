using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public sealed class StringPathTreeNode<TData> : ITreeNode<StringPathTreeNode<TData>>
    {
        public StringPathTreeNode<TData> Parent { get; }

        private readonly Dictionary<string, StringPathTreeNode<TData>> _children = new();

        public string PathPart { get; }

        public string FolderPath
        {
            get
            {
                string path = null;

                foreach (var node in this.TraverseToRoot(false))
                {
                    if (path == null)
                    {
                        path = node.PathPart;
                    }
                    else
                    {
                        path = node.PathPart + "/" + path;
                    }
                }
                
                return path;
            }
        }

        public string FullPath
        {
            get
            {
                string path = PathPart;

                foreach (var node in this.TraverseToRoot(false))
                {
                    path = node.PathPart + "/" + path;
                }
                
                return path;
            }
        }
        
        public int Depth { get; }

        public TData data;
        
        public IReadOnlyDictionary<string, StringPathTreeNode<TData>> Children => _children;

        public StringPathTreeNode(string pathPart, StringPathTreeNode<TData> parent = null)
        {
            this.PathPart = pathPart;

            this.Parent = parent;
            parent?._children.Add(pathPart, this);
            
            this.Depth = parent?.Depth + 1 ?? 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveChild(string pathPart)
        {
            if (_children.Remove(pathPart) == false)
            {
                Debug.LogWarning($"{pathPart} not found in children of {this.PathPart}");
            }
        }

        #region Tree Node

        StringPathTreeNode<TData> IParentProvider<StringPathTreeNode<TData>>.GetParent() => Parent;

        public IEnumerable<StringPathTreeNode<TData>> GetChildren() => _children.Values;

        #endregion
    }
}