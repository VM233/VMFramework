#if FISHNET
using System;
using FishNet.Connection;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class Container
    {
        public bool IsDirty { get; set; }
        
        public event Action<IContainer> OnOpenOnServerEvent;
        public event Action<IContainer> OnCloseOnServerEvent;
        
        #region Open & Close

        public void OpenOnServer()
        {
            if (IsDebugging)
            {
                Debugger.LogWarning($"{this} opened on server");
            }

            OnOpenOnServerEvent?.Invoke(this);
        }

        public void CloseOnServer()
        {
            if (IsDebugging)
            {
                Debugger.LogWarning($"{this} closed on server");
            }

            OnCloseOnServerEvent?.Invoke(this);
        }

        #endregion
    }
}
#endif