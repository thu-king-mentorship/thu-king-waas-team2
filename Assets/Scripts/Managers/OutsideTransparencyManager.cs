using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WAAS.Managers
{
    /// <summary>
    /// Class <c>OutsideTransparencyManager</c> is responsible for managing the transparency of objects tagged as "Outside".
    /// </summary>
    public class OutsideTransparencyManager : MonoBehaviour
    {

        /// <value>Property <c>Instance</c> is the singleton instance of the OutsideTransparencyManager.</value>
        public static OutsideTransparencyManager Instance { get; private set; }

        /// <value>Property <c>outsideLayer</c> is the layer mask for the outside objects.</value>
        [SerializeField] private LayerMask outsideLayer;

        /// <value>Property <c>_outsideRenderers</c> is a list of MeshRenderers that are on the outside layer.</value>
        private readonly List<MeshRenderer> _outsideRenderers = new();

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
            CacheAllOutsideRenderers();
        }

        /// <summary>
        /// Method <c>CacheAllOutsideRenderers</c> finds all objects in the scene that are on the outside layer.
        /// </summary>
        private void CacheAllOutsideRenderers()
        {
            _outsideRenderers.Clear();
            var allRenderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
            foreach (var r in allRenderers)
            {
                if (((1 << r.gameObject.layer) & outsideLayer) != 0)
                    _outsideRenderers.Add(r);
            }
        }

        /// <summary>
        /// Method <c>HideOutside</c> hides all outside objects by setting their active state to false.
        /// </summary>
        public void HideOutside()
        {
            foreach (var r in _outsideRenderers.Where(r => r != null))
            {
                r.enabled = false;
            }
        }

        /// <summary>
        /// Method <c>ShowOutside</c> shows all outside objects by setting their active state to true.
        /// </summary>
        public void ShowOutside()
        {
            foreach (var r in _outsideRenderers.Where(r => r != null))
            {
                r.enabled = true;
            }
        }
    }
}