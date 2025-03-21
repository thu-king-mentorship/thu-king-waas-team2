using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WAAS.Entities
{
    /// <summary>
    /// Class <c>PlayerMovement</c> is a script that allows the player to move the character.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {

        /// <value>Property <c>moveSpeed</c> represents the speed at which the player moves.</value>
        [SerializeField]
        private float maxMoveSpeed = 5.0f;
        
        /// <value>Property <c>minMoveSpeed</c> represents the minimum speed at which the player moves.</value>
        [SerializeField]
        private float minMoveSpeed = 1.0f;
        
        /// <value>Property <c>_currentMoveSpeed</c> represents the current speed at which the player moves.</value>
        private float _currentMoveSpeed;

        /// <value>Property <c>_moveInput</c> represents the input from the player to move the character.</value>
        private Vector2 _moveInput;
        
        /// <value>Property <c>_movementEnabled</c> represents whether the player can move the character.</value>
        private bool _movementEnabled = true;
        
        /// <value>Property <c>_controller</c> represents the CharacterController component attached to the player GameObject.</value>
        private CharacterController _controller;
        
        /// <value>Property <c>_playerHealth</c> represents the PlayerHealth component attached to the player GameObject.</value>
        private CharacterHealth _playerHealth;
        
        /// <value>Property <c>_playerLight</c> represents the PlayerLight component attached to the player GameObject.</value>
        private CharacterLight _playerLight;

        /// <value>Property <c>animator</c> represents the Animator component attached to the player GameObject.</value>
        [SerializeField]
        private Animator animator;
        
        /// <value>Property <c>MoveX</c> represents the hash of the MoveX parameter in the Animator.</value>
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        
        /// <value>Property <c>MoveY</c> represents the hash of the MoveY parameter in the Animator.</value>
        private static readonly int MoveY = Animator.StringToHash("MoveY");

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _playerHealth = GetComponent<CharacterHealth>();
            _playerLight = GetComponent<CharacterLight>();
            _controller = GetComponent<CharacterController>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            _currentMoveSpeed = maxMoveSpeed;
        }
        
        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _playerHealth.OnDeath += HandleDeath;
            _playerLight.OnLightChanged += AdjustSpeedToLight;
        }

        /// <summary>
        /// Method <c>HandleDeath</c> is called when the player dies.
        /// </summary>
        private void OnDestroy()
        {
            if (_playerHealth != null)
                _playerHealth.OnDeath -= HandleDeath;
            if (_playerLight != null)
                _playerLight.OnLightChanged -= AdjustSpeedToLight;
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
            _controller.Move(isoDirection * (_currentMoveSpeed * Time.deltaTime));
        }

        /// <summary>
        /// Method <c>FixedUpdate</c> is called at a fixed interval.
        /// </summary>
        private void FixedUpdate()
        {
            UpdateAnimation();
        }

        /// <summary>
        /// Method <c>HandleDeath</c> is called when the player dies.
        /// </summary>
        private void HandleDeath()
        {
            _movementEnabled = false;
        }

        /// <summary>
        /// Method <c>AdjustSpeedToLight</c> adjusts the player's speed based on their light.
        /// </summary>
        private void AdjustSpeedToLight(int currentLight, int maxLight)
        {
            _currentMoveSpeed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, (float)currentLight / maxLight);
        }
        
        private void UpdateAnimation()
        {
            if (_moveInput != Vector2.zero)
            {
                animator.SetFloat(MoveX, _moveInput.x);
                animator.SetFloat(MoveY, _moveInput.y);
                animator.Play("Walk");
            }
            else
            {
                animator.Play("ReadyIdle");
            }
        }
    }
}
