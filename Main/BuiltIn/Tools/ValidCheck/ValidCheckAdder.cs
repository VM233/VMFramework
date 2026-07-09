using System.Collections.Generic;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Core
{
    public class ValidCheckAdder : MonoBehaviour
    {
        [ComponentRequired(typeof(IValidCheckComponent))]
        [IsNotNullOrEmpty]
        public List<GameObject> checkComponentObjects = new();

        protected IValidCheckDispatcher dispatcher;
        protected readonly List<IValidCheckComponent> checkComponents = new();

        protected virtual void Awake()
        {
            checkComponents.Clear();
            foreach (var obj in checkComponentObjects)
            {
                var components = obj.GetComponentsInChildren<IValidCheckComponent>();
                checkComponents.AddRange(components);
            }

            dispatcher.OnCheckValid += OnCheckValid;
        }

        protected virtual void OnCheckValid(IValidCheckDispatcher validCheckDispatcher, ref bool isValid)
        {
            foreach (var checkComponent in checkComponents)
            {
                if (checkComponent.IsValid(validCheckDispatcher) == false)
                {
                    isValid = false;
                    break;
                }
            }
        }
    }
}