#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.ResourcesManagement
{
    public partial class EffectConfig : IGameEditorMenuTreeNode
    {
        Icon IGameEditorMenuTreeNode.Icon
        {
            get
            {
                if (prefab == null)
                {
                    return Icon.None;
                }

                if (prefab.GetComponent<ParticleSystem>() != null)
                {
                    return SdfIconType.Flower1;
                }

                if (prefab.GetComponent<TrailRenderer>() != null)
                {
                    return SdfIconType.AlignBottom;
                }

                return Icon.None;
            }
        }
    }
}
#endif