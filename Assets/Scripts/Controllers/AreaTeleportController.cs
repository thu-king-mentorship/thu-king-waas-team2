using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>AreaTeleportController</c> is responsible for teleporting the player to a specified exit point when they enter the trigger area.
    /// </summary>
    public class AreaTeleportController : MonoBehaviour
    {
        /// <value>Property <c>targetExitPoint</c> is the transform that represents the exit point for teleportation.</value>
        [SerializeField]
        private Transform targetExitPoint;

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || targetExitPoint == null)
                return;

            var characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
                other.transform.position = targetExitPoint.position;
                characterController.enabled = true;
            }
            else
            {
                other.transform.position = targetExitPoint.position;
            }
        }
    }
}