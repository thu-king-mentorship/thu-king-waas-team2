using UnityEngine;
using UnityEngine.UI;
using WAAS.Renderers;

namespace WAAS
{
    /// <summary>
    /// Method <c>DebugLightMaskViewer</c> is responsible for displaying the light mask in the UI.
    /// </summary>
    public class DebugLightMaskViewer : MonoBehaviour
    {
        /// <value>Property <c>targetImage</c> is the UI image used to display the light mask.</value>
        [SerializeField]
        private RawImage targetImage;

        /// <value>Property <c>lightMaskRenderer</c> is a reference to the light mask renderer.</value>
        [SerializeField]
        private LightMaskRenderer lightMaskRenderer;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            if (targetImage != null && lightMaskRenderer != null)
            {
                targetImage.texture = lightMaskRenderer.LightMask;
            }
        }
    }
}