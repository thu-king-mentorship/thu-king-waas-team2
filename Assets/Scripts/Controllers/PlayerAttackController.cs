using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerAttackController</c> is a script that manages the player's attack actions.
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        #region Attack Properties

            /// <value>Property <c>meleeRange</c> represents the range of the melee attack.</value>
            [Header("Melee Attack")]
            [SerializeField]
            private float meleeRange = 1.5f;
            
            /// <value>Property <c>meleeHitboxSize</c> represents the size of the melee hitbox.</value>
            [SerializeField]
            private Vector2 meleeHitboxSize = new Vector2(1.2f, 1.2f);
            
            /// <value>Property <c>meleeDamage</c> represents the damage dealt by the melee attack.</value>
            [SerializeField]
            private float meleeDamage = 10f;
            
            /// <value>Property <c>enemyLayer</c> represents the layer mask for the enemy.</value>
            [SerializeField]
            private LayerMask enemyLayer;

            /// <value>Property <c>projectilePrefab</c> represents the prefab for the projectile.</value>
            [Header("Ranged Attack")]
            [SerializeField]
            private GameObject projectilePrefab;
            
            /// <value>Property <c>projectileSpeed</c> represents the speed of the projectile.</value>
            [SerializeField]
            private float projectileSpeed = 10f;
            
            /// <value>Property <c>projectileSpawnOrigin</c> represents the origin point from where the projectile is spawned.</value>
            [SerializeField]
            private Transform projectileSpawnOrigin;
            
            /// <value>Property <c>projectileOffsetDistance</c> represents the distance from the spawn origin to the projectile.</value>
            [SerializeField] private float projectileOffsetDistance = 0.65f;
        
        #endregion
        
        #region References

            /// <value>Property <c>_movementController</c> represents the PlayerMovementController component attached to the player GameObject.</value>
            private PlayerMovementController _movementController;
            
            /// <value>Property <c>_stateController</c> represents the PlayerStateController component attached to the player GameObject.</value>
            private PlayerStateController _stateController;
        
        #endregion

        #region Animation Properties

            /// <value>Property <c>animator</c> represents the Animator component attached to the player GameObject.</value>
            [Header("Animation Properties")]
            [SerializeField]
            private Animator animator;
        
        #endregion

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
            _stateController = GetComponent<PlayerStateController>();
        }

        /// <summary>
        /// Method <c>OnAttack</c> is called when the player performs a melee attack.
        /// </summary>
        public void OnAttack()
        {
            if (_stateController.HasState(PlayerState.Attacking) || _stateController.HasState(PlayerState.Jumping))
                return;

            _stateController.SetState(PlayerState.Attacking);
            animator.Play("Attack");

            var dir = _movementController.LastDirection.normalized;
            var hitCenter = transform.position + new Vector3(dir.x + dir.y, 0, dir.y - dir.x).normalized * meleeRange;

            var hits = new Collider[8];
            Physics.OverlapBoxNonAlloc(hitCenter, new Vector3(meleeHitboxSize.x / 2, 1f, meleeHitboxSize.y / 2), hits, Quaternion.identity, enemyLayer);
        }

        /// <summary>
        /// Method <c>OnAttackAnimationEnd</c> is called when the melee attack animation ends.
        /// </summary>
        public void OnAttackAnimationEnd()
        {
            _stateController.UnsetState(PlayerState.Attacking);
        }


        /// <summary>
        /// Method <c>OnRangedAttack</c> is called when the player performs a ranged attack.
        /// </summary>
        public void OnRangedAttack()
        {
            if (_stateController.HasState(PlayerState.Attacking) || _stateController.HasState(PlayerState.Jumping))
                return;
            if (projectilePrefab == null || projectileSpawnOrigin == null)
                return;
            _stateController.SetState(PlayerState.Attacking);
            animator.Play("RangedAttack");
        }

        /// <summary>
        /// Method <c>FireProjectile</c> is called to instantiate and fire a projectile.
        /// </summary>
        public void FireProjectile()
        {
            var dir = _movementController.LastDirection.normalized;
            var shootDir = new Vector3(dir.x + dir.y, 0, dir.y - dir.x).normalized;
            var spawnPos = projectileSpawnOrigin.position + shootDir * projectileOffsetDistance;

            var projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = shootDir * projectileSpeed;
        }

        /// <summary>
        /// Method <c>OnRangedAttackAnimationEnd</c> is called when the ranged attack animation ends.
        /// </summary>
        public void OnRangedAttackAnimationEnd()
        {
            _stateController.UnsetState(PlayerState.Attacking);
        }

        /// <summary>
        /// Method <c>OnDrawGizmosSelected</c> is called when the object is selected in the editor.
        /// </summary>
        #if UNITY_EDITOR
            private void OnDrawGizmosSelected()
            {
                if (_movementController == null) return;

                var dir = _movementController.LastDirection.normalized;
                var hitCenter = transform.position + new Vector3(dir.x + dir.y, 0, dir.y - dir.x).normalized * meleeRange;

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(hitCenter, new Vector3(meleeHitboxSize.x, 1f, meleeHitboxSize.y));
            }
        #endif
    }
}
