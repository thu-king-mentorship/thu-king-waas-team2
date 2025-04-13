using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Method <c>BillboardController</c> is a script that controls the billboard.
    /// </summary>
    public class BillboardController : MonoBehaviour
    {
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
