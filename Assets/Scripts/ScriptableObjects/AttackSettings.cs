using UnityEngine;

namespace WAAS.ScriptableObjects
{
    /// <summary>
    /// Enum <c>AttackTimingMode</c> defines the different modes of attack timing.
    /// </summary>
    public enum AttackTimingMode
    {
        Continuous,
        Cooldown,
        AnimationEvent
    }

    /// <summary>
    /// Class <c>AttackSettings</c> is a scriptable object that defines the settings for an attack.
    /// </summary>
    public abstract class AttackSettings : ScriptableObject
    {
        /// <value>Property <c>allowAttackWhileMoving</c> indicates whether the player can attack while moving.</value>
        [Header("General Settings")]
        public bool allowAttackWhileMoving;
        
        /// <value>Property <c>lightCost</c> indicates the amount of light consumed by the attack.</value>
        public int lightCost;

        /// <value>Property <c>timingMode</c> indicates the timing mode of the attack.</value>
        [Header("Timing Settings")]
        public AttackTimingMode timingMode = AttackTimingMode.AnimationEvent;
        
        /// <value>Property <c>attackCooldown</c> indicates the cooldown time between attacks.</value>
        public float attackCooldown = 1.0f;
    }
}
