using TMPro;
using UnityEngine;
using WAAS.Managers;
using WAAS.ScriptableObjects;

namespace WAAS.UI
{
    /// <summary>
    /// Class <c>LevelInfoUI</c> manages the UI elements related to the level information.
    /// </summary>
    public class LevelInfoUI : MonoBehaviour
    {
        /// <value>Property <c>levelNameText</c> represents the text displaying the level name.</value>
        public TextMeshProUGUI levelNameText;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        public void Start()
        {
            GameManager.Instance.OnVillageChanged += UpdateLevelName;
        }
        
        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        public void OnDestroy()
        {
            GameManager.Instance.OnVillageChanged -= UpdateLevelName;
        }
        
        /// <summary>
        /// Method <c>UpdateLevelName</c> updates the level name text.
        /// </summary>
        /// <param name="village">The village.</param>
        private void UpdateLevelName(VillageData village)
        {
            levelNameText.text = village.villageName;
        }
    }
}
