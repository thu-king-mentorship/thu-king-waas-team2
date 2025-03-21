using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WAAS.Managers;
using WAAS.ScriptableObjects;

namespace WAAS.UI
{
    /// <summary>
    /// Class <c>KarmaInfoUI</c> is a script that manages the karma UI.
    /// </summary>
    public class KarmaInfoUI : MonoBehaviour
    {
        /// <value>Property <c>karmaUIPrefab</c> represents the prefab of the karma UI.</value>
        [SerializeField]
        private GameObject karmaUIPrefab;
        
        /// <value>Property <c>karmaUIContainer</c> represents the container of the karma UI.</value>
        [SerializeField]
        private Transform karmaUIContainer;
        
        /// <value>Property <c>_villageUIMap</c> represents the map of villages to UI elements.</value>
        private readonly Dictionary<VillageData, GameObject> _villageUIMap = new Dictionary<VillageData, GameObject>();
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            KarmaManager.Instance.OnKarmaChanged += UpdateKarmaUI;
            GenerateKarmaUI();
        }
        
        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            KarmaManager.Instance.OnKarmaChanged -= UpdateKarmaUI;
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
        /// <param name="currentKarma">The karma value.</param>
        /// <param name="minKarma">The minimum karma.</param>
        /// <param name="maxKarma">The maximum karma.</param>
        private void UpdateKarmaUI(VillageData village, int currentKarma, int minKarma, int maxKarma)
        {
            if (!_villageUIMap.TryGetValue(village, out var uiEntry))
                return;
            var villageKarmaText = uiEntry.transform.Find("VillageKarma").GetComponent<TextMeshProUGUI>();
            villageKarmaText.text = currentKarma.ToString();
        }
    }
}
