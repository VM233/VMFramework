#if UNITY_EDITOR
using UnityEngine.Tilemaps;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.Maps;

namespace VMFramework.MapExtension
{
    internal sealed class TileBaseConfigsUnregisterUnit : GamePrefabUnregisterUnit<TileBase>
    {
        protected override string ProcessButtonName => "Unregister TileBaseConfig";

        protected override void OnProcessAsset(TileBase tileBase)
        {
            GamePrefabWrapperRemover.RemoveGamePrefabWrapperWhere<DefaultTileBaseConfig>(config =>
                config.tile == tileBase);
        }
    }
}
#endif