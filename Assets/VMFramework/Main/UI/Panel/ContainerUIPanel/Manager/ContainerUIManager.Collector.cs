using System.Collections.Generic;

namespace VMFramework.UI
{
    public partial class ContainerUIManager
    {
        private static readonly HashSet<IContainerUIPanel> openedPanels = new();

        private static void OnPanelOpenCollectorRegister(IUIPanel panel)
        {
            var containerPanel = (IContainerUIPanel)panel;
            openedPanels.Add(containerPanel);
        }

        private static void OnPanelCloseCollectorUnregister(IUIPanel panel)
        {
            var containerPanel = (IContainerUIPanel)panel;
            openedPanels.Remove(containerPanel);
        }
    }
}