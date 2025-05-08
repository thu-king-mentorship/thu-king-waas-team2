using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WAAS.Renderers
{
    /// <summary>
    /// Class <c>LightMaskRenderer</c> is responsible for rendering light sources to a mask texture.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class LightMaskRenderer : MonoBehaviour
    {
        /// <value>Property <c>drawMaterial</c> is the material used for rendering light sources.</value>
        [Header("Light Rendering")]
        [SerializeField]
        private Material drawMaterial;
        
        /// <value>Property <c>fallbackTexture</c> is the texture used when no light sources are present.</value>
        [SerializeField]
        private Texture2D fallbackTexture;

        /// <value>Property <c>lightSources</c> is a list of dynamic light sources.</value>
        [Header("Dynamic Light Sources")]
        private static readonly List<LightMaskData> LightSources = new();

        /// <value>Property <c>_lightMask</c> is the render texture used for the light mask.</value>
        private RenderTexture _lightMask;
        
        /// <value>Property <c>LightMask</c> is the render texture used for the light mask.</value>
        public RenderTexture LightMask => _lightMask;

        /// <value>Property <c>_LightMaskTex</c> is the property ID for the light mask texture.</value>
        private static readonly int LightMaskTex = Shader.PropertyToID("_LightMaskTex");
        
        /// <value>Property <c>syncedMaterials</c> is a list of materials to sync with the light mask.</value>
        [Header("Materials")]
        [SerializeField]
        private List<Material> syncedMaterials;
        
        /// <value>Property <c>_camera</c> is the camera component used for rendering.</value>
        private Camera _camera;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _lightMask = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            _lightMask.Create();
        }

        /// <summary>
        /// Method <c>LateUpdate</c> is called after all Update functions have been called.
        /// </summary>
        private void LateUpdate()
        {
            if (_lightMask == null || drawMaterial == null || _camera == null)
                return;

            var prev = RenderTexture.active;
            RenderTexture.active = _lightMask;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _lightMask.width, _lightMask.height, 0);
            GL.Clear(true, true, Color.black);

            foreach (var lightSource in LightSources)
            {
                if (lightSource == null)
                    continue;

                var worldPos = lightSource.transform.position + lightSource.offset;
                var vp = _camera.WorldToViewportPoint(worldPos);
                if (vp.z < 0)
                    continue;

                var px = vp.x * _lightMask.width - lightSource.size * 0.5f;
                var py = (1 - vp.y) * _lightMask.height - lightSource.size * 0.5f;

                Graphics.DrawTexture(
                    new Rect(px, py, lightSource.size, lightSource.size),
                    fallbackTexture,
                    drawMaterial
                );
            }

            GL.PopMatrix();
            RenderTexture.active = prev;
            
            // Set the light mask texture to the global shader property
            Shader.SetGlobalTexture(LightMaskTex, _lightMask);
            
            // Sync the light mask texture with the materials
            foreach (var material in syncedMaterials.Where(material => material != null))
            {
                material.SetTexture(LightMaskTex, _lightMask);
            }
        }
        
        /// <summary>
        /// Method <c>Register</c> registers a light source to be rendered.
        /// </summary>
        /// <param name="source">The light source to register.</param>
        public static void Register(LightMaskData source)
        {
            if (source != null && !LightSources.Contains(source))
                LightSources.Add(source);
        }

        /// <summary>
        /// Method <c>Unregister</c> unregisters a light source from being rendered.
        /// </summary>
        /// <param name="source">The light source to unregister.</param>
        public static void Unregister(LightMaskData source)
        {
            if (source != null)
                LightSources.Remove(source);
        }
    }
}