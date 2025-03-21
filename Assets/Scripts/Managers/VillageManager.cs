using System;
using System.Collections.Generic;
using UnityEngine;
using WAAS.ScriptableObjects;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>VillageManager</c> is a script that manages the villages.
    /// </summary>
    public class VillageManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the VillageManager.</value>
        public static VillageManager Instance { get; private set; }
        
        /// <value>Property <c>villages</c> represents the list of villages.</value>
        [SerializeField]
        private List<VillageData> villages;

        /// <value>Property <c>Villages</c> represents the list of villages.</value>
        public IReadOnlyList<VillageData> Villages => villages;
        
        /// <value>Property <c>CurrentVillage</c> represents the current village.</value>
        public VillageData CurrentVillage { get; private set; }
        
        public event Action<VillageData> OnVillageChanged;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Method <c>SetCurrentVillage</c> sets the current village.
        /// </summary>
        /// <param name="village">The village.</param>
        public void SetCurrentVillage(VillageData village)
        {
            CurrentVillage = village;
            DebugLogManager.Instance.Log($"Village: {village.villageName}");
            OnVillageChanged?.Invoke(village);
        }
    }
}
