using UnityEngine;
using UnityEngine.InputSystem;

namespace WAAS
{
    /// <summary>
    /// Class <c>PlayerMovement</c> is a script that allows the player to move the character.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        /// <value>Property <c>moveSpeed</c> represents the speed at which the player moves.</value>
        [SerializeField]
        private float moveSpeed = 5.0f;

        /// <value>Property <c>_moveInput</c> represents the input from the player to move the character.</value>
        private Vector2 _moveInput;
        
        /// <value>Property <c>_movementEnabled</c> represents whether the player can move the character.</value>
        private bool _movementEnabled = true;
        
        /// <value>Property <c>_controller</c> represents the CharacterController component attached to the player GameObject.</value>
        private CharacterController _controller;
        
        /// <value>Property <c>_playerHealth</c> represents the PlayerHealth component attached to the player GameObject.</value>
        private PlayerHealth _playerHealth;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            _playerHealth.OnDeath += HandleDeath;
        }

        /// <summary>
        /// Method <c>HandleDeath</c> is called when the player dies.
        /// </summary>
        private void OnDestroy()
        {
            if (_playerHealth != null)
                _playerHealth.OnDeath -= HandleDeath;
        }

        /// <summary>
        /// Method <c>OnMove</c> is called when the player moves the character.
        /// </summary>
        /// <param name="value">The input value from the player.</param>
        public void OnMove(InputValue value)
        {
            if (!_movementEnabled)
                return;
            _moveInput = value.Get<Vector2>();
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            var isoDirection = new Vector3(_moveInput.x + _moveInput.y, 0, _moveInput.y - _moveInput.x).normalized;
            _controller.Move(isoDirection * (moveSpeed * Time.deltaTime));
        }

        /// <summary>
        /// Method <c>HandleDeath</c> is called when the player dies.
        /// </summary>
        private void HandleDeath()
        {
            _movementEnabled = false;
        }
    }
}
