using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IUIPanel : IControllerGameItem, IToken
    {
        public bool IsUnique { get; }
        
        public bool IsOpened { get; }
        
        public bool UIEnabled { get; }
        
        public IUIPanel SourceUIPanel { get; }
        
        public IReadOnlyCollection<IPanelModifier> Modifiers { get; }

        public event Action<IUIPanel> OnOpenEvent;

        public event Action<IUIPanel> OnPreCloseEvent;
        
        public event Action<IUIPanel> OnPostCloseEvent;
        
        public event Action<IUIPanel> OnDestructEvent;

        public void OnOpen(IUIPanel source);
        
        public void OnClose();

        public void OnPostClose();

        public void SetEnabled(bool enableState);
    }
}