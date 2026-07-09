using UnityEngine;
using UnityEngine.VFX;
using VMFramework.OdinExtensions;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectTextureSetter : MonoBehaviour
    {
        [VisualEffectPropertyName]
        [IsNotNullOrEmpty]
        public string texturePropertyName;

        protected VisualEffect visualEffect;

        protected virtual void Awake()
        {
            visualEffect = GetComponentInParent<VisualEffect>();
        }

        public void SetTexture(Texture2D texture)
        {
            visualEffect.SetTexture(texturePropertyName, texture);
        }
    }
}