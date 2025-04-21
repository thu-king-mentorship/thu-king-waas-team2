using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // El sprite mira hacia la cámara pero mantiene el eje "arriba" global
        transform.forward = mainCamera.transform.forward;
    }
}
