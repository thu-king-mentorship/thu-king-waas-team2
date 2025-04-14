using UnityEngine;

namespace WAAS.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackSettingsMelee", menuName = "WAAS/Attack Settings/Melee")]
    public class AttackSettingsMelee : AttackSettings
    {
        /// <value>Property <c>meleeRange</c> indicates the range of the melee attack.</value>
        [Header("Melee Settings")]
        public float meleeRange = 1.5f;
        
        /// <value>Property <c>meleeHitboxSize</c> indicates the size of the melee hitbox.</value>
        public Vector2 meleeHitboxSize = new Vector2(1.2f, 1.2f);
        
        /// <value>Property <c>meleeDamage</c> indicates the damage dealt by the melee attack.</value>
        public float meleeDamage = 10.0f;
    }
}