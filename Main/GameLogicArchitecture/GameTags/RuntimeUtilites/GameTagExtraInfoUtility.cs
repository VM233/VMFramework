using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTagExtraInfoUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetExtraInfo<TExtraInfo>(this IGameTagExtraInfosOwner owner, string gameTag,
            out TExtraInfo extraInfo)
        {
            if (owner.GameTagExtraInfos == null)
            {
                extraInfo = default;
                return false;
            }

            return owner.GameTagExtraInfos.TryGetValueAs(gameTag, out extraInfo);
        }
    }
}