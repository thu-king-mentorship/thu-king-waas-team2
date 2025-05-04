using System.Collections.Generic;
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

        /// <value>Property <c>lightMask</c> is the render texture used for the light mask.</value>
        private RenderTexture lightMask;
        
        /// <value>Property <c>LightMask</c> is the render texture used for the light mask.</value>
        public RenderTexture LightMask => lightMask;
        
        /// <value>Property <c>_camera</c> is the camera component used for rendering.</value>
        private Camera _camera;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _camera = GetComponent<Camera>();

            lightMask = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            lightMask.Create();
        }

        /// <summary>
        /// Method <c>LateUpdate</c> is called after all Update functions have been called.
        /// </summary>
        private void LateUpdate()
        {
            if (lightMask == null || drawMaterial == null || _camera == null)
                return;

            var prev = RenderTexture.active;
            RenderTexture.active = lightMask;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, lightMask.width, lightMask.height, 0);
            GL.Clear(true, true, Color.black);

            foreach (var lightSource in LightSources)
            {
                if (lightSource == null)
                    continue;

                var worldPos = lightSource.transform.position + lightSource.offset;
                var vp = _camera.WorldToViewportPoint(worldPos);
                if (vp.z < 0)
                    continue;

                var px = vp.x * lightMask.width - lightSource.size * 0.5f;
                var py = (1 - vp.y) * lightMask.height - lightSource.size * 0.5f;

                Graphics.DrawTexture(
                    new Rect(px, py, lightSource.size, lightSource.size),
                    fallbackTexture,
                    drawMaterial
                );
            }

            GL.PopMatrix();
            RenderTexture.active = prev;
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