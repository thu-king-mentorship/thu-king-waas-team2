using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WAAS.ScriptableObjects;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>KarmaUIManager</c> is a script that manages the karma UI.
    /// </summary>
    public class KarmaUIManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the KarmaUIManager.</value>
        public static KarmaUIManager Instance { get; private set; }
        
        /// <value>Property <c>karmaUIPrefab</c> represents the prefab of the karma UI.</value>
        [SerializeField]
        private GameObject karmaUIPrefab;
        
        /// <value>Property <c>karmaUIContainer</c> represents the container of the karma UI.</value>
        [SerializeField]
        private Transform karmaUIContainer;
        
        /// <value>Property <c>_villageUIMap</c> represents the map of villages to UI elements.</value>
        private readonly Dictionary<VillageData, GameObject> _villageUIMap = new Dictionary<VillageData, GameObject>();

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
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            GenerateKarmaUI();
        }
        
        /// <summary>
        /// Method <c>GenerateKarmaUI</c> generates the karma UI.
        /// </summary>
        private void GenerateKarmaUI()
        {
            foreach (var village in VillageManager.Instance.Villages)
            {
                var uiEntry = Instantiate(karmaUIPrefab, karmaUIContainer);
                _villageUIMap.Add(village, uiEntry);
                var villageNameText = uiEntry.transform.Find("VillageName").GetComponent<TextMeshProUGUI>();
                var villageKarmaText = uiEntry.transform.Find("VillageKarma").GetComponent<TextMeshProUGUI>();
                villageNameText.text = village.villageName;
                villageKarmaText.text = village.startingKarma.ToString();
            }
        }

        /// <summary>
        /// Method <c>UpdateKarmaUI</c> updates the karma UI of the village with the specified index.
        /// </summary>
        /// <param name="village">The village.</param>
        /// <param name="karmaValue">The karma value.</param>
        public void UpdateKarmaUI(VillageData village, int karmaValue)
        {
            if (!_villageUIMap.TryGetValue(village, out var uiEntry))
                return;
            var villageKarmaText = uiEntry.transform.Find("VillageKarma").GetComponent<TextMeshProUGUI>();
            villageKarmaText.text = karmaValue.ToString();
        }
    }
}
