#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.ResourcesManagement
{
    internal sealed class SpritePresetUnregisterUnit : GamePrefabUnregisterUnit<Sprite>
    {
        protected override string ProcessButtonName => "Unregister SpritePresets";

        protected override void OnProcessAsset(Sprite sprite)
        {
            GamePrefabWrapperRemover.RemoveGamePrefabWrapperWhere<SpritePreset>(prefab =>
                prefab.sprite == sprite);
        }
    }
}
#endif