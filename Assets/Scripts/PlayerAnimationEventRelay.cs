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
        /// Method <c>OnJumpAnimationEnd</c> is called when the jump animation ends.
        /// </summary>
        public void OnJumpAnimationEnd() => _movementController?.OnJumpAnimationEnd();
        
        /// <summary>
        /// Method <c>OnMeleeAttackAnimationHitFrame</c> is called when the melee attack animation reaches the hit frame.
        /// </summary>
        public void OnMeleeAttackAnimationHitFrame() => _attackController?.OnMeleeAttackAnimationHitFrame();

        /// <summary>
        /// Method <c>OnMeleeAttackAnimationEnd</c> is called when the melee attack animation ends.
        /// </summary>
        public void OnMeleeAttackAnimationEnd() => _attackController?.OnMeleeAttackAnimationEnd();

        /// <summary>
        /// Method <c>OnRangedAttackAnimationFireFrame</c> is called when the ranged attack animation reaches the fire frame.
        /// </summary>
        public void OnRangedAttackAnimationFireFrame() => _attackController?.OnRangedAttackAnimationFireFrame();
        
        /// <summary>
        /// Method <c>OnRangedAttackAnimationEnd</c> is called when the ranged attack animation ends.
        /// </summary>
        public void OnRangedAttackAnimationEnd() => _attackController?.OnRangedAttackAnimationEnd();
        
    }
}
