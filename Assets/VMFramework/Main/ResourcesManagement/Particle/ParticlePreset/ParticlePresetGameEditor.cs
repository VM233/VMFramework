﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor;

namespace VMFramework.ResourcesManagement
{
    public partial class ParticlePreset : IGameEditorMenuTreeNode
    {
        public Sprite spriteIcon => AssetPreview.GetAssetPreview(particlePrefab).ToSprite();
    }
}
#endif