using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>ProjectileController</c> is a script that manages the projectile's behavior.
    /// </summary>
    public class ProjectileController : MonoBehaviour
    {
        /// <value>Property <c>lifetime</c> represents the time before the projectile is destroyed.</value>
        [SerializeField]
        private float lifetime = 3f;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Lightable"))
                Destroy(gameObject);

            
        }
    }
}