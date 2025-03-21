using System.Globalization;
using UnityEngine;
using TMPro;
using WAAS.Entities;

namespace WAAS.DemoTemp
{
    /// <summary>
    /// Class <c>EnemyUI</c> is a script that manages the enemy UI.
    /// </summary>
    public class EnemyUI : MonoBehaviour
    {
        /// <value>Property <c>healthText</c> represents the text displaying the health.</value>
        [SerializeField]
        private TextMeshPro healthText;

        /// <value>Property <c>lightText</c> represents the text displaying the light.</value>
        [SerializeField]
        private TextMeshPro lightText;
        
        /// <value>Property <c>lightModifierText</c> represents the text displaying the light modifier.</value>
        [SerializeField]
        private TextMeshPro lightModifierText;
        
        /// <value>Property <c>karmaModifierText</c> represents the text displaying the karma modifier.</value>
        [SerializeField]
        private TextMeshPro karmaModifierText;

        /// <value>Property <c>characterHealth</c> represents the CharacterHealth component attached to the enemy GameObject.</value>
        private CharacterHealth _characterHealth;
        
        /// <value>Property <c>characterLight</c> represents the CharacterLight component attached to the enemy GameObject.</value>
        private CharacterLight _characterLight;
        
        /// <value>Property <c>enemyModifiers</c> represents the EnemyModifiers component attached to the enemy GameObject.</value>
        private EnemyModifiers _enemyModifiers;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _characterHealth = GetComponent<CharacterHealth>();
            _characterLight = GetComponent<CharacterLight>();
            _enemyModifiers = GetComponent<EnemyModifiers>();
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _characterHealth.OnHealthChanged += UpdateHealthText;
            _characterLight.OnLightChanged += UpdateLightText;
            _enemyModifiers.OnLightModifierChanged += UpdateLightModifierText;
            _enemyModifiers.OnKarmaModifierChanged += UpdateKarmaModifierText;
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_characterHealth != null)
                _characterHealth.OnHealthChanged -= UpdateHealthText;
            if (_characterLight != null)
                _characterLight.OnLightChanged -= UpdateLightText;
            if (_enemyModifiers == null)
                return;
            _enemyModifiers.OnLightModifierChanged -= UpdateLightModifierText;
            _enemyModifiers.OnKarmaModifierChanged -= UpdateKarmaModifierText;
        }
        
        /// <summary>
        /// Method <c>UpdateHealthText</c> updates the health text.
        /// </summary>
        /// <param name="currentHealth">The current health of the enemy.</param>
        /// <param name="maxHealth">The maximum health of the enemy.</param>
        private void UpdateHealthText(int currentHealth, int maxHealth)
        {
            healthText.text = currentHealth.ToString();
        }
        
        /// <summary>
        /// Method <c>UpdateLightText</c> updates the light text.
        /// </summary>
        /// <param name="currentLight">The current light of the enemy.</param>
        /// <param name="maxLight">The maximum light of the enemy.</param>
        private void UpdateLightText(int currentLight, int maxLight)
        {
            lightText.text = currentLight.ToString();
        }
        
        /// <summary>
        /// Method <c>UpdateLightModifierText</c> updates the light modifier text.
        /// </summary>
        /// <param name="lightModifier">The light modifier of the enemy.</param>
        private void UpdateLightModifierText(float lightModifier)
        {
            lightModifierText.text = lightModifier.ToString(CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Method <c>UpdateKarmaModifierText</c> updates the karma modifier text.
        /// </summary>
        /// <param name="karmaModifier">The karma modifier of the enemy.</param>
        private void UpdateKarmaModifierText(float karmaModifier)
        {
            karmaModifierText.text = karmaModifier.ToString(CultureInfo.InvariantCulture);
        }
    }
}
