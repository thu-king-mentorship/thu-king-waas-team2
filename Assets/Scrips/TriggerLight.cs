using UnityEngine;

public class TriggerHeal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LightUI playerHealth = other.GetComponent<LightUI>();

            if (playerHealth != null)
            {
                playerHealth.SetHealing(true); // Activa la curaci�n
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LightUI playerHealth = other.GetComponent<LightUI>();

            if (playerHealth != null)
            {
                playerHealth.SetHealing(false); // Desactiva la curaci�n
            }
        }
    }
}
