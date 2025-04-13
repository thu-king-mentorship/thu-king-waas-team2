using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WAAS.Controllers;

namespace WAAS.UI
{
    /// <summary>
    /// Class <c>PlayerLightUI</c> manages the UI elements related to the player's light.
    /// </summary>
    public class PlayerLightUI : MonoBehaviour
    {
        /// <value>Property <c>lightBarFill</c> represents the fill of the light bar.</value>
        [SerializeField]
        private Image lightBarFill;
        
        /// <value>Property <c>lightText</c> represents the text displaying the player's light.</value>
        [SerializeField]
        private TextMeshProUGUI lightText;
        
        /// <value>Property <c>playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        [SerializeField]
        private CharacterLight playerLight;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            playerLight.OnLightChanged += UpdateLightBar;
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (playerLight != null)
                playerLight.OnLightChanged -= UpdateLightBar;
        }

        /// <summary>
        /// Method <c>UpdateLightBar</c> updates the light bar fill.
        /// </summary>
        /// <param name="currentLight">The current light of the player.</param>
        /// <param name="maxLight">The maximum light of the player.</param>
        private void UpdateLightBar(int currentLight, int maxLight)
        {
            lightBarFill.fillAmount = (float)currentLight / maxLight;
            lightText.text = $"{currentLight} / {maxLight}";
        }
    }
}