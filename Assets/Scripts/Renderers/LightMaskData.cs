using UnityEngine;

namespace WAAS.Renderers
{
    /// <summary>
    /// Class <c>LightMaskData</c> is responsible for storing data about light sources in the scene.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class LightMaskData : MonoBehaviour
    {
        /// <value>Property <c>size</c> is the size of the light source.</value>
        [Range(0.0f, 1000.0f)]
        public float size = 200.0f;
        
        /// <value>Property <c>intensity</c> is the intensity of the light source.</value>
        public Color color = Color.white;
        
        /// <value>Property <c>intensity</c> is the intensity of the light source.</value>
        [Range(0.0f, 2.0f)]
        public float intensity = 1.0f;
        
        /// <value>Property <c>offset</c> is the offset for the light source.</value>
        public Vector3 offset = Vector3.zero;

        /// <summary>
        /// Method <c>OnEnable</c> is called when the script instance is being loaded.
        /// </summary>
        private void OnEnable()
        {
            LightMaskRenderer.Register(this);
        }

        /// <summary>
        /// Method <c>OnDisable</c> is called when the script instance is being unloaded.
        /// </summary>
        private void OnDisable()
        {
            LightMaskRenderer.Unregister(this);
        }
    }
}