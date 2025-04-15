using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>LightPathController</c> is responsible for drawing a path using a LineRenderer component.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class LightPathController : MonoBehaviour
    {
        /// <value>Property <c>pathContainer</c> represents the container that holds the path points.</value>
        [Header("Path Settings")]
        [SerializeField]
        private Transform pathContainer;
        
        /// <value>Property <c>drawSpeed</c> represents the speed at which the path is drawn.</value>
        [SerializeField]
        private float drawSpeed = 5;

        /// <value>Property <c>_pathPoints</c> represents the list of points that define the path to be drawn.</value>
        private List<Transform> _pathPoints = new();

        /// <value>Property <c>_lineRenderer</c> represents the LineRenderer component used to draw the path.</value>
        private LineRenderer _lineRenderer;

        /// <value>Property <c>_isDrawing</c> indicates whether the path is currently being drawn.</value>
        private bool _isDrawing;

        /// <value>Property <c>_segmentLengths</c> stores the lengths of each segment of the path.</value>
        private List<float> _segmentLengths = new();

        /// <value>Property <c>_totalLength</c> represents the total length of the path.</value>
        private float _totalLength;

        /// <summary>
        /// Method <c>Awake</c> initializes the LineRenderer component and sets its initial state.
        /// </summary>
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false;
            if (pathContainer == null)
                return;
            _pathPoints.Clear();
            foreach (Transform child in pathContainer)
                _pathPoints.Add(child);
        }

        /// <summary>
        /// Method <c>ActivatePath</c> starts drawing the path if it is not already being drawn and if there are enough points.
        /// </summary>
        public void ActivatePath()
        {
            if (_isDrawing || _pathPoints.Count < 2)
                return;
            PrecomputeLengths();
            StartCoroutine(DrawContinuousPath());
        }

        /// <summary>
        /// Method <c>PrecomputeLengths</c> calculates the lengths of each segment of the path and the total length.
        /// </summary>
        private void PrecomputeLengths()
        {
            _segmentLengths.Clear();
            _totalLength = 0f;

            for (var i = 0; i < _pathPoints.Count - 1; i++)
            {
                var dist = Vector3.Distance(_pathPoints[i].position, _pathPoints[i + 1].position);
                _segmentLengths.Add(dist);
                _totalLength += dist;
            }
        }

        /// <summary>
        /// Coroutine <c>DrawContinuousPath</c> is responsible for drawing the path over time.
        /// </summary>
        /// <returns>An IEnumerator that draws the path continuously.</returns>
        private IEnumerator DrawContinuousPath()
        {
            _isDrawing = true;
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, _pathPoints[0].position);

            var distanceDrawn = 0f;
            var currentSegment = 0;
            var currentStart = _pathPoints[0].position;
            var currentEnd = _pathPoints[1].position;
            var segmentLength = _segmentLengths[0];

            while (distanceDrawn < _totalLength)
            {
                distanceDrawn += drawSpeed * Time.deltaTime;

                while (distanceDrawn > segmentLength && currentSegment < _segmentLengths.Count - 1)
                {
                    distanceDrawn -= segmentLength;
                    currentSegment++;
                    currentStart = _pathPoints[currentSegment].position;
                    currentEnd = _pathPoints[currentSegment + 1].position;
                    segmentLength = _segmentLengths[currentSegment];

                    _lineRenderer.positionCount = currentSegment + 2;
                    _lineRenderer.SetPosition(currentSegment + 1, currentStart);
                }

                var t = Mathf.Clamp01(distanceDrawn / segmentLength);
                var interpolated = Vector3.Lerp(currentStart, currentEnd, t);

                _lineRenderer.positionCount = currentSegment + 2;
                _lineRenderer.SetPosition(currentSegment + 1, interpolated);

                yield return null;
            }

            // Snap to final position
            _lineRenderer.positionCount = _pathPoints.Count;
            for (var i = 0; i < _pathPoints.Count; i++)
                _lineRenderer.SetPosition(i, _pathPoints[i].position);

            _isDrawing = false;
        }
    }
}