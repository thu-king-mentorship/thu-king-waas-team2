using UnityEngine;

namespace WAAS.ScriptableObjects
{
    /// <summary>
    /// Class <c>VillageData</c> is a scriptable object that contains data about the village.
    /// </summary>
    [CreateAssetMenu(fileName = "VillageData", menuName = "Scriptable Objects/VillageData")]
    public class VillageData : ScriptableObject
    {
        /// <value>Property <c>villageName</c> represents the name of the village.</value>
        public string villageName;
        
        /// <value>Property <c>startingKarma</c> represents the starting karma of the village.</value>
        public int startingKarma;

        /// <value>Property <c>nextVillage</c> represents the next village in the sequence.</value>
        public VillageData nextVillage;
    }
}
