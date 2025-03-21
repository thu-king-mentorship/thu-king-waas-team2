using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WAAS.Entities;

namespace WAAS.UI
{
    /// <summary>
    /// Class <c>PlayerHealthUI</c> manages the UI elements related to the player's health.
    /// </summary>
    public class PlayerHealthUI : MonoBehaviour
    {
        /// <value>Property <c>healthBarFill</c> represents the fill of the health bar.</value>
        [SerializeField]
        private Image healthBarFill;
        
        /// <value>Property <c>healthText</c> represents the text displaying the player's health.</value>
        [SerializeField]
        private TextMeshProUGUI healthText;
        
        /// <value>Property <c>playerHealth</c> represents the PlayerHealth component attached to the player GameObject.</value>
        [SerializeField]
        private CharacterHealth playerHealth;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (playerHealth != null)
                playerHealth.OnHealthChanged -= UpdateHealthBar;
        }

        /// <summary>
        /// Method <c>UpdateHealthBar</c> updates the health bar fill.
        /// </summary>
        /// <param name="currentHealth">The current health of the player.</param>
        /// <param name="maxHealth">The maximum health of the player.</param>
        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}
