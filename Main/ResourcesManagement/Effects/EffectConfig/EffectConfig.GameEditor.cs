#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.Effects
{
    public partial class EffectConfig : IGameEditorMenuTreeNode
    {
        EditorIcon IEditorIconProvider.Icon
        {
            get
            {
                if (prefab == null)
                {
                    return EditorIcon.None;
                }

                if (prefab.GetComponent<ParticleSystem>() != null)
                {
                    return SdfIconType.Flower1;
                }

                if (prefab.GetComponent<TrailRenderer>() != null)
                {
                    return SdfIconType.AlignBottom;
                }

                return EditorIcon.None;
            }
        }
    }
}
#endif