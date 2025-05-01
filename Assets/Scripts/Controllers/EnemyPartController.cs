using System;
using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Method <c>GolemPart</c> is a script that controls the golem part.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class EnemyPartController : MonoBehaviour
    {
        /// <value>Property <c>enemyDisassemblyController</c> is a reference to the EnemyDisassemblyController.</value>
        [SerializeField]
        private EnemyDisassemblyController enemyDisassemblyController;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (enemyDisassemblyController == null)
                enemyDisassemblyController = GetComponentInParent<EnemyDisassemblyController>();
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the collider other enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Lighter"))
                return;
            var projectileDirection = (transform.position - other.transform.position).normalized;
            enemyDisassemblyController.OnHit(projectileDirection);
        }
    }
}