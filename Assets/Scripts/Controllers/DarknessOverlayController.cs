using UnityEngine;
using UnityEngine.UI;
using WAAS.Renderers;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>DarknessOverlayController</c> is responsible for controlling the darkness overlay effect.
    /// </summary>
    public class DarknessOverlayController : MonoBehaviour
    {
        /// <value>Property <c>lightMaskRenderer</c> is a reference to the light mask renderer.</value>
        [Header("Light System References")]
        [SerializeField]
        private LightMaskRenderer lightMaskRenderer;
        
        /// <value>Property <c>playerTransform</c> is a reference to the player's transform.</value>
        [SerializeField]
        private Transform playerTransform;

        /// <value>Property <c>offset</c> is the offset for the player aura.</value>
        [Header("Player aura offset (optional)")]
        [SerializeField]
        private float offset = 0.1f;

        /// <value>Property <c>_material</c> is the material used for the darkness overlay.</value>
        private Material _material;

        /// <value>Property <c>_originalPlayerScreenPos</c> is the original player screen position.</value>
        private static readonly int LightMaskTex = Shader.PropertyToID("_LightMaskTex");
        
        /// <value>Property <c>_originalPlayerScreenPos</c> is the original player screen position.</value>
        private static readonly int PlayerScreenPos = Shader.PropertyToID("_PlayerScreenPos");

        /// <value>Property <c>_originalPlayerScreenPos</c> is the original player screen position.</value>
        private Vector4 _originalPlayerScreenPos;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _material = GetComponent<Image>().material;
            if (_material != null)
                _originalPlayerScreenPos = _material.GetVector(PlayerScreenPos);
        }

        /// <summary>
        /// Method <c>LateUpdate</c> is called every frame, after all Update functions have been called.
        /// </summary>
        private void LateUpdate()
        {
            if (_material == null || lightMaskRenderer == null || playerTransform == null)
                return;

            // Update light mask
            _material.SetTexture(LightMaskTex, lightMaskRenderer.LightMask);

            // Update player screen position
            var viewportPos = Camera.main.WorldToViewportPoint(playerTransform.position);
            viewportPos.y += offset;
            _material.SetVector(PlayerScreenPos, new Vector4(viewportPos.x, viewportPos.y, 0, 0));
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_material != null)
                _material.SetVector(PlayerScreenPos, _originalPlayerScreenPos);
        }
    }
}