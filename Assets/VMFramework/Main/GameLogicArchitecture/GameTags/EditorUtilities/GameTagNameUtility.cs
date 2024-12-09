#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GameTagNameUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<ValueDropdownItem> GetAllGameTagsNameList()
        {
            foreach (var gameTagInfo in GameTag.GetAllTags())
            {
                var name = ((INameOwner)gameTagInfo).Name;
                
                yield return new ValueDropdownItem(name, gameTagInfo.id);
            }
        }
    }
}
#endif