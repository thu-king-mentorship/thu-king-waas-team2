using UnityEngine;
using WAAS.ScriptableObjects;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>LevelManager</c> manages the setup of the level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the LevelManager.</value>
        public static LevelManager Instance { get; private set; }

        /// <value>Property <c>village</c> represents the village represented by the level.</value>
        [SerializeField]
        private VillageData village;
        
        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            GameManager.Instance.SetCurrentVillage(village);
        }
    }
}
