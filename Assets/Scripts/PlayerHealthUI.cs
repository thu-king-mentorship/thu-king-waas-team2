using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WAAS
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
        
        /// <value>Property <c>_playerHealth</c> represents the PlayerHealth component attached to the player GameObject.</value>
        private PlayerHealth _playerHealth;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            if (_playerHealth != null)
                _playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_playerHealth == null)
                return;
            _playerHealth.OnHealthChanged -= UpdateHealthBar;
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
