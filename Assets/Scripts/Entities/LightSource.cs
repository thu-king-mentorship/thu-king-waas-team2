using UnityEngine;
using WAAS.Managers;
using WAAS.ScriptableObjects;

namespace WAAS.Entities
{
    public class LightSource : MonoBehaviour
    {
        /// <value>Property <c>village</c> represents the village this light source belongs to.</value>
        [SerializeField]
        private VillageData village;

        /// <value>Property <c>sourceLight</c> represents how many light the light source has.</value>
        [SerializeField]
        private int sourceLight = 50;

        /// <value>Property <c>lightChangeAmount</c> represents the amount of light to give or extract.</value>
        [SerializeField]
        private int lightChangeAmount = 10;
        
        /// <value>Property <c>karmaChangeAmount</c> represents the amount of karma to change when giving or extracting light.</value>
        [SerializeField]
        private int karmaChangeAmount = 5;

        /// <value>Property <c>playerInRange</c> represents whether the player is in range of the light source.</value>
        private bool _playerInRange;
        
        /// <value>Property <c>_playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        private PlayerLight _playerLight;

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The Collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            _playerInRange = true;
            _playerLight = other.GetComponent<PlayerLight>();
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
        /// Method <c>OnAttack</c> is called when the player presses the attack button.
        /// </summary>
        public void OnAttack()
        {
            OnGiveLight();
        }
        
        /// <summary>
        /// Method <c>OnInteract</c> is called when the player presses the interact button.
        /// </summary>
        public void OnJump()
        {
            OnExtractLight();
        }

        /// <summary>
        /// Method <c>OnGiveLight</c> is called when the player gives light to the light source.
        /// </summary>
        public void OnGiveLight()
        {
            if (!_playerInRange || _playerLight == null)
                return;
            
            if (_playerLight.CurrentLight < lightChangeAmount)
            {
                DebugLogManager.Instance.Log("Not enough light to give!");
                return;
            }
            _playerLight.UseLight(lightChangeAmount);
            sourceLight += lightChangeAmount;
            ModifyKarma(karmaChangeAmount);
            DebugLogManager.Instance.Log($"Gave light! {village.villageName}'s light source now has {lightChangeAmount}. Player Light: {_playerLight.CurrentLight}");

        }

        /// <summary>
        /// Method <c>OnExtractLight</c> is called when the player extracts light from the light source.
        /// </summary>
        public void OnExtractLight()
        {
            if (!_playerInRange || _playerLight == null)
                return;
            
            if (sourceLight <= lightChangeAmount)
            {
                DebugLogManager.Instance.Log("Not enough light to extract!");
                return;
            }
            if (_playerLight.CurrentLight == _playerLight.MaxLight)
            {
                DebugLogManager.Instance.Log("Player light is full!");
                return;
            }
            _playerLight.RestoreLight(lightChangeAmount);
            sourceLight -= lightChangeAmount;
            ModifyKarma(-karmaChangeAmount);
            DebugLogManager.Instance.Log($"Extracted light! {village.villageName}'s light source now has {lightChangeAmount}. Player Light: {_playerLight.CurrentLight}");
        }

        /// <summary>
        /// Method <c>ModifyKarma</c> modifies the karma of the village this light source belongs to.
        /// </summary>
        /// <param name="change">The amount of karma to change.</param>
        private void ModifyKarma(int change)
        {
            if (village == null)
                return;
            KarmaManager.Instance.ModifyKarma(village, change);
            if (village.nextVillage != null)
                KarmaManager.Instance.ModifyKarma(village.nextVillage, -change / 2);
        }
    }
}
