using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>LightSourceController</c> represents areas where the player's light is increased gradually.
    /// </summary>
    public class LightSourceController : MonoBehaviour
    {
        /// <value>Property <c>lightIncrease</c> represents the amount of light to increase when the player is in the light area.</value>
        [SerializeField]
        private int lightIncrease = 1;
        
        /// <value>Property <c>lightRecoveryRate</c> represents the rate at which the light is restored.</value>
        [SerializeField]
        private float lightRecoveryRate = 0.1f;
        
        /// <value>Property <c>_lightRecoveryTimer</c> represents the timer for light recovery.</value>
        private float _lightRecoveryTimer;
        
        /// <value>Property <c>_playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        private PlayerLightController _playerLight;
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (_playerLight == null)
                return;
            _lightRecoveryTimer += Time.deltaTime;
            if (!(_lightRecoveryTimer >= lightRecoveryRate)) 
                return;
            _playerLight.RestoreLight(lightIncrease);
            _lightRecoveryTimer = 0f;
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The Collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerLight = other.GetComponent<PlayerLightController>();
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