using UnityEngine;

namespace WAAS.Managers
{
    /// <summary>
    /// Method <c>CameraFollowManager</c> is responsible for managing the camera follow behavior.
    /// </summary>
    public class CameraFollowManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> is the singleton instance of the manager.</value>
        public static CameraFollowManager Instance { get; private set; }

        /// <value>Property <c>followProxy</c> is the transform that the camera will follow.</value>
        [SerializeField]
        private Transform followProxy;

        /// <value>Property <c>smoothSpeed</c> is the speed at which the camera will follow the target.</value>
        [SerializeField]
        private float smoothSpeed = 3f;

        /// <value>Property <c>defaultTarget</c> is the default target for the camera to follow.</value>
        [SerializeField]
        private Transform defaultTarget;

        /// <value>Property <c>currentTarget</c> is the current target of the camera.</value>
        private Transform _currentTarget;

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
        /// /// </summary>
        private void Start()
        {
            if (followProxy == null)
                followProxy = transform;

            if (defaultTarget != null)
                _currentTarget = defaultTarget;
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (followProxy != null && _currentTarget != null)
            {
                followProxy.position = Vector3.Lerp(
                    followProxy.position,
                    _currentTarget.position,
                    Time.deltaTime * smoothSpeed
                );
            }
        }

        /// <summary>
        /// Method <c>SetFollowTarget</c> sets the target for the camera to follow.
        /// </summary>
        /// <param name="newTarget">The new target transform.</param>
        public void SetFollowTarget(Transform newTarget)
        {
            _currentTarget = newTarget;
        }

        /// <summary>
        /// Method <c>ResetFollowTarget</c> resets the target to the default target.
        /// </summary>
        public Transform GetCurrentTarget() => _currentTarget;
    }
}