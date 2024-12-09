namespace VMFramework.UI
{
    public interface IContextMenu : IUIPanel
    {
        public void Open(IContextMenuProvider contextMenuProvider, IUIPanel source);

        public void Close(IContextMenuProvider contextMenuProvider);
    }
}