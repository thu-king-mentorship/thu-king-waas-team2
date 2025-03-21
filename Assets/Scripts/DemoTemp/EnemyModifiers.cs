using System;
using UnityEngine;
using WAAS.Entities;
using WAAS.Managers;
using WAAS.ScriptableObjects;

namespace WAAS.DemoTemp
{
    /// <summary>
    /// Method <c>EnemyModifiers</c> is a script that represents the enemy modifiers.
    /// </summary>
    public class EnemyModifiers : MonoBehaviour
    {
        /// <value>Property <c>_lightModifier</c> represents the modifier applied to the enemy stats depending on their light.</value>
        private float _lightModifier;
        
        /// <value>Property <c>_karmaModifier</c> represents the modifier applied to the enemy stats depending on the village's karma.</value>
        private float _karmaModifier;

        /// <value>Property <c>_enemyLight</c> represents the enemy's light.</value>
        private CharacterLight _enemyLight;

        /// <value>Property <c>transformToScale</c> represents the transform to scale.</value>
        [SerializeField]
        private Transform transformToScale;

        /// <value>Property <c>_transformToScaleOriginalY</c> represents the original Y position of the transform to scale.</value>
        private float _transformToScaleOriginalY;
        
        /// <value>Property <c>LightModifier</c> represents an event that is invoked when the light modifier changes.</value>
        public event Action<float> OnLightModifierChanged;
        
        /// <value>Property <c>KarmaModifier</c> represents an event that is invoked when the karma modifier changes.</value>
        public event Action<float> OnKarmaModifierChanged;

        /// <value>Property <c>lightChangeAmount</c> represents the amount of light to give or extract.</value>
        [SerializeField]
        private int lightChangeAmount = 2;

        /// <value>Property <c>playerInRange</c> represents whether the player is in range of the light source.</value>
        private bool _playerInRange;
        
        /// <value>Property <c>_playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        private CharacterLight _playerLight;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _enemyLight = GetComponent<CharacterLight>();
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _enemyLight.OnLightChanged += UpdateLightModifier;
            KarmaManager.Instance.OnKarmaChanged += UpdateKarmaModifier;
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_enemyLight != null)
                _enemyLight.OnLightChanged -= UpdateLightModifier;
            if (KarmaManager.Instance != null)
                KarmaManager.Instance.OnKarmaChanged -= UpdateKarmaModifier;
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The Collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerInRange = true;
            _playerLight = other.GetComponent<CharacterLight>();
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the Collider other has stopped touching
        /// </summary>
        /// <param name="other">The Collider that stopped touching the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerInRange = false;
            _playerLight = null;
        }
        
        /// <summary>
        /// Method <c>OnInteract</c> is called when the player presses the interact button.
        /// </summary>
        public void OnJump()
        {
            OnExtractLight();
        }

        /// <summary>
        /// Method <c>OnExtractLight</c> is called when the player extracts light from the light source.
        /// </summary>
        private void OnExtractLight()
        {
            if (!_playerInRange || _playerLight == null)
                return;
            
            if (_enemyLight.CurrentLight < lightChangeAmount)
            {
                DebugLogManager.Instance.Log("Not enough light to extract!");
                return;
            }
            if (_playerLight.CurrentLight == _playerLight.MaxLight)
            {
                DebugLogManager.Instance.Log("Player light is full!");
                return;
            }
            _enemyLight.UseLight(lightChangeAmount);
            _playerLight.RestoreLight(lightChangeAmount);
        }
        
        /// <summary>
        /// Method <c>OnLightChanged</c> is called when the light of the enemy changes.
        /// </summary>
        /// <param name="currentLight">The current light.</param>
        /// <param name="maxLight">The maximum light.</param>
        private void UpdateLightModifier(int currentLight, int maxLight)
        {
            _lightModifier = 1f - (float)currentLight / maxLight;
            DebugLogManager.Instance.Log($"{name} light modifier: {_lightModifier}");
            OnLightModifierChanged?.Invoke(_lightModifier);
            UpdateScale();
        }
        
        /// <summary>
        /// Method <c>OnKarmaChanged</c> is called when the karma of the village changes.
        /// </summary>
        /// <param name="village">The current village.</param>
        /// <param name="currentKarma">The karma value of the village.</param>
        /// <param name="minKarma">The minimum karma value of the village.</param>
        /// <param name="maxKarma">The maximum karma value of the village.</param>
        private void UpdateKarmaModifier(VillageData village, int currentKarma, int minKarma, int maxKarma)
        {
            if (village != VillageManager.Instance.CurrentVillage)
                return;
            _karmaModifier = -2f * ((currentKarma - KarmaManager.Instance.MinKarma) / (float)(KarmaManager.Instance.MaxKarma - KarmaManager.Instance.MinKarma)) + 1f;
            DebugLogManager.Instance.Log($"{name} karma modifier: {_karmaModifier}");
            OnKarmaModifierChanged?.Invoke(_karmaModifier);
            UpdateScale();
        }

        /// <summary>
        /// Method <c>UpdateScale</c> updates the scale of the enemy.
        /// </summary>
        private void UpdateScale()
        {
            var scale = Mathf.Clamp(1.0f + _lightModifier + _karmaModifier, 0.1f, 2.0f);
            transformToScale.localScale = new Vector3(scale, scale, scale);
            if (_transformToScaleOriginalY == 0)
                _transformToScaleOriginalY = transformToScale.position.y;
            var position = transformToScale.position;
            position.y = _transformToScaleOriginalY + (scale - 1.0f) * 0.5f;
            transformToScale.position = new Vector3(position.x, position.y, position.z);
        }
    }
}
