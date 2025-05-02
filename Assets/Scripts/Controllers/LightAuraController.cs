using UnityEngine;

namespace WAAS
{
    /// <summary>
    /// Class <c>LightAuraController</c> is responsible for controlling the light aura effect on the player.
    /// </summary>
    public class LightAuraController : MonoBehaviour
    {
        /// <value>Property <c>overlayMaterial</c> is the material used for the overlay.</value>
        [SerializeField]
        private Material overlayMaterial;

        /// <value>Property <c>offset</c> is the offset value for the overlay position.</value>
        [SerializeField]
        private float offset = 0.1f;

        /// <value>Property <c>_originalPlayerScreenPos</c> is the original player screen position.</value>
        private Vector4 _originalPlayerScreenPos;

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            if (overlayMaterial == null)
                return;
            _originalPlayerScreenPos = overlayMaterial.GetVector("_PlayerScreenPos");
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (overlayMaterial == null)
                return;
            var viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            viewportPos.y += offset;
            overlayMaterial.SetVector("_PlayerScreenPos", new Vector4(viewportPos.x, viewportPos.y, 0, 0));
        }

        /// <summary>
        /// Method <c>OnDestroy</c> is called when the object is destroyed.
        /// </summary>
        private void OnDestroy() {
            if (overlayMaterial == null)
                return;
            overlayMaterial.SetVector("_PlayerScreenPos", _originalPlayerScreenPos);
        }
    }
}
