using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.Cameras
{
    [ManagerCreationProvider(ManagerType.EnvironmentCore)]
    public sealed class CameraManager : ManagerBehaviour<CameraManager>
    {
        [Required]
        public Camera mainCamera;

        public Camera MainCamera => mainCamera;

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (mainCamera == null)
            {
                Debug.LogWarning($"没有在{nameof(CameraManager)}里设置{nameof(mainCamera)}");
            }
        }
    }
}