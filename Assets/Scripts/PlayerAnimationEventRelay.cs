using UnityEngine;
using WAAS.Controllers;

namespace WAAS
{
    /// <summary>
    /// Method <c>PlayerAnimationEventRelay</c> is a script that relays animation events to the player.
    /// </summary>
    public class AnimationEventRelay : MonoBehaviour
    {
        /// <value>Property <c>_attackController</c> represents the PlayerAttackController component attached to the player GameObject.</value>
        private PlayerAttackController _attackController;
        
        /// <value>Property <c>_movementController</c> represents the PlayerMovementController component attached to the player GameObject.</value>
        private PlayerMovementController _movementController;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _attackController = GetComponentInParent<PlayerAttackController>();
            _movementController = GetComponentInParent<PlayerMovementController>();
        }

        /// <summary>
        /// Method <c>OnAttackAnimationEnd</c> is called when the attack animation ends.
        /// </summary>
        public void OnAttackAnimationEnd() => _attackController?.OnAttackAnimationEnd();
        
        /// <summary>
        /// Method <c>OnJumpAnimationEnd</c> is called when the jump animation ends.
        /// </summary>
        public void OnJumpAnimationEnd() => _movementController?.OnJumpAnimationEnd();
        
        /// <summary>
        /// Method <c>OnRangedAttackAnimationEnd</c> is called when the ranged attack animation ends.
        /// </summary>
        public void OnRangedAttackAnimationEnd() => _attackController?.OnRangedAttackAnimationEnd();
        
        /// <summary>
        /// Method <c>OnJumpAnimationStart</c> is called when the jump animation starts.
        /// </summary>
        public void FireProjectile() => _attackController?.FireProjectile();
    }
}
