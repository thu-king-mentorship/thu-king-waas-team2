using System;
using System.Collections;
using UnityEngine;

namespace WAAS.Entities
{
    /// <summary>
    /// Class <c>CharacterLight</c> is a script that manages the character's light.
    /// </summary>
    public class CharacterLight : MonoBehaviour
    {
        /// <value>Property <c>maxLight</c> represents the maximum light the character can have.</value>
        [SerializeField]
        private int maxLight = 100;
        
        /// <value>Property <c>MaxLight</c> represents the maximum light the character can have.</value>
        public int MaxLight => maxLight;
        
        /// <value>Property <c>_currentLight</c> represents the current light of the character.</value>
        private int _currentLight;

        /// <value>Property <c>CurrentLight</c> represents the current light of the character.</value>
        public int CurrentLight => _currentLight;

        /// <value>Event <c>OnLightChanged</c> is invoked when the character's light changes.</value>
        public event Action<int, int> OnLightChanged;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _currentLight = maxLight;
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
            OnLightChanged?.Invoke(_currentLight, maxLight);
        }

        /// <summary>
        /// Method <c>RestoreLight</c> increases the character's light.
        /// </summary>
        /// <param name="amount">The amount of light to increase.</param>
        public void RestoreLight(int amount)
        {
            _currentLight = Mathf.Clamp(_currentLight + amount, 0, maxLight);
            OnLightChanged?.Invoke(_currentLight, maxLight);
        }

        /// <summary>
        /// Method <c>UseLight</c> reduces the character's light.
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