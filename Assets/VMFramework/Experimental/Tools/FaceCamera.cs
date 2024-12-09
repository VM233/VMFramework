using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Cameras;

public class FaceCamera : MonoBehaviour
{
    [SerializeField]
    [Required]
    private Camera cameraToFace;

    private void Start()
    {
        if (cameraToFace == null)
        {
            cameraToFace = Camera.main;
        }

        if (cameraToFace == null)
        {
            cameraToFace = CameraManager.MainCamera;
        }
    }

    void Update()
    {
        transform.rotation =
            Quaternion.LookRotation(transform.position -
                                    cameraToFace.transform.position);
    }
}
