using System;
using System.Collections.Generic;
using EnumsNET;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [Serializable]
    public class VisualElementSelection
    {
        public VisualElementPath path = new();

        [EnumToggleButtons]
        public VisualElementSelectionMode mode = VisualElementSelectionMode.Self;

        public IEnumerable<VisualElement> Query(VisualElement root)
        {
            var container = path.Query(root);

            if (container == null)
            {
                yield break;
            }

            if (mode.HasAnyFlags(VisualElementSelectionMode.Self))
            {
                yield return container;
            }

            if (mode.HasAllFlags(
                    VisualElementSelectionMode.DirectChildren | VisualElementSelectionMode.IndirectChildren))
            {
                foreach (var child in container.GetAllChildren(includingSelf: false,
                             childrenGetter: element => element.Children()))
                {
                    yield return child;
                }
            }
            else
            {
                if (mode.HasAnyFlags(VisualElementSelectionMode.DirectChildren))
                {
                    foreach (var child in container.Children())
                    {
                        yield return child;
                    }
                }

                if (mode.HasAnyFlags(VisualElementSelectionMode.IndirectChildren))
                {
                    foreach (var child in container.Children())
                    {
                        foreach (var indirectChild in child.GetAllChildren(includingSelf: true,
                                     childrenGetter: element => element.Children()))
                        {
                            yield return indirectChild;
                        }
                    }
                }
            }
        }
    }
}