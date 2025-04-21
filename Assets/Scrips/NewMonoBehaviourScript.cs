using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Obtiene la c�mara principal
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Orienta el plano hacia la c�mara
            transform.LookAt(mainCamera.transform);

            // Si deseas que la superficie se vea de frente y no de atr�s, voltea el objeto
            transform.Rotate(0, 180, 0);
        }
    }
}
