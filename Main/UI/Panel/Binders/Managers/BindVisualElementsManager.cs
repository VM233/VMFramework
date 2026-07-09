using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class BindVisualElementsManager : PanelModifier
    {
        public delegate void BindVisualElementChangedHandler(string bindName, object bindObject,
            VisualElement visualElement, bool added);

        public event BindVisualElementChangedHandler OnBindVisualElementChanged;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, Dictionary<object, VisualElement>> bindElementsLookup = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, Dictionary<VisualElement, object>> bindObjectsLookup = new();

        public virtual bool Add(string bindName, object bindObject, VisualElement visualElement)
        {
            if (bindObject == null)
            {
                UnityEngine.Debug.LogError($"Cannot bind a null object to a visual element.", this);
                return false;
            }

            if (visualElement == null)
            {
                UnityEngine.Debug.LogError($"Cannot bind a visual element to a null object.", this);
                return false;
            }

            var bindElements = bindElementsLookup.GetOrCreate(bindName);

            if (bindElements.ContainsKey(bindObject))
            {
                UnityEngine.Debug.LogWarning($"The bind object {bindObject} is already bound to a visual element. Overwriting.",
                    this);
            }

            bindElements[bindObject] = visualElement;

            var bindObjects = bindObjectsLookup.GetOrCreate(bindName);

            if (bindObjects.ContainsKey(visualElement))
            {
                UnityEngine.Debug.LogWarning(
                    $"The {visualElement.GetType().Name} {visualElement.name} " +
                    $"is already bound to a bind object. Overwriting.", this);
            }

            bindObjects[visualElement] = bindObject;

            OnBindVisualElementChanged?.Invoke(bindName, bindObject, visualElement, added: true);
            
            return true;
        }

        public virtual bool Remove(string bindName, object bindObject)
        {
            return Remove(bindName, bindObject, out _);
        }

        public virtual bool Remove(string bindName, object bindObject, out VisualElement visualElement)
        {
            if (bindObject == null)
            {
                visualElement = null;
                UnityEngine.Debug.LogError($"Cannot unbind a null object from a visual element.", this);
                return false;
            }

            if (bindElementsLookup.TryGetValue(bindName, out var bindElements) == false)
            {
                visualElement = null;
                return false;
            }

            if (bindElements.Remove(bindObject, out visualElement) == false)
            {
                return false;
            }

            if (bindElementsLookup.TryGetValue(bindName, out var bindObjects))
            {
                bindObjects.Remove(visualElement);
            }

            OnBindVisualElementChanged?.Invoke(bindName, bindObject, visualElement, added: false);
            
            return true;
        }

        public virtual bool TryGetVisualElement(string bindName, object bindObject, out VisualElement visualElement)
        {
            if (bindObject == null)
            {
                UnityEngine.Debug.LogError($"Cannot get a visual element for a null object.", this);
                visualElement = null;
                return false;
            }

            if (bindElementsLookup.TryGetValue(bindName, out var bindElements) == false)
            {
                visualElement = null;
                return false;
            }

            return bindElements.TryGetValue(bindObject, out visualElement);
        }

        public virtual bool TryGetBindObject(string bindName, VisualElement visualElement, out object bindObject)
        {
            if (visualElement == null)
            {
                UnityEngine.Debug.LogError($"Cannot get a bind object for a null visual element.", this);
                bindObject = null;
                return false;
            }

            if (bindObjectsLookup.TryGetValue(bindName, out var bindObjects) == false)
            {
                bindObject = null;
                return false;
            }

            return bindObjects.TryGetValue(visualElement, out bindObject);
        }

        public virtual IReadOnlyDictionary<object, VisualElement> GetBindInfos(string bindName)
        {
            if (bindElementsLookup.TryGetValue(bindName, out var bindElements) == false)
            {
                return DictionaryFactory<object, VisualElement>.Empty;
            }

            return bindElements;
        }
    }
}