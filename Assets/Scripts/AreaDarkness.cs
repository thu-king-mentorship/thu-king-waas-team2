using UnityEngine;

namespace WAAS
{
    /// <summary>
    /// Class <c>AreaDarkness</c> represents darkness areas, where the player's light is reduced gradually.
    /// </summary>
    public class AreaDarkness : MonoBehaviour
    {
        /// <value>Property <c>lightReduction</c> represents the amount of light to reduce when the player is in the darkness area.</value>
        [SerializeField]
        private int lightReduction = 1;

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called once per frame for every Collider other that is touching the trigger.
        /// </summary>
        /// <param name="other">The Collider that is touching the trigger.</param>
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            other.GetComponent<PlayerLight>().UseLight(lightReduction);
        }
    }
}
