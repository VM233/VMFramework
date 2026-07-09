#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.Editor
{
    internal sealed class ManagerViewerContainer : SimpleOdinEditorWindowContainer
    {
        [ShowInInspector]
        private Transform managerContainer => ManagerCreatorContainers.ManagerContainer;
        
        [ShowInInspector]
        private IReadOnlyDictionary<string, Transform> managerTypeContainers =>
            ManagerCreatorContainers.ManagerTypeContainers;
        
        [ShowInInspector]
        private List<Type> abstractManagerTypes;
        
        [ShowInInspector]
        private List<Type> interfaceManagerTypes;
        
        [Searchable]
        [ShowInInspector]
        private List<Type> managerTypes;
        
        [Button]
        private void CreateManagers()
        {
            ManagerCreator.CreateManagers();
        }

        public override void Init()
        {
            base.Init();

            abstractManagerTypes = ManagerCreator.AbstractManagerTypes.ToList();
            interfaceManagerTypes = ManagerCreator.InterfaceManagerTypes.ToList();
            managerTypes = ManagerCreator.ManagerTypes.ToList();
        }
    }
}
#endif