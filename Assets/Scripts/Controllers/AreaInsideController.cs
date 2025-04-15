using UnityEngine;
using WAAS.Managers;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>AreaInsideController</c> is responsible for managing the visibility of outside objects when the player enters or exits a trigger area.
    /// </summary>
    public class AreaInsideController : MonoBehaviour
    {

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            EnvironmentManager.Instance.HideOutside();
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the collider other exits the trigger.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            EnvironmentManager.Instance.ShowOutside();
        }
    }
}
