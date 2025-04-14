using System.Collections;
using UnityEngine;
using WAAS.ScriptableObjects;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerAttackController</c> is a script that manages the player's attack actions.
    /// </summary>
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerLightController))]
    public class PlayerAttackController : MonoBehaviour
    {
        #region Attack Properties

            /// <value>Property <c>meleeSettings</c> represents the melee attack settings.</value>
            [Header("Attack Properties")]
            [SerializeField]
            private AttackSettingsMelee meleeSettings;
            
            /// <value>Property <c>rangedSettings</c> represents the ranged attack settings.</value>
            [SerializeField]
            private AttackSettingsRanged rangedSettings;
            
            /// <value>Property <c>projectileSpawnOrigin</c> indicates the origin point from where the projectile is spawned.</value>
            [SerializeField]
            private Transform projectileSpawnOrigin;
        
            /// <value>Property <c>projectileSpawnOrigin</c> indicates the origin point from where the projectile is spawned.</value>
            [SerializeField]
            private float projectileOffsetDistance = 1.0f;
            
            /// <value>Property <c>enemyLayer</c> indicates the layer mask for the enemy.</value>
            [SerializeField]
            private LayerMask enemyLayer;
            
            /// <value>Property <c>_canMeleeAttack</c> indicates whether the player can perform a melee attack.</value>
            private bool _canMeleeAttack = true;
            
            /// <value>Property <c>_canRangedAttack</c> indicates whether the player can perform a ranged attack.</value>
            private bool _canRangedAttack = true;
            
            /// <value>Property <c>ShouldBlockMovement</c> indicates whether the player should block movement during an attack.</value>
            public bool ShouldBlockMovement =>
                (_stateController.HasState(PlayerState.AttackingMelee) && !meleeSettings.allowAttackWhileMoving) ||
                (_stateController.HasState(PlayerState.AttackingRanged) && !rangedSettings.allowAttackWhileMoving);
            
            /// <value>Property <c>_reservedLight</c> indicates the amount of light reserved for the attack.</value>
            private int _reservedLight;
        
        #endregion

        #region Animation Properties

            /// <value>Property <c>animator</c> represents the Animator component attached to the player GameObject.</value>
            [Header("Animation Properties")]
            [SerializeField]
            private Animator animator;
            
            /// <value>Property <c>AttackHash</c> is the hash value for the attack animation.</value>
            private static readonly int AttackHash = Animator.StringToHash("Attack");
            
            /// <value>Property <c>RangedAttackHash</c> is the hash value for the ranged attack animation.</value>
            private static readonly int RangedAttackHash = Animator.StringToHash("RangedAttack");
        
        #endregion
        
        #region References
            
            /// <value>Property <c>_stateController</c> represents the PlayerStateController component attached to the player GameObject.</value>
            private PlayerStateController _stateController;

            /// <value>Property <c>_movementController</c> represents the PlayerMovementController component attached to the player GameObject.</value>
            private PlayerMovementController _movementController;
                
            /// <value>Property <c>_attackController</c> represents the PlayerAttackController component attached to the player GameObject.</value>
            private PlayerLightController _lightController;
        
        #endregion
        
        #region Unity Events Methods

            /// <summary>
            /// Method <c>Awake</c> is called when the script instance is being loaded.
            /// </summary>
            private void Awake()
            {
                _movementController = GetComponent<PlayerMovementController>();
                _stateController = GetComponent<PlayerStateController>();
                _lightController = GetComponent<PlayerLightController>();
                if (animator == null)
                    throw new MissingReferenceException("Animator component is not assigned.");
            }
        
        #endregion
        
        #region General Attack Methods
        
            /// <summary>
            /// Method <c>CheckAttackConditions</c> checks if the player can perform an attack based on the provided settings.
            /// </summary>
            /// <param name="settings">The attack settings to check.</param>
            /// <returns>True if the player can perform the attack; otherwise, false.</returns>
            private bool CheckAttackConditions(AttackSettings settings)
            {
                // Check if the player is already attacking
                if (settings.timingMode != AttackTimingMode.Continuous 
                        && (_stateController.HasState(PlayerState.AttackingMelee) 
                            || _stateController.HasState(PlayerState.AttackingRanged)))
                    return false;

                // Check if the attack is allowed while moving or jumping
                if (!settings.allowAttackWhileMoving
                    && (_stateController.HasState(PlayerState.Moving)
                        || _stateController.HasState(PlayerState.Jumping)))
                    return false;

                // Check if the cooldown is active
                var isMelee = settings is AttackSettingsMelee;
                var canAttack = isMelee ? _canMeleeAttack : _canRangedAttack;
                if (settings.timingMode == AttackTimingMode.Cooldown && !canAttack)
                    return false;
                
                // Check if the player has enough light for the attack
                if (!TryReserveLight(settings.lightCost))
                    return false;

                // All conditions are met
                return true;
            }
            
            /// <summary>
            /// Method <c>StartCooldown</c> starts a cooldown routine for the specified duration.
            /// </summary>
            /// <param name="onCooldownEnd">Action to be called when the cooldown ends.</param>
            /// <param name="duration">The duration of the cooldown.</param>
            /// <returns>IEnumerator that represents the cooldown routine.</returns>
            private IEnumerator CooldownRoutine(System.Action onCooldownEnd, float duration)
            {
                yield return new WaitForSeconds(duration);
                onCooldownEnd?.Invoke();
            }
        
        #endregion
        
        #region Melee Attack Methods

            /// <summary>
            /// Method <c>OnAttack</c> is called when the player performs a melee attack.
            /// </summary>
            public void OnAttack()
            {
                // Check if the player can perform a melee attack
                if (!CheckAttackConditions(meleeSettings))
                    return;
                
                // Deduct light cost for the attack
                CommitReservedLight();
                
                // Set the player state to Attacking and play the attack animation
                _stateController.SetState(PlayerState.AttackingMelee);
                animator.Play(AttackHash, 0, 0.0f);

                // Reset the melee cooldown timer and activate it
                if (meleeSettings.timingMode != AttackTimingMode.Cooldown)
                    return;
                _canMeleeAttack = false;
                StartCoroutine(CooldownRoutine(() => _canMeleeAttack = true, meleeSettings.attackCooldown));
            }
            
            /// <summary>
            /// Method <c>CheckMeleeAttackHits</c> checks for hits during the melee attack.
            /// </summary>
            private void CheckMeleeAttackHits()
            {
                var dir = _movementController.LastDirection.normalized;
                var hitCenter = transform.position
                                + new Vector3(dir.x + dir.y, 0, dir.y - dir.x).normalized * meleeSettings.meleeRange;
                var hits = new Collider[8];
                Physics.OverlapBoxNonAlloc(
                    hitCenter,
                    new Vector3(meleeSettings.meleeHitboxSize.x / 2, 1f, meleeSettings.meleeHitboxSize.y / 2),
                    hits,
                    Quaternion.identity,
                    enemyLayer
                );
            }
        
        #endregion
        
        #region Ranged Attack Methods
        
            /// <summary>
            /// Method <c>OnRangedAttack</c> is called when the player performs a ranged attack.
            /// </summary>
            public void OnRangedAttack()
            {
                // Check if the player can perform a ranged attack
                if (!CheckAttackConditions(rangedSettings))
                    return;
                
                // Check if the projectile prefab and spawn origin are set
                if (rangedSettings.projectilePrefab == null || projectileSpawnOrigin == null)
                    return;
                
                // Deduct light cost for the attack
                CommitReservedLight();

                // Set the player state to Attacking and play the ranged attack animation
                _stateController.SetState(PlayerState.AttackingRanged);
                animator.Play(RangedAttackHash, 0, 0.0f);

                // Spawn logic may be triggered by animation event in AnimationEvent mode
                if (rangedSettings.timingMode is AttackTimingMode.Cooldown or AttackTimingMode.Continuous)
                    FireProjectile();

                // Reset the ranged cooldown timer and activate it
                if (rangedSettings.timingMode != AttackTimingMode.Cooldown)
                    return;
                _canRangedAttack = false;
                StartCoroutine(CooldownRoutine(() => _canRangedAttack = true, rangedSettings.attackCooldown));
            }

            /// <summary>
            /// Method <c>FireProjectile</c> is called to instantiate and fire a projectile.
            /// </summary>
            private void FireProjectile()
            {
                var dir = _movementController.LastDirection.normalized;
                var shootDir = new Vector3(dir.x + dir.y, 0, dir.y - dir.x).normalized;
                var spawnPos = projectileSpawnOrigin.position + shootDir * projectileOffsetDistance;

                var projectile = Instantiate(rangedSettings.projectilePrefab, spawnPos, Quaternion.identity);
                var rb = projectile.GetComponent<Rigidbody>();
                if (rb == null)
                    return;
                rb.useGravity = false;
                rb.linearVelocity = shootDir * rangedSettings.projectileSpeed;
            }
        
        #endregion
        
        #region Light Methods
        
            /// <summary>
            /// Method <c>TryReserveLight</c> attempts to reserve light for the attack.
            /// </summary>
            /// <param name="amount">The amount of light to reserve.</param>
            /// <returns>Boolean indicating whether the reservation was successful.</returns>
            private bool TryReserveLight(int amount)
            {
                if (_lightController == null || _lightController.CurrentLight < amount)
                    return false;
                _reservedLight += amount;
                return true;
            }
            
            /// <summary>
            /// Method <c>CommitReservedLight</c> commits the reserved light to the light controller.
            /// </summary>
            private void CommitReservedLight()
            {
                if (_lightController == null)
                    return;
                _lightController.UseLight(_reservedLight);
                _reservedLight = 0;
            }
        
        #endregion
        
        #region Animation Methods
        
            /// <summary>
            /// Method <c>OnMeleeAttackAnimationHitFrame</c> is called when the melee attack animation reaches the hit frame.
            /// </summary>
            public void OnMeleeAttackAnimationHitFrame()
            {
                CheckMeleeAttackHits();
            }
        
            /// <summary>
            /// Method <c>OnRangedAttackAnimationFireFrame</c> is called when the ranged attack animation reaches the fire frame.
            /// </summary>
            public void OnRangedAttackAnimationFireFrame()
            {
                // Spawn logic may be triggered by animation event in AnimationEvent mode
                if (rangedSettings.timingMode == AttackTimingMode.AnimationEvent)
                    FireProjectile();
            }

            /// <summary>
            /// Method <c>OnMeleeAttackAnimationEnd</c> is called when the melee attack animation ends.
            /// </summary>
            public void OnMeleeAttackAnimationEnd()
            {
                _stateController.SetState(PlayerState.AttackingMelee, false);
            }

            /// <summary>
            /// Method <c>OnRangedAttackAnimationEnd</c> is called when the ranged attack animation ends.
            /// </summary>
            public void OnRangedAttackAnimationEnd()
            {
                _stateController.SetState(PlayerState.AttackingRanged, false);
            }
        
        #endregion
    }
}
