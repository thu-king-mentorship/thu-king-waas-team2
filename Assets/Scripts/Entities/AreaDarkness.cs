using UnityEngine;

namespace WAAS.Entities
{
    /// <summary>
    /// Class <c>AreaDarkness</c> represents darkness areas, where the player's light is reduced gradually.
    /// </summary>
    public class AreaDarkness : MonoBehaviour
    {
        /// <value>Property <c>lightReduction</c> represents the amount of light to reduce when the player is in the darkness area.</value>
        [SerializeField]
        private int lightReduction = 1;
        
        /// <value>Property <c>_playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        private PlayerLight _playerLight;
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (_playerLight != null)
                _playerLight.UseLight(lightReduction);
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The Collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerLight = other.GetComponent<PlayerLight>();
        }
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The Collider that stopped touching the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerLight = null;
        }
    }
}
