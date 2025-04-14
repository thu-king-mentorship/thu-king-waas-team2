using UnityEngine;
using UnityEngine.InputSystem;
using WAAS.ScriptableObjects;

namespace WAAS.Controllers
{
    /// <summary>
    /// Class <c>PlayerMovement</c> is a script that allows the player to move the character.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerStateController))]
    [RequireComponent(typeof(PlayerAttackController))]
    [RequireComponent(typeof(PlayerLightController))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Movement Properties

            /// <value>Property <c>movementSettings</c> represents the movement settings for the player.</value>
            [Header("Movement Properties")]
            [SerializeField]
            private MovementSettings movementSettings;
            
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
            
            /// <value>Property <c>_verticalVelocity</c> represents the vertical velocity of the player.</value>
            private float _verticalVelocity;
            
            /// <value>Property <c>_isGrounded</c> represents whether the player is on the ground.</value>
            private bool _isGrounded;
        
        #endregion
        
        #region Animation Properties

            /// <value>Property <c>animator</c> represents the Animator component attached to the player GameObject.</value>
            [Header("Animation Properties")]
            [SerializeField]
            private Animator animator;
            
            /// <value>Property <c>MoveXHash</c> represents the hash of the MoveX parameter in the Animator.</value>
            private static readonly int MoveXHash = Animator.StringToHash("MoveX");
            
            /// <value>Property <c>MoveYHash</c> represents the hash of the MoveY parameter in the Animator.</value>
            private static readonly int MoveYHash = Animator.StringToHash("MoveY");
            
            /// <value>Property <c>JumpHash</c> is the hash value for the jump animation.</value>
            private static readonly int JumpHash = Animator.StringToHash("Jump");
            
            /// <value>Property <c>WalkHash</c> is the hash value for the walk animation.</value>
            private static readonly int WalkHash = Animator.StringToHash("Walk");
            
            /// <value>Property <c>ReadyIdleHash</c> is the hash value for the ready idle animation.</value>
            private static readonly int ReadyIdleHash = Animator.StringToHash("ReadyIdle");
        
        #endregion
        
        #region References
        
            /// <value>Property <c>_controller</c> represents the CharacterController component attached to the player GameObject.</value>
            private CharacterController _controller;
                
            /// <value>Property <c>_stateController</c> represents the PlayerStateController component attached to the player GameObject.</value>
            private PlayerStateController _stateController;
                
            /// <value>Property <c>_attackController</c> represents the PlayerAttackController component attached to the player GameObject.</value>
            private PlayerAttackController _attackController;
            
            /// <value>Property <c>_lightController</c> represents the PlayerLightController component attached to the player GameObject.</value>
            private PlayerLightController _lightController;
        
        #endregion

        #region Unity Events Methods
        
            /// <summary>
            /// Method <c>Awake</c> is called when the script instance is being loaded.
            /// </summary>
            private void Awake()
            {
                _controller = GetComponent<CharacterController>();
                _stateController = GetComponent<PlayerStateController>();
                _attackController = GetComponent<PlayerAttackController>();
                _lightController = GetComponent<PlayerLightController>();
                if (animator == null)
                    animator = GetComponentInChildren<Animator>();
                _currentMoveSpeed = movementSettings.maxMoveSpeed;
            }
            
            /// <summary>
            /// Method <c>Start</c> is called before the first frame update.
            /// </summary>
            private void Start()
            {
                if (_lightController != null)
                    _lightController.OnLightChanged += AdjustSpeedToLight;
            }

            /// <summary>
            /// Method <c>HandleDeath</c> is called when the player dies.
            /// </summary>
            private void OnDestroy()
            {
                if (_lightController != null)
                    _lightController.OnLightChanged -= AdjustSpeedToLight;
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
                    _stateController.SetState(PlayerState.Jumping, false);
                }

                _verticalVelocity += movementSettings.gravity * Time.deltaTime;

                var isoDirection = new Vector3(_moveInput.x + _moveInput.y, 0, _moveInput.y - _moveInput.x).normalized;
                var moveVector = isoDirection * _currentMoveSpeed;
                moveVector.y = _verticalVelocity;

                if (_attackController.ShouldBlockMovement)
                    moveVector.x = moveVector.z = 0;

                _controller.Move(moveVector * Time.deltaTime);

                _stateController.SetState(PlayerState.Moving, !_attackController.ShouldBlockMovement && _moveInput != Vector2.zero);
            }

            /// <summary>
            /// Method <c>FixedUpdate</c> is called at a fixed interval.
            /// </summary>
            private void FixedUpdate()
            {
                UpdateAnimation();
            }
        
        #endregion

        #region Movement Methods

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
                _verticalVelocity = movementSettings.jumpForce;
                _stateController.SetState(PlayerState.Jumping);
            }
        
            /// <summary>
            /// Method <c>EnableMovement</c> enables the player's movement.
            /// </summary>
            /// <param name="movementEnabled">Whether the player can move the character.</param>
            public void EnableMovement(bool movementEnabled)
            {
                _movementEnabled = movementEnabled;
            }

            /// <summary>
            /// Method <c>AdjustSpeedToLight</c> adjusts the player's speed based on their light.
            /// </summary>
            /// <param name="currentLight">The current light level of the player.</param>
            /// <param name="maxLight">The maximum light level of the player.</param>
            private void AdjustSpeedToLight(int currentLight, int maxLight)
            {
                if (!movementSettings.adjustSpeedWithLight)
                    return;
                _currentMoveSpeed = Mathf.Lerp(movementSettings.minMoveSpeed,
                    movementSettings.maxMoveSpeed,
                    (float)currentLight / maxLight);
            }
        
        #endregion
        
        #region Animation Methods
        
            /// <summary>
            /// Method <c>UpdateAnimation</c> updates the player's animation based on their movement.
            /// </summary>
            private void UpdateAnimation()
            {
                if (_moveInput != Vector2.zero)
                {
                    _lastMoveDirection = _moveInput;
                    animator.SetFloat(MoveXHash, _moveInput.x);
                    animator.SetFloat(MoveYHash, _moveInput.y);
                }
                else
                {
                    animator.SetFloat(MoveXHash, _lastMoveDirection.x);
                    animator.SetFloat(MoveYHash, _lastMoveDirection.y);
                }

                if (_stateController.HasState(PlayerState.AttackingMelee)
                    || _stateController.HasState(PlayerState.AttackingRanged))
                    return;

                if (_stateController.HasState(PlayerState.Jumping))
                    animator.Play(JumpHash);
                else if (_moveInput != Vector2.zero)
                    animator.Play(WalkHash);
                else
                    animator.Play(ReadyIdleHash);
            }
            
            /// <summary>
            /// Method <c>OnJumpAnimationEnd</c> is called when the jump animation ends.
            /// </summary>
            public void OnJumpAnimationEnd()
            {
                _stateController.SetState(PlayerState.Jumping, false);
            }
        
        #endregion
    }
}