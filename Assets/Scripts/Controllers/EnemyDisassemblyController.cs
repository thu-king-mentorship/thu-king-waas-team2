using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>EnemyDisassemblyController</c> is a script that controls the disassembly and reassembly of enemy parts.
    /// </summary>
    [DisallowMultipleComponent]
    public class EnemyDisassemblyController : MonoBehaviour
    {
        /// <value>Property <c>bodyParts</c> is a list of transforms that represent the body parts of the enemy.</value>
        [Header("Body Parts")]
        [SerializeField]
        private List<Transform> bodyParts = new();

        /// <value>Property <c>disassemblyImpulse</c> is the impulse applied to the body parts during disassembly.</value>
        [Header("Disassembly Settings")]
        [SerializeField]
        private float disassemblyImpulse = 5.0f;
        
        /// <value>Property <c>reassemblyDelay</c> is the delay before the body parts are reassembled.</value>
        [Header("Reassembly Settings")]
        [SerializeField]
        private float reassemblyDelay = 3.0f;
        
        /// <value>Property <c>reassemblyDuration</c> is the duration of the reassembly animation.</value>
        [SerializeField]
        private float reassemblyDuration = 1.5f;
        
        /// <value>Property <c>_isAnimating</c> is a boolean that indicates whether the enemy is currently animating.</value>
        private bool _isAnimating;

        /// <value>Property <c>originalLocalPositions</c> is a dictionary that stores the original local positions of the body parts.</value>
        private readonly Dictionary<Transform, Vector3> originalLocalPositions = new();
        
        /// <value>Property <c>originalLocalRotations</c> is a dictionary that stores the original local rotations of the body parts.</value>
        private readonly Dictionary<Transform, Quaternion> originalLocalRotations = new();

        /// <value>Property <c>TWEEN_ID</c> is the ID used for the DOTween animations.</value>
        private const string TweenID = "EnemyDisassembly";

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            foreach (var part in bodyParts)
            {
                // Store the original local position and rotation of the part
                originalLocalPositions[part] = part.localPosition;
                originalLocalRotations[part] = part.localRotation;
                
                // Ensure Rigidbody is set to kinematic
                var rb = part.GetComponent<Rigidbody>();
                if (rb == null)
                    continue;
                rb.isKinematic = true;
                
                // Ensure Collider is set to trigger
                var col = part.GetComponent<Collider>();
                if (col != null)
                {
                    col.isTrigger = true;
                }
            }
        }

        /// <summary>
        /// Method <c>OnHit</c> is called when the enemy is hit.
        /// </summary>
        /// <param name="projectileDirection">The direction of the projectile.</param>
        public void OnHit(Vector3 projectileDirection)
        {
            // Check if the enemy is already animating
            if (_isAnimating)
                return;
            StartCoroutine(ExplodeAndRebuild(projectileDirection));
        }

        /// <summary>
        /// Coroutine <c>ExplodeAndRebuild</c> handles the explosion and rebuilding of the enemy parts.
        /// </summary>
        /// <param name="direction">The direction of the explosion.</param>
        /// <returns>An IEnumerator that handles the explosion and rebuilding process.</returns>
        private IEnumerator ExplodeAndRebuild(Vector3 direction)
        {
            // Start the animation
            _isAnimating = true;

            // Disassemble
            foreach (var part in bodyParts)
            {
                part.parent = null;

                var rb = part.GetComponent<Rigidbody>();
                if (rb == null)
                    continue;
                rb.isKinematic = false;
                
                var col = part.GetComponent<Collider>();
                if (col != null)
                    col.isTrigger = false;

                var randomized = direction + Random.insideUnitSphere * 0.2f;
                randomized.y = Mathf.Abs(randomized.y); // ensure upward push
                rb.AddForce(randomized.normalized * disassemblyImpulse, ForceMode.Impulse);
            }
            
            yield return new WaitForSeconds(reassemblyDelay);

            // Reassemble
            foreach (var part in bodyParts)
            {
                var rb = part.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                var col = part.GetComponent<Collider>();
                if (col != null)
                    col.isTrigger = true;

                DOTween.Kill(part);
                part.DOMove(transform.TransformPoint(originalLocalPositions[part]), reassemblyDuration)
                    .SetEase(Ease.InOutQuad)
                    .SetId(TweenID);
                part.DORotateQuaternion(originalLocalRotations[part], reassemblyDuration)
                    .SetEase(Ease.InOutQuad)
                    .SetId(TweenID);
            }

            yield return new WaitForSeconds(reassemblyDuration);

            // Recreate children and reset their positions
            foreach (var part in bodyParts)
            {
                part.SetParent(transform);
                part.localPosition = originalLocalPositions[part];
                part.localRotation = originalLocalRotations[part];
            }
            
            // Reset the animation state
            _isAnimating = false;
        }
    }
}