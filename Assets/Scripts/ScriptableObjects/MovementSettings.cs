using UnityEngine;

namespace WAAS.ScriptableObjects
{
    /// <summary>
    /// Class <c>MovementSettings</c> is a scriptable object that defines the movement settings for the player.
    /// </summary>
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "WAAS/Player/Movement Settings")]
    public class MovementSettings : ScriptableObject
    {
        /// <value>Property <c>maxMoveSpeed</c> indicates the maximum movement speed of the player.</value>
        [Header("Speed")]
        public float maxMoveSpeed = 5f;
        
        /// <value>Property <c>minMoveSpeed</c> indicates the minimum movement speed of the player.</value>
        public float minMoveSpeed = 1f;
        
        /// <value>Property <c>adjustSpeedWithLight</c> indicates whether the player's speed is adjusted based on the light level.</value>
        public bool adjustSpeedWithLight = true;

        /// <value>Property <c>jumpForce</c> indicates the force applied when the player jumps.</value>
        [Header("Jump")]
        public float jumpForce = 5f;

        /// <value>Property <c>gravity</c> indicates the gravity applied to the player.</value>
        [Header("Gravity")]
        public float gravity = -9.81f;
    }
}