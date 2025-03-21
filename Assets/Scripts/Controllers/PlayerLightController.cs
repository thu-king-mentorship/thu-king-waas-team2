using UnityEngine;
using WAAS.Managers;
using WAAS.ScriptableObjects;

namespace WAAS.Controllers
{
    /// <summary>
    /// Adjusts the player's light range based on their karma in the current village.
    /// </summary>
    public class PlayerLightController : MonoBehaviour
    {
        /// <value>Property <c>playerLight</c> represents the player's light.</value>
        [SerializeField]
        private Light playerLight;

        /// <value>Property <c>minLightRange</c> represents the minimum light range.</value>
        [SerializeField]
        private float minLightRange = 4f;
        
        /// <value>Property <c>maxLightRange</c> represents the maximum light range.</value>
        [SerializeField]
        private float maxLightRange = 10f;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            if (playerLight == null)
                playerLight = GetComponentInChildren<Light>();
            KarmaManager.Instance.OnKarmaChanged += UpdateLightRange;
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            KarmaManager.Instance.OnKarmaChanged -= UpdateLightRange;
        }

        /// <summary>
        /// Method <c>UpdateLightRange</c> updates the player's light range based on their karma in the current village.
        /// </summary>
        /// <param name="village">The current village.</param>
        /// <param name="currentKarma">The karma value of the village.</param>
        /// <param name="minKarma">The minimum karma value of the village.</param>
        /// <param name="maxKarma">The maximum karma value of the village.</param>
        private void UpdateLightRange(VillageData village, int currentKarma, int minKarma, int maxKarma)
        {
            if (village != VillageManager.Instance.CurrentVillage)
                return;
            var t = (float)(currentKarma - minKarma) / (maxKarma - minKarma);
            playerLight.range = Mathf.Lerp(minLightRange, maxLightRange, t);
            DebugLogManager.Instance.Log($"Light range: {playerLight.range}");
        }
    }
}