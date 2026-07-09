using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IUIPanel : IControllerGameItem, IToken, IBindObjectsManagerProvider
    {
        public delegate void OpenHandler(IUIPanel panel);

        public delegate void ReenterHandler(IUIPanel panel);

        public delegate void CloseHandler(IUIPanel panel);

        public delegate void ClosableCheckHandler(IUIPanel panel, ref bool closable);

        public delegate void DestructHandler(IUIPanel panel);

        public bool IsUnique { get; }

        public bool IsOpened { get; }

        public bool IsClosing { get; }

        public bool UIEnabled { get; }

        public IUIPanel SourceUIPanel { get; }

        public IReadOnlyCollection<IPanelModifier> Modifiers { get; }

        public event OpenHandler OnOpen;

        public event ReenterHandler OnReenter;

        public event CloseHandler OnPreClose;

        public event CloseHandler OnPostClose;

        public event ClosableCheckHandler OnCheckClosable;

        public event DestructHandler OnDestruct;

        internal void OnOpenInternal(IUIPanel source);

        internal void OnPreCloseInternal();

        internal void OnPostCloseInternal();

        /// <summary>
        /// 重新进入UI，只在<see cref="IsOpened"/>为true时有效
        /// </summary>
        public void Reenter();

        public void SetEnabled(bool enableState);
    }
}