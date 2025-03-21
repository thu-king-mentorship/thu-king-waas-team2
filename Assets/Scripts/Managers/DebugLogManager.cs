using System.Collections;
using UnityEngine;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>DebugLogManager</c> manages the debug log.
    /// </summary>
    public class DebugLogManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the DebugLogManager.</value>
        public static DebugLogManager Instance { get; private set; }
        
        /// <value>Property <c>debugLogContainer</c> represents the container for the debug logs.</value>
        public Transform debugLogContainer;
        
        /// <value>Property <c>debugLogPrefab</c> represents the prefab for the debug logs.</value>
        public GameObject debugLogPrefab;
        
        /// <value>Property <c>logDuration</c> represents the duration of the log.</value>
        [SerializeField]
        private float logDuration = 5f;
        
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
        /// Method <c>Log</c> logs a message to the debug log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            var log = Instantiate(debugLogPrefab, debugLogContainer);
            var logText = log.GetComponent<TMPro.TextMeshProUGUI>();
            logText.text = message;
            StartCoroutine(DestroyLog(log));
        }

        /// <summary>
        /// Coroutine <c>DestroyLog</c> destroys the log after a certain duration.
        /// </summary>
        /// <param name="log">The log to destroy.</param>
        private IEnumerator DestroyLog(GameObject log)
        {
            yield return new WaitForSeconds(logDuration);
            if (log != null)
                Destroy(log);
        }
    }
}
