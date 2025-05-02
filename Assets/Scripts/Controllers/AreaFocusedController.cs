using UnityEngine;
using Unity.Cinemachine;
using WAAS.Managers;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>AreaFocusedController</c> is responsible for controlling the camera focus point
    /// </summary>
    public class AreaFocusedController : MonoBehaviour
    {
        /// <value>Property <c>virtualCamera</c> is the Cinemachine virtual camera to be controlled.</value>
        [SerializeField]
        private CinemachineCamera virtualCamera;

        /// <value>Property <c>focusPoint</c> is the transform that the camera will focus on.</value>
        [SerializeField]
        private Transform focusPoint;

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || focusPoint == null)
                return;
            CameraFollowManager.Instance.SetFollowTarget(focusPoint);
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the collider other exits the trigger.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            CameraFollowManager.Instance.SetFollowTarget(other.transform);
        }
    }
}