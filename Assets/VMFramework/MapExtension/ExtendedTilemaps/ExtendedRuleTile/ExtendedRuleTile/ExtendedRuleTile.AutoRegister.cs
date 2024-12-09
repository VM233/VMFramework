using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    [GamePrefabTypeAutoRegister(EMPTY_TILE_ID)]
    public partial class ExtendedRuleTile : IGamePrefabAutoRegisterProvider
    {
        public const string EMPTY_TILE_ID = "empty_tile";

        void IGamePrefabAutoRegisterProvider.OnGamePrefabAutoRegister()
        {
            if (id == EMPTY_TILE_ID)
            {
                enableUpdate = false;
            }
        }
    }
}