#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    public interface IGameEditorMenuTreeNodesProvider : INameOwner
    {
        public bool AutoStackMenuTreeNodes => false;

        public bool IsMenuTreeNodesVisible => true;

        public IEnumerable<object> GetAllMenuTreeNodes();
    }
}
#endif