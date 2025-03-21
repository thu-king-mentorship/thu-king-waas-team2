using System;
using UnityEngine;
using WAAS.ScriptableObjects;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>GameManager</c> is a script that manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the GameManager.</value>
        public static GameManager Instance { get; private set; }
        
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
            OnVillageChanged?.Invoke(village);
            DebugLogManager.Instance.Log($"Current Village set to: {village.villageName}");
        }
    }
}
