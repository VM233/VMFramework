using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.Cameras
{
    [ManagerCreationProvider(ManagerType.EnvironmentCore)]
    public sealed class CameraManager : ManagerBehaviour<CameraManager>
    {
        [SerializeField]
        private Camera _mainCamera;

        public static Camera MainCamera => Instance._mainCamera;

        public static CameraController MainCameraController =>
            MainCamera.GetComponent<CameraController>();

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (_mainCamera == null)
            {
                Debug.LogWarning($"没有在{nameof(CameraManager)}里设置{nameof(_mainCamera)}");
            }
        }
    }
}
