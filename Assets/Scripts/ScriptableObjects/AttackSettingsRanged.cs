using UnityEngine;

namespace WAAS.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackSettingsRanged", menuName = "WAAS/Attack Settings/Ranged")]
    public class AttackSettingsRanged : AttackSettings
    {
        /// <value>Property <c>projectileSpeed</c> indicates the speed of the projectile.</value>
        [Header("Ranged Settings")]
        public float projectileSpeed = 10.0f;
        
        /// <value>Property <c>projectilePrefab</c> indicates the prefab for the projectile.</value>
        public GameObject projectilePrefab;
    }
}