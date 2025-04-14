using UnityEngine;
using Unity.Cinemachine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>FocusedAreaController</c> is responsible for controlling the camera focus point
    /// </summary>
    public class FocusedAreaController : MonoBehaviour
    {
        /// <value>Property <c>virtualCamera</c> is the Cinemachine virtual camera to be controlled.</value>
        [SerializeField]
        private CinemachineCamera virtualCamera;

        /// <value>Property <c>focusPoint</c> is the transform that the camera will focus on.</value>
        [SerializeField]
        private Transform focusPoint;

        /// <value>Property <c>player</c> is the transform of the player.</value>
        private Transform _player;

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            Debug.Log("Player entered the trigger");
            virtualCamera.Follow = focusPoint;
            _player = other.transform;
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the collider other exits the trigger.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            Debug.Log("Player exited the trigger");
            virtualCamera.Follow = _player;
            _player = null;
        }
    }
}