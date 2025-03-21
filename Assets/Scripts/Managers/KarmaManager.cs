using System.Collections.Generic;
using UnityEngine;
using WAAS.ScriptableObjects;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>KarmaManager</c> is a script that manages the karma of the villages.
    /// </summary>
    public class KarmaManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the KarmaManager.</value>
        public static KarmaManager Instance { get; private set; }
        
        /// <value>Property <c>MinKarma</c> represents the minimum karma a village can have.</value>
        private const int MinKarma = -50;
        
        /// <value>Property <c>MaxKarma</c> represents the maximum karma a village can have.</value>
        private const int MaxKarma = 50;

        /// <value>Property <c>_villageKarma</c> represents the karma of the villages.</value>
        private Dictionary<VillageData, int> _villageKarma = new Dictionary<VillageData, int>();

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
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            foreach (var village in VillageManager.Instance.Villages)
            {
                _villageKarma.Add(village, village.startingKarma);
            }
        }

        /// <summary>
        /// Method <c>GetKarma</c> returns the karma of the village.
        /// </summary>
        /// <param name="village">The village.</param>
        /// <returns>The karma of the village with the specified index.</returns>
        public int GetKarma(VillageData village)
        {
            return _villageKarma.GetValueOrDefault(village, 0);
        }

        /// <summary>
        /// Method <c>ModifyKarma</c> modifies the karma of the village by the specified amount.
        /// </summary>
        /// <param name="village">The village.</param>
        /// <param name="amount">The amount to modify the karma by.</param>
        public void ModifyKarma(VillageData village, int amount)
        {
            if (!_villageKarma.ContainsKey(village))
                return;
            _villageKarma[village] = Mathf.Clamp(_villageKarma[village] + amount, MinKarma, MaxKarma);
            Debug.Log($"{village.villageName} karma: {_villageKarma[village]}");
            KarmaUIManager.Instance?.UpdateKarmaUI(village, _villageKarma[village]);
        }
    }
}
