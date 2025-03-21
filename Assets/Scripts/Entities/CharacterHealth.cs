using System;
using System.Collections;
using UnityEngine;

namespace WAAS.Entities
{
    /// <summary>
    /// Class <c>CharacterHealth</c> is a script that manages the character's health.
    /// </summary>
    public class CharacterHealth : MonoBehaviour
    {
        /// <value>Property <c>maxHealth</c> represents the maximum health the character can have.</value>
        [SerializeField]
        private int maxHealth = 100;
        
        /// <value>Property <c>MaxHealth</c> represents the maximum health the character can have.</value>
        public int MaxHealth => maxHealth;
        
        /// <value>Property <c>_currentHealth</c> represents the current health of the character.</value>
        private int _currentHealth;
        
        /// <value>Property <c>CurrentHealth</c> represents the current health of the character.</value>
        public int CurrentHealth => _currentHealth;
        
        /// <value>Event <c>OnHealthChanged</c> is invoked when the character's health changes.</value>
        public event Action<int, int> OnHealthChanged;

        /// <value>Event <c>OnDeath</c> is invoked when the character dies.</value>
        public event Action OnDeath;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            StartCoroutine(ImmediatelyAfterStart());
        }
        
        /// <summary>
        /// Method <c>ImmediatelyAfterStart</c> is called in the frame immediately after the first frame.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ImmediatelyAfterStart()
        {
            yield return null;
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        /// <summary>
        /// Method <c>Heal</c> increases the character's health.
        /// </summary>
        public void Heal(int amount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        /// <summary>
        /// Method <c>TakeDamage</c> reduces the character's health.
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (_currentHealth <= 0)
                return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(0, _currentHealth);

            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            if (_currentHealth == 0)
                Die();
        }

        /// <summary>
        /// Handles character death.
        /// </summary>
        private void Die()
        {
            OnDeath?.Invoke();
        }
    }
}
