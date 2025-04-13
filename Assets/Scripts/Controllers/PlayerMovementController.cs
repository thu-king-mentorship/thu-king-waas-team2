using UnityEngine;
using UnityEngine.InputSystem;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerMovement</c> is a script that allows the player to move the character.
    /// </summary>
    public class PlayerMovementController : MonoBehaviour
    {
        #region Movement Properties

            /// <value>Property <c>moveSpeed</c> represents the speed at which the player moves.</value>
            [Header("Movement Properties")]
            [SerializeField]
            private float maxMoveSpeed = 5.0f;
            
            /// <value>Property <c>minMoveSpeed</c> represents the minimum speed at which the player moves.</value>
            [SerializeField]
            private float minMoveSpeed = 1.0f;
            
            /// <value>Property <c>_currentMoveSpeed</c> represents the current speed at which the player moves.</value>
            private float _currentMoveSpeed;

            /// <value>Property <c>_moveInput</c> represents the input from the player to move the character.</value>
            private Vector2 _moveInput;
            
            /// <value>Property <c>_lastMoveDirection</c> represents the last direction the player moved.</value>
            private Vector2 _lastMoveDirection = Vector2.down;
            
            /// <value>Property <c>LastDirection</c> represents the last direction the player moved.</value>
            public Vector2 LastDirection => _lastMoveDirection;
            
            /// <value>Property <c>_movementEnabled</c> represents whether the player can move the character.</value>
            private bool _movementEnabled = true;
        
        #endregion
        
        #region Jump Properties

            /// <value>Property <c>jumpSpeed</c> represents the speed at which the player jumps.</value>
            [Header("Jump Properties")]
            [SerializeField]
            private float jumpForce = 5.0f;
            
            /// <value>Property <c>gravity</c> represents the gravity applied to the player.</value>
            [SerializeField]
            private float gravity = -9.81f;
            
            /// <value>Property <c>_verticalVelocity</c> represents the vertical velocity of the player.</value>
            private float _verticalVelocity;
            
            /// <value>Property <c>_isGrounded</c> represents whether the player is on the ground.</value>
            private bool _isGrounded;
        
        #endregion
        
        #region References
        
            /// <value>Property <c>_controller</c> represents the CharacterController component attached to the player GameObject.</value>
            private CharacterController _controller;
            
            /// <value>Property <c>stateController</c> represents the PlayerStateController component attached to the player GameObject.</value>
            private PlayerStateController _stateController;
        
        #endregion
        
        #region Animation Properties

            /// <value>Property <c>animator</c> represents the Animator component attached to the player GameObject.</value>
            [Header("Animation Properties")]
            [SerializeField]
            private Animator animator;
            
            /// <value>Property <c>MoveX</c> represents the hash of the MoveX parameter in the Animator.</value>
            private static readonly int MoveX = Animator.StringToHash("MoveX");
            
            /// <value>Property <c>MoveY</c> represents the hash of the MoveY parameter in the Animator.</value>
            private static readonly int MoveY = Animator.StringToHash("MoveY");
        
        #endregion

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _stateController = GetComponent<PlayerStateController>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            _currentMoveSpeed = maxMoveSpeed;
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            _isGrounded = _controller.isGrounded;

            if (_isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -2f;
                _stateController.UnsetState(PlayerState.Jumping);
            }

            _verticalVelocity += gravity * Time.deltaTime;

            var isoDirection = new Vector3(_moveInput.x + _moveInput.y, 0, _moveInput.y - _moveInput.x).normalized;
            var moveVector = isoDirection * _currentMoveSpeed;
            moveVector.y = _verticalVelocity;

            if (_stateController.HasState(PlayerState.Attacking))
                moveVector.x = moveVector.z = 0;

            _controller.Move(moveVector * Time.deltaTime);

            if (!_stateController.HasState(PlayerState.Attacking) && _moveInput != Vector2.zero)
                _stateController.SetState(PlayerState.Moving);
            else
                _stateController.UnsetState(PlayerState.Moving);
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
        /// Method <c>OnJump</c> is called when the player jumps.
        /// </summary>
        /// <param name="value">The input value from the player.</param>
        public void OnJump(InputValue value)
        {
            if (!_movementEnabled)
                return;
            if (!_isGrounded)
                return;
            _verticalVelocity = jumpForce;
            _stateController.SetState(PlayerState.Jumping);
        }

        /// <summary>
        /// Method <c>FixedUpdate</c> is called at a fixed interval.
        /// </summary>
        private void FixedUpdate()
        {
            UpdateAnimation();
        }
        
        /// <summary>
        /// Method <c>OnJumpAnimationEnd</c> is called when the jump animation ends.
        /// </summary>
        public void OnJumpAnimationEnd()
        {
            _stateController.UnsetState(PlayerState.Jumping);
        }
        
        /// <summary>
        /// Method <c>EnableMovement</c> enables the player's movement.
        /// </summary>
        /// <param name="movementEnabled">Whether the player can move the character.</param>
        private void EnableMovement(bool movementEnabled)
        {
            _movementEnabled = movementEnabled;
        }

        /// <summary>
        /// Method <c>AdjustSpeedToLight</c> adjusts the player's speed based on their light.
        /// </summary>
        private void AdjustSpeedToLight(int currentLight, int maxLight)
        {
            _currentMoveSpeed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, (float)currentLight / maxLight);
        }
        
        /// <summary>
        /// Method <c>UpdateAnimation</c> updates the player's animation based on their movement.
        /// </summary>
        private void UpdateAnimation()
        {
            if (_stateController.HasState(PlayerState.Attacking))
                return;

            if (_moveInput != Vector2.zero)
            {
                _lastMoveDirection = _moveInput;
                animator.SetFloat(MoveX, _moveInput.x);
                animator.SetFloat(MoveY, _moveInput.y);
            }
            else
            {
                animator.SetFloat(MoveX, _lastMoveDirection.x);
                animator.SetFloat(MoveY, _lastMoveDirection.y);
            }

            if (_stateController.HasState(PlayerState.Jumping))
                animator.Play("Jump");
            else if (_moveInput != Vector2.zero)
                animator.Play("Walk");
            else
                animator.Play("ReadyIdle");
        }
    }
}