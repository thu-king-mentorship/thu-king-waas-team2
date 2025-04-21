using UnityEngine;

public class CubeChase : MonoBehaviour
{
    public Transform player;      // Referencia al transform del jugador
    public float speed = 5f;      // Velocidad a la que el cubo se mueve hacia el jugador
    public float stopDistance = 1f; // Distancia a la que el cubo debe detenerse cuando colisione
    public GameObject eye1;       // Referencia al cubo que cambiar� su material y luz (ojo 1)
    public GameObject eye2;       // Referencia adicional para otro cubo si es necesario

    private bool isMoving = true;  // Bandera que controla si el cubo est� movi�ndose o no

    void Update()
    {
        if (player == null)
            return;

        // Si el cubo est� movi�ndose, lo movemos hacia el jugador
        if (isMoving)
        {
            // Calculamos la direcci�n hacia el jugador
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            // Si estamos dentro del rango de parada, detenemos el movimiento
           
           
            
                direction.Normalize();  // Normalizamos la direcci�n para movimiento constante
                transform.position += direction * speed * Time.deltaTime;  // Mover el cubo

                // Hacer que el cubo rote hacia el jugador (solo la rotaci�n sobre el eje Y)
                Quaternion targetRotation = Quaternion.LookRotation(direction);  // Crear la rotaci�n hacia el jugador
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);  // Rotar suavemente
            
        }
    }

    // M�todo para detener el movimiento
    public void StopMovement()
    {
        isMoving = false;  // Detenemos el movimiento
        if (eye1 != null)
        {
            // Llamamos al m�todo ChangeMaterial de eye1
            eye1.GetComponent<EyeCube>().ChangeMaterial();
            eye2.GetComponent<EyeCube>().ChangeMaterial();
        }
    }

    // M�todo para reiniciar el movimiento, si lo deseas
    public void ResumeMovement()
    {
        isMoving = true;  // Reactivamos el movimiento
        eye1.GetComponent<EyeCube>().ChangeMaterial();
        eye2.GetComponent<EyeCube>().ChangeMaterial();
    }
}
