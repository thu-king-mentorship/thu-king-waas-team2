using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>LightableObjectController</c> is a script that controls the light of an object.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class LightableObjectController : MonoBehaviour
    {
        /// <value>Property <c>_light</c> represents the Light component attached to the object.</value>
        [SerializeField]
        private Light lightSource;

        /// <value>Property <c>lightPath</c> represents a LightPathController to draw a path when the light is enabled.</value>
        [SerializeField]
        private LightPathController lightPath;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (lightSource == null)
                lightSource = GetComponentInChildren<Light>();
            lightSource.enabled = false;
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Lighter"))
                return;
            EnableLight();
            lightPath?.ActivatePath();
        }

        /// <summary>
        /// Method <c>EnableLight</c> enables the light of the object.
        /// </summary>
        private void EnableLight()
        {
            lightSource.enabled = true;
        }
    }
}