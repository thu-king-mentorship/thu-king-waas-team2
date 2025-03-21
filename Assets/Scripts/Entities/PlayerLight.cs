using System;
using UnityEngine;

namespace WAAS.Entities
{
    /// <summary>
    /// Class <c>PlayerLight</c> is a script that manages the player's light.
    /// </summary>
    public class PlayerLight : MonoBehaviour
    {
        /// <value>Property <c>maxLight</c> represents the maximum light the player can have.</value>
        [SerializeField]
        private int maxLight = 100;
        
        /// <value>Property <c>_currentLight</c> represents the current light of the player.</value>
        private int _currentLight;

        /// <value>Property <c>CurrentLight</c> represents the current light of the player.</value>
        public int CurrentLight => _currentLight;
        
        /// <value>Property <c>MaxLight</c> represents the maximum light the player can have.</value>
        public int MaxLight => maxLight;

        /// <value>Event <c>OnLightChanged</c> is invoked when the player's light changes.</value>
        public event Action<int, int> OnLightChanged;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _currentLight = maxLight;
        }

        /// <summary>
        /// Method <c>RestoreLight</c> increases the player's light.
        /// </summary>
        /// <param name="amount">The amount of light to increase.</param>
        public void RestoreLight(int amount)
        {
            _currentLight = Mathf.Clamp(_currentLight + amount, 0, maxLight);
            OnLightChanged?.Invoke(_currentLight, maxLight);
        }

        /// <summary>
        /// Method <c>UseLight</c> reduces the player's light.
        /// </summary>
        /// <param name="amount">The amount of light to reduce.</param>
        public void UseLight(int amount)
        {
            if (_currentLight <= 0)
                return;

            _currentLight -= amount;
            _currentLight = Mathf.Max(0, _currentLight);

            OnLightChanged?.Invoke(_currentLight, maxLight);
        }
    }
}