using UnityEngine;

namespace WAAS
{
    /// <summary>
    /// Class <c>AreaLight</c> represents areas where the player's light is increased gradually.
    /// </summary>
    public class AreaLight : MonoBehaviour
    {
        /// <value>Property <c>lightIncrease</c> represents the amount of light to increase when the player is in the light area.</value>
        [SerializeField]
        private int lightIncrease = 1;
        
        /// <summary>
        /// Method <c>OnTriggerStay</c> is called once per frame for every Collider other that is touching the trigger.
        /// </summary>
        /// <param name="other">The Collider that is touching the trigger.</param>
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            other.GetComponent<PlayerLight>().RestoreLight(lightIncrease);
        }
    }
}
