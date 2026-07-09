#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    internal sealed class VisualElementPathDrawer : OdinValueDrawer<VisualElementPath>
    {
        private const string STOP_HERE_OPTION = "<Stop Here>";

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var isListElement = Property.Parent?.ChildResolver is IOrderedCollectionResolver;
            var settings = Property.GetAttribute<VisualElementPathSettingsAttribute>();
            var path = ValueEntry.SmartValue ?? new VisualElementPath();
            path.names ??= new List<string>();
            ValueEntry.SmartValue = path;

            if (TryGetRoot(settings, out var root, out var error) == false)
            {
                SirenixEditorGUI.ErrorMessageBox(error);
                CallNextDrawer(label);
                return;
            }

            var trimmed = EnsurePathValid(root, path.names);

            void DrawInner()
            {
                if (isListElement == false)
                {
                    DrawHeader(Property, label, path.names);
                }
                else
                {
                    DrawListSubtitleIfNeeded(path.names);
                }

                var cleared = false;
                EditorGUI.BeginChangeCheck();

                DrawPathSelectors(root, path.names, settings);

                if (path.names.Count > 0)
                {
                    if (GUILayout.Button("Clear Path"))
                    {
                        path.names.Clear();
                        cleared = true;
                    }
                }

                var changed = EditorGUI.EndChangeCheck();

                if (changed || trimmed || cleared)
                {
                    ValueEntry.SmartValue = path;

                    if (ValueEntry.WeakValues is { Count: > 1 } weakValues)
                    {
                        for (var i = 0; i < weakValues.Count; i++)
                        {
                            weakValues[i] = new VisualElementPath()
                            {
                                names = new(path.names)
                            };
                        }
                    }

                    ValueEntry.WeakValues?.ForceMarkDirty();
                    ValueEntry.Property.MarkSerializationRootDirty();
                    ValueEntry.ApplyChanges();
                }
            }

            if (isListElement)
            {
                DrawInner();
            }
            else
            {
                SirenixEditorGUI.BeginBox();
                DrawInner();
                SirenixEditorGUI.EndBox();
            }
        }

        private static void DrawHeader(InspectorProperty property, GUIContent label, List<string> path)
        {
            var title = string.IsNullOrWhiteSpace(label?.text) ? property?.NiceName ?? "Element Path" : label.text;

            string subtitle;
            if (path.Count == 0)
            {
                subtitle = "Not selected";
            }
            else if (path.Count == 1)
            {
                subtitle = string.Empty;
            }
            else
            {
                subtitle = string.Join(" / ", path);
            }

            SirenixEditorGUI.Title(title, subtitle, TextAlignment.Left, true);
        }

        private static void DrawListSubtitleIfNeeded(List<string> path)
        {
            if (path.Count <= 1)
            {
                return;
            }

            var subtitle = string.Join(" / ", path);
            GUILayout.Label(subtitle, SirenixGUIStyles.LeftAlignedGreyMiniLabel);
        }

        private readonly struct PathOption
        {
            public readonly string label;
            public readonly string name;

            public PathOption(string label, string name)
            {
                this.label = label;
                this.name = name;
            }
        }

        private void DrawPathSelectors(VisualElement root, List<string> path,
            VisualElementPathSettingsAttribute settings)
        {
            var workingPath = new List<string>(path);
            var depth = 0;
            var changed = false;

            while (true)
            {
                var current = depth == 0 ? root : FindElementByPath(root, workingPath.Take(depth));

                if (current == null)
                {
                    workingPath.RemoveRange(depth, workingPath.Count - depth);
                    break;
                }

                var options = depth == 0
                    ? GetAllNamedOptions(root, settings)
                    : GetNamedDescendantOptions(current, settings);

                if (options.Count == 0 && depth >= workingPath.Count)
                {
                    break;
                }

                var optionLabels = new List<string>
                {
                    STOP_HERE_OPTION
                };
                optionLabels.AddRange(options.Select(o => o.label));

                var currentValue = depth < workingPath.Count ? workingPath[depth] : STOP_HERE_OPTION;
                var currentIndex = 0;
                var found = options.FindIndex(o => o.name == currentValue);
                if (found >= 0)
                {
                    currentIndex = found + 1;
                }

                var selectedIndex = DrawUnityDropdown($"Level {depth + 1}", optionLabels.ToArray(), currentIndex);
                if (selectedIndex <= 0)
                {
                    if (depth < workingPath.Count)
                    {
                        workingPath.RemoveRange(depth, workingPath.Count - depth);
                        changed = true;
                    }

                    break;
                }

                var selectedName = options[selectedIndex - 1].name;

                if (depth < workingPath.Count)
                {
                    if (workingPath[depth] != selectedName)
                    {
                        workingPath[depth] = selectedName;
                        changed = true;
                    }
                }
                else
                {
                    workingPath.Add(selectedName);
                    changed = true;
                }

                depth++;
            }

            if (changed)
            {
                path.Clear();
                path.AddRange(workingPath);
            }
        }

        private static bool EnsurePathValid(VisualElement root, List<string> path)
        {
            if (path.Count == 0)
            {
                return false;
            }

            var current = FindFirstByName(root, path[0]);
            if (current == null)
            {
                path.Clear();
                return true;
            }

            for (var i = 1; i < path.Count; i++)
            {
                current = FindNamedDescendant(current, path[i]);
                if (current == null)
                {
                    path.RemoveRange(i, path.Count - i);
                    return true;
                }
            }

            return false;
        }

        private bool TryGetRoot(VisualElementPathSettingsAttribute settings, out VisualElement root, out string error)
        {
            root = null;

            var isFromLocalProvider = settings?.IsFromLocalProvider == true;
            var mustFromParent = settings?.MustFromParent == true;

            foreach (var parent in Property.TraverseToRoot(false, p => p.Parent))
            {
                if (parent.ParentValues.IsNullOrEmpty())
                {
                    continue;
                }

                var target = parent.ParentValues[0];

                if (target is IVisualElementGenerator generator)
                {
                    if (isFromLocalProvider)
                    {
                        if (target is not IGameItem)
                        {
                            if (mustFromParent == false || target is not Component)
                            {
                                root = generator.GenerateVisualElement();
                                error = null;
                                return root != null;
                            }
                        }
                    }
                    else if (target is IGameItem)
                    {
                        root = generator.GenerateVisualElement();
                        error = null;
                        return root != null;
                    }
                }

                if (settings != null && string.IsNullOrWhiteSpace(settings.VisualTreeFieldName) == false)
                {
                    if (TryGetVisualTreeFromNamedMember(target, settings.VisualTreeFieldName, out var visualTreeAsset))
                    {
                        root = visualTreeAsset.CloneTree();
                        error = null;
                        return true;
                    }
                }

                if (target is Component component)
                {
                    if (isFromLocalProvider)
                    {
                        var generatorsInParent = component.GetComponentsInParent<IVisualElementGenerator>(true);
                        foreach (var generatorInParent in generatorsInParent)
                        {
                            if (mustFromParent)
                            {
                                if (generatorInParent is Component componentInParent)
                                {
                                    if (componentInParent.transform == component.transform)
                                    {
                                        continue;
                                    }
                                }
                            }
                            
                            if (generatorInParent is not IGameItem)
                            {
                                root = generatorInParent.GenerateVisualElement();
                                error = null;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        var gameItemInParent = component.GetComponentInParent<IGameItem>(true);
                        if (gameItemInParent != null)
                        {
                            if (gameItemInParent is IVisualElementGenerator generatorInParent)
                            {
                                root = generatorInParent.GenerateVisualElement();
                                error = null;
                                return true;
                            }
                        }
                        else
                        {
                            var document = component.GetComponentInParent<UIDocument>(true);
                            if (document != null && document.visualTreeAsset != null)
                            {
                                root = document.visualTreeAsset.CloneTree();
                                error = null;
                                return true;
                            }
                        }
                    }
                }
            }

            error = $"No Root:{nameof(VisualElement)} found." +
                    $"Specify a {nameof(VisualTreeAsset)} member name in {nameof(VisualElementPathSettingsAttribute)} " +
                    $"or ensure a parent implements {nameof(IVisualElementGenerator)}/{nameof(UIDocument)}.";
            return false;
        }

        private static bool TryGetVisualTreeFromNamedMember(object target, string memberName, out VisualTreeAsset asset)
        {
            asset = null;
            if (target == null || string.IsNullOrWhiteSpace(memberName))
            {
                return false;
            }

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = target.GetType();

            var field = type.GetField(memberName, flags);
            if (field != null && typeof(VisualTreeAsset).IsAssignableFrom(field.FieldType))
            {
                asset = field.GetValue(target) as VisualTreeAsset;
                if (asset != null)
                {
                    return true;
                }
            }

            var property = type.GetProperty(memberName, flags);
            if (property != null && typeof(VisualTreeAsset).IsAssignableFrom(property.PropertyType) && property.CanRead)
            {
                asset = property.GetValue(target, null) as VisualTreeAsset;
                if (asset != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static VisualElement FindElementByPath(VisualElement root, IEnumerable<string> path)
        {
            if (path.IsNullOrEmpty())
            {
                return root;
            }

            using var enumerator = path.GetEnumerator();
            if (enumerator.MoveNext() == false)
            {
                return root;
            }

            var current = FindFirstByName(root, enumerator.Current);

            while (enumerator.MoveNext() && current != null)
            {
                current = FindNamedDescendant(current, enumerator.Current);
            }

            return current;
        }

        private static VisualElement FindFirstByName(VisualElement root, string name)
        {
            return root.QueryAll<VisualElement>().FirstOrDefault(e => string.Equals(e.name, name));
        }

        private static VisualElement FindNamedDescendant(VisualElement parent, string name)
        {
            foreach (var element in parent.QueryAll<VisualElement>())
            {
                if (element == parent)
                {
                    continue;
                }

                if (string.Equals(element.name, name))
                {
                    return element;
                }
            }

            return null;
        }

        private static List<PathOption> GetAllNamedOptions(VisualElement root,
            VisualElementPathSettingsAttribute settings)
        {
            var dict = new Dictionary<string, PathOption>();

            foreach (var element in root.QueryAll<VisualElement>())
            {
                if (string.IsNullOrWhiteSpace(element.name))
                {
                    continue;
                }

                var allowed = IsTypeAllowed(element, settings);
                var label = allowed ? element.name : $"Other/{element.name}";
                if (dict.ContainsKey(element.name) == false)
                {
                    dict[element.name] = new PathOption(label, element.name);
                }
            }

            return dict.Values.ToList();
        }

        private static List<PathOption> GetNamedDescendantOptions(VisualElement parent,
            VisualElementPathSettingsAttribute settings)
        {
            var dict = new Dictionary<string, PathOption>();

            foreach (var element in parent.QueryAll<VisualElement>())
            {
                if (element == parent || string.IsNullOrWhiteSpace(element.name))
                {
                    continue;
                }

                var allowed = IsTypeAllowed(element, settings);
                var label = allowed ? element.name : $"Other/{element.name}";
                if (dict.ContainsKey(element.name) == false)
                {
                    dict[element.name] = new PathOption(label, element.name);
                }
            }

            return dict.Values.ToList();
        }

        private static int DrawUnityDropdown(string label, string[] options, int currentIndex)
        {
            if (options.Length == 0)
            {
                return currentIndex;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, options.Length - 1);
            return EditorGUILayout.Popup(label, currentIndex, options);
        }

        private static bool IsTypeAllowed(VisualElement element, VisualElementPathSettingsAttribute settings)
        {
            if (settings == null || settings.AllowedTypes.IsNullOrEmpty())
            {
                return true;
            }

            var type = element.GetType();
            return settings.AllowedTypes.Any(t => type.IsDerivedFrom(t, true));
        }
    }
}
#endif