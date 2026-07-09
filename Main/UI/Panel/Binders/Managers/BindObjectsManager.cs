using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.UI
{
    public class BindObjectsManager : PanelModifier
    {
        public const string GLOBAL_BIND_NAME = "Bind Objects";

        public delegate void BindObjectChangedHandler(string bindName, object bindObject, object parentObject,
            bool added);

        public IReadOnlyCollection<string> BindNames => bindNames;

        public IReadOnlyCollection<string> SingleModeNames => singleModeNames;

        public event BindObjectChangedHandler OnBindObjectPreChanged;
        public event BindObjectChangedHandler OnBindObjectChanged;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, HashSet<object>> bindObjectsByName = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, Dictionary<object, HashSet<object>>> subObjectsByName = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly HashSet<string> bindNames = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly HashSet<string> singleModeNames = new();

        protected readonly Dictionary<string, HashSet<object>> addQueue = new();

        public virtual void Collect()
        {
            bindNames.Clear();
            singleModeNames.Clear();

            bindNames.Add(GLOBAL_BIND_NAME);

            foreach (var bindObjectsNamesProvider in GetComponentsInChildren<IBindObjectsNamesProvider>())
            {
                bindObjectsNamesProvider.GetBindObjectsNames(bindNames, singleModeNames);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Collect();

            Panel.OnPostClose += OnClose;
        }

        protected virtual void OnClose(IUIPanel panel)
        {
            ClearAllObjects();
        }

        protected virtual void Update()
        {
            if (Panel.IsOpened == false)
            {
                return;
            }

            foreach (var (bindName, bindObjects) in addQueue)
            {
                if (bindObjects.Count > 0)
                {
                    AddObjects(bindName, bindObjects);
                    bindObjects.Clear();
                }
            }
        }

        protected virtual void InvokeChange(string bindName, object bindObject, object parentObject, bool added)
        {
            OnBindObjectPreChanged?.Invoke(bindName, bindObject, parentObject, added);
            OnBindObjectChanged?.Invoke(bindName, bindObject, parentObject, added);
        }

        public virtual void AddObject(object bindObject)
        {
            AddObject(GLOBAL_BIND_NAME, bindObject);
        }

        public virtual void AddObjects(IEnumerable<object> bindObjects)
        {
            AddObjects(GLOBAL_BIND_NAME, bindObjects);
        }

        public virtual void RemoveObject(object bindObject)
        {
            RemoveObject(GLOBAL_BIND_NAME, bindObject);
        }

        public virtual void ClearObjects()
        {
            ClearObjects(GLOBAL_BIND_NAME);
        }

        public virtual bool ContainsObject(object bindObject)
        {
            return ContainsObject(GLOBAL_BIND_NAME, bindObject);
        }

        public virtual void DelayedAddObject(string bindName, object bindObject)
        {
            var targetBindObjects = addQueue.GetOrCreate(bindName);
            targetBindObjects.Add(bindObject);
        }

        public virtual void DelayedAddObjects(string bindName, IEnumerable<object> bindObjects)
        {
            var targetBindObjects = addQueue.GetOrCreate(bindName);
            targetBindObjects.AddRange(bindObjects);
        }

        public virtual void AddObject(string bindName, object bindObject)
        {
            if (bindName.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogWarning($"Cannot add object with null or empty bind name.", this);
                return;
            }

            if (bindObject == null)
            {
                UnityEngine.Debug.LogWarning($"Cannot add null object to bind name {bindName}.", this);
                return;
            }

            var bindObjects = bindObjectsByName.GetOrCreate(bindName);

            if (singleModeNames.Contains(bindName))
            {
                if (bindObjects.Count > 0)
                {
                    ClearObjects(bindName);
                }
            }

            if (bindObjects.Add(bindObject))
            {
                InvokeChange(bindName, bindObject, parentObject: null, added: true);
            }
        }

        public virtual void AddObjects(string bindName, IEnumerable<object> bindObjects)
        {
            var tempBindObjects = bindObjects.ToListDefaultPooled();
            foreach (var bindObject in tempBindObjects)
            {
                AddObject(bindName, bindObject);
            }

            tempBindObjects.ReturnToDefaultPool();
        }

        public virtual void RemoveObject(string bindName, object bindObject)
        {
            if (bindName.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogWarning($"Cannot remove object with null or empty bind name.", this);
                return;
            }

            if (bindObject == null)
            {
                UnityEngine.Debug.LogWarning($"Cannot remove null object from bind name {bindName}.", this);
                return;
            }

            if (bindObjectsByName.TryGetValue(bindName, out var bindObjects))
            {
                if (bindObjects.Remove(bindObject))
                {
                    InvokeChange(bindName, bindObject, parentObject: null, added: false);
                }
            }

            if (addQueue.TryGetValue(bindName, out var addQueueObjects))
            {
                addQueueObjects.Remove(bindObject);
            }
        }

        public virtual void RemoveObjects(string bindName, IEnumerable<object> bindObjects)
        {
            var tempBindObjects = bindObjects.ToListDefaultPooled();
            foreach (var bindObject in tempBindObjects)
            {
                RemoveObject(bindName, bindObject);
            }

            tempBindObjects.ReturnToDefaultPool();
        }

        public virtual void ClearObjects(string bindName)
        {
            if (bindName.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogWarning($"Cannot clear objects with null or empty bind name.", this);
                return;
            }

            if (bindObjectsByName.TryGetValue(bindName, out var bindObjects))
            {
                var tempBindObjects = bindObjects.ToListDefaultPooled();
                bindObjects.Clear();

                foreach (var bindObject in tempBindObjects)
                {
                    InvokeChange(bindName, bindObject, parentObject: null, added: false);
                }

                tempBindObjects.ReturnToDefaultPool();
            }

            if (addQueue.TryGetValue(bindName, out var addQueueObjects))
            {
                addQueueObjects.Clear();
            }
        }

        public virtual void ClearAllObjects()
        {
            var bindNames = bindObjectsByName.Keys.ToListDefaultPooled();
            foreach (var bindName in bindNames)
            {
                ClearObjects(bindName);
            }

            bindNames.ReturnToDefaultPool();
        }

        public virtual bool AddObject(string bindName, object bindObject, object parentObject)
        {
            var subObjectsLookup = subObjectsByName.GetOrCreate(bindName);

            if (subObjectsLookup.TryGetValue(parentObject, out var subObjects) == false)
            {
                subObjects = HashSetPool<object>.Default.Get();
                subObjects.Clear();
                subObjectsLookup.Add(parentObject, subObjects);
            }

            if (subObjects.Add(bindObject))
            {
                InvokeChange(bindName, bindObject, parentObject, added: true);
                return true;
            }

            return false;
        }

        public virtual void RemoveObject(string bindName, object bindObject, object parentObject)
        {
            if (subObjectsByName.TryGetValue(bindName, out var subObjectsLookup) == false)
            {
                return;
            }

            if (subObjectsLookup.TryGetValue(parentObject, out var subObjects) == false)
            {
                return;
            }

            if (subObjects.Remove(bindObject))
            {
                if (subObjects.Count == 0)
                {
                    subObjectsLookup.Remove(parentObject);
                    subObjects.ReturnToDefaultPool();
                }

                InvokeChange(bindName, bindObject, parentObject, added: false);
            }
        }

        public virtual object GetObject(string bindName)
        {
            if (singleModeNames.Contains(bindName) == false)
            {
                throw new ArgumentException($"Bind name {bindName} is not in single mode.");
            }

            if (bindObjectsByName.TryGetValue(bindName, out var bindObjects) == false)
            {
                return null;
            }

            return bindObjects.FirstOrDefault();
        }

        public virtual IReadOnlyCollection<object> GetObjects(string bindName)
        {
            if (bindObjectsByName.TryGetValue(bindName, out var bindObjects))
            {
                return bindObjects;
            }

            return Array.Empty<object>();
        }

        public virtual IReadOnlyCollection<object> GetSubObjects(string bindName, object parentObject)
        {
            if (subObjectsByName.TryGetValue(bindName, out var subObjectsLookup) == false)
            {
                return Array.Empty<object>();
            }

            if (subObjectsLookup.TryGetValue(parentObject, out var subObjects) == false)
            {
                return Array.Empty<object>();
            }

            return subObjects;
        }

        public virtual bool ContainsObject(string bindName, object bindObject)
        {
            if (bindObjectsByName.TryGetValue(bindName, out var bindObjects))
            {
                return bindObjects.Contains(bindObject);
            }

            return false;
        }
    }
}