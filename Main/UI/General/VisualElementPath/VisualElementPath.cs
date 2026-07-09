using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [Serializable]
    public class VisualElementPath : IEmptyCheckable, IVisualElementSelector
    {
        public List<string> names = new();

        public bool IsEmpty()
        {
            return names.Count == 0;
        }

        public VisualElement Query(VisualElement root)
        {
            if (root == null)
            {
                return null;
            }

            if (names.IsNullOrEmpty())
            {
                return null;
            }

            var current = root;

            foreach (var name in names)
            {
                current = current.Q(name);

                if (current == null)
                {
                    break;
                }
            }

            return current;
        }

        public T Query<T>(VisualElement root)
            where T : VisualElement
        {
            if (root == null)
            {
                return null;
            }

            if (names.IsNullOrEmpty())
            {
                return null;
            }

            var current = root;
            T result = null;

            for (var i = 0; i < names.Count; i++)
            {
                var name = names[i];

                if (i == names.Count - 1)
                {
                    result = current.Q<T>(name);
                    break;
                }

                current = current.Q(name);

                if (current == null)
                {
                    break;
                }
            }

            return result;
        }
    }
}