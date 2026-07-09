using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Timers
{
    public class GameItemReturnTimer : Timer<ulong>
    {
        protected IGameItem gameItem;

        public void Set(IGameItem gameItem)
        {
            this.gameItem = gameItem;
        }
    }
}