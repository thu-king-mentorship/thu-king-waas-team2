using UnityEngine;

public class ConeDetection : MonoBehaviour
{



    // Este m�todo se llama cuando un collider entra en el rango del trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube")) // Aseg�rate de que el cubo tenga la etiqueta "Cube"
        {
            other.GetComponent<CubeChase>().StopMovement();
        }
    }

    // Este m�todo se llama cuando un collider sale del rango del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cube")) // Aseg�rate de que el cubo tenga la etiqueta "Cube"
        {
            // Restauramos el color por defecto del cubo cuando sale del rango
            other.GetComponent<CubeChase>().ResumeMovement();
        }
    }
}
