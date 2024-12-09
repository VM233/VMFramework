using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class CameraUtility
    {
        /// <summary>
        /// 获取透视相机的尺寸
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPerspectiveCameraSize(this Camera camera, float distance)
        {
            if (distance <= 0 || distance > camera.farClipPlane)
            {
                distance = camera.farClipPlane;
            }

            // fov表示相机的垂直视野角度
            // 2 * tan(fov/2) = height / distance
            float height = 2 * distance * Mathf.Tan(camera.fieldOfView / 2 * Mathf.Deg2Rad);
            float width = height * camera.aspect;

            return new Vector2(width, height);
        }

        /// <summary>
        /// 获取透视相机的矩形区域
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat GetPerspectiveCameraRectangle(this Camera camera, float distance)
        {
            Vector3 cameraPos = camera.transform.position;
            Vector2 size = GetPerspectiveCameraSize(camera, distance);
            return RectangleFloat.FromPivotSize(cameraPos.XY(), size);
        }
        
        /// <summary>
        /// 获取正交相机的尺寸
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetOrthographicCameraSize(this Camera camera)
        {
            float height = camera.orthographicSize * 2; //高度 = 正交相机的size*2
            float width = height * camera.aspect; //宽度 = 高度*宽高比

            return new Vector2(width, height);
        }
        
        /// <summary>
        /// 获取正交相机的矩形区域
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleFloat GetOrthographicCameraRectangle(this Camera camera)
        {
            Vector3 cameraPos = camera.transform.position;
            Vector2 size = GetOrthographicCameraSize(camera);
            return RectangleFloat.FromPivotSize(cameraPos.XY(), size);
        }
    }
}