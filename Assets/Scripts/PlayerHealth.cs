using System;
using UnityEngine;

namespace WAAS
{
    /// <summary>
    /// Class <c>PlayerHealth</c> is a script that manages the player's health.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        /// <value>Property <c>maxHealth</c> represents the maximum health the player can have.</value>
        [SerializeField]
        private int maxHealth = 100;
        
        /// <value>Property <c>_currentHealth</c> represents the current health of the player.</value>
        private int _currentHealth;
        
        /// <value>Property <c>CurrentHealth</c> represents the current health of the player.</value>
        public int CurrentHealth => _currentHealth;
        
        /// <value>Property <c>MaxHealth</c> represents the maximum health the player can have.</value>
        public int MaxHealth => maxHealth;
        
        /// <value>Event <c>OnHealthChanged</c> is invoked when the player's health changes.</value>
        public event Action<int, int> OnHealthChanged;

        /// <value>Event <c>OnDeath</c> is invoked when the player dies.</value>
        public event Action OnDeath;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        /// <summary>
        /// Method <c>Heal</c> increases the player's health.
        /// </summary>
        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        /// <summary>
        /// Method <c>TakeDamage</c> reduces the player's health.
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
        /// Handles player death.
        /// </summary>
        private void Die()
        {
            OnDeath?.Invoke();
        }
    }
}
