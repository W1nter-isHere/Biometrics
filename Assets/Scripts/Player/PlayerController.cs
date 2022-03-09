using System;
using System.Collections;
using System.IO;
using Channels;
using Core;
using Events;
using Managers;
using SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : Singleton<PlayerController>, ISaveable
    {
        [SerializeField] private InputChannel inputChannel;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform ceilingChecker;
        [SerializeField] private Transform frontChecker;

        private float _movementSpeed;
        private float _dashPower;
        private float _dashDuration;
        private float _maxDashCooldown;
        private float _jumpPower;
        
        private float _direction;
        private float _dashCooldown;
        private ushort _jumpCount;
        private ushort _wallJumpCount;
        private bool _walking;
        private bool _jumped;
        private bool _onGround;

        private GameObject _dash;

        public bool IsBehaviourPlaying;
        public GameState GameState { get; set; } = GameState.Paused;
        public bool OnGround => _onGround;
        private bool _touchingWall;
        public bool TouchingWall => _touchingWall;
        private bool _facingRight = true;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private float _drag = 1;
        private readonly Collider2D[] _groundColliders = new Collider2D[2];
        private readonly Collider2D[] _ceilingColliders = new Collider2D[2];
        private readonly Collider2D[] _frontColliders = new Collider2D[4];
        private GameObject _adjacentWall;
        private GameObject _lastJumpedWall;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Jumped = Animator.StringToHash("Jumped");
        public static readonly int Progress1 = Shader.PropertyToID("_Progress");
        public static readonly int Enabled = Shader.PropertyToID("_Enabled");

        protected override void Awake()
        {
            base.Awake();
            GameState = GameState.Paused;
        }

        private void Start()
        {
            if (playerStats == null) throw new Exception("Player Stats can not be null!");
            GetComponent<SpriteRenderer>().material.SetFloat(Progress1, 1);
            GetComponent<SpriteRenderer>().material.SetInt(Enabled, 1);

            StartCoroutine(SpawnIn());
            
            _movementSpeed = playerStats.movementSpeed;
            _dashPower = playerStats.dashPower;
            _dashDuration = playerStats.dashDuration;
            _maxDashCooldown = playerStats.maxDashCooldown;
            _jumpPower = playerStats.jumpPower;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _dash = GameObject.Find("DashCooldownHudText");
            
            IEnumerator SpawnIn()
            {
                LeanTween.value(gameObject, f => GetComponent<SpriteRenderer>().material.SetFloat(Progress1, f), 1, 2, 1f);
                yield return new WaitForSeconds(1f);
                GetComponent<SpriteRenderer>().material.SetInt(Enabled, 0);
                GameState = GameState.OnGoing;
            }
        }

        private void OnEnable()
        {
            inputChannel.OnMovePerformed += MovePerformed;
            inputChannel.OnMoveCancelled += MoveCancelled;
            inputChannel.OnFallPerformed += Fall;
            inputChannel.OnFallCancelled += FallCancel;
            inputChannel.OnDashPressed += Dash;
            inputChannel.OnJumpPressed += Jump;
            
            inputChannel.preMovePerformed.AddListener(CancelMovement);
            inputChannel.preAttackPressed.AddListener(CancelMovement);
            inputChannel.preDashPerformed.AddListener(CancelMovement);
            inputChannel.preJumpPressed.AddListener(CancelMovement);
            inputChannel.preSecondaryAttackPressed.AddListener(CancelMovement);
            inputChannel.preFallPerformed.AddListener(CancelMovement);
        }

        private void OnDisable()
        {
            inputChannel.OnMovePerformed -= MovePerformed;
            inputChannel.OnMoveCancelled -= MoveCancelled;
            inputChannel.OnFallPerformed -= Fall;
            inputChannel.OnFallCancelled -= FallCancel;
            inputChannel.OnDashPressed -= Dash;
            inputChannel.OnJumpPressed -= Jump;
            
            inputChannel.preMovePerformed.RemoveListener(CancelMovement);
            inputChannel.preAttackPressed.RemoveListener(CancelMovement);
            inputChannel.preDashPerformed.RemoveListener(CancelMovement);
            inputChannel.preJumpPressed.RemoveListener(CancelMovement);
            inputChannel.preSecondaryAttackPressed.RemoveListener(CancelMovement);
            inputChannel.preFallPerformed.RemoveListener(CancelMovement);
        }

        private void Update()
        {
            if (GameState == GameState.Paused) return;

            if (_dash == null)
            {
                _dash = GameObject.Find("DashCooldownHudText");
            }
            
            if (_dash != null)
            {
                _dash.SetActive(true);
                if (_dashCooldown > 0)
                {
                    _dashCooldown -= Time.deltaTime;
                }
            
                if (_dashCooldown >= 0)
                {
                    _dash.GetComponent<TextMeshProUGUI>().text = $"{_dashCooldown:0.00}";
                }
            
                if (_dashCooldown <= 0)
                {
                    _dash.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            
            _animator.SetBool(IsWalking, _walking);
            _animator.SetBool(IsGrounded, _onGround);
            _animator.SetBool(Jumped, _jumped);
            _jumped = false;
        }
        
        private void FixedUpdate()
        {
            if (GameState == GameState.Paused)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }

            // check if player is on the ground
            _onGround = false;
            for (var i = 0; i < Physics2D.OverlapBoxNonAlloc(groundChecker.position, new Vector2(0.15f, 0.1f), 0, _groundColliders); i++)
            {
                var c = _groundColliders[i];
                if (c == null) continue;
                if (c.gameObject == gameObject) continue;
                _onGround = true;
            }

            // check if the player is touching a wall
            _touchingWall = false;
            for (var i = 0; i < Physics2D.OverlapBoxNonAlloc(frontChecker.position, new Vector2(0.1f, 1.06f), 0, _frontColliders); i++)
            {
                var c = _frontColliders[i];
                if (c == null) continue;
                if (c.gameObject == gameObject) continue;
                if (!c.gameObject.CompareTag("Environment")) continue;
                _adjacentWall = c.gameObject;
                _touchingWall = true;
            }

            // add player movement
            _walking = false;
            if (_direction != 0)
            {
                if (!_touchingWall)
                {
                    _rigidbody2D.velocity = new Vector2(_direction * (_movementSpeed * 300) * Time.deltaTime * _drag, Mathf.Clamp(_rigidbody2D.velocity.y, -25, 25));
                    _walking = true;
                }
                else if (_rigidbody2D.velocity.y < 0 && !_onGround)
                {
                    _rigidbody2D.velocity = new Vector2(0, -1);
                    _walking = true;
                }
            }
            
            // flipping the player appropriately
            if (_direction > 0 && !_facingRight)
            {
                Flip();
            }
            else if (_direction < 0 && _facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            if (GameState == GameState.Paused) return;
            
            _facingRight = !_facingRight;
            transform.Rotate(0, 180, 0);
        }

        #region Input Listeners

        private void MovePerformed(float ctx)
        {
            if (GameState == GameState.Paused) return;
            _direction = ctx;
        }

        private void MoveCancelled()
        {
            if (GameState == GameState.Paused) return;
            _direction = 0;
            LeanTween.value(gameObject, vector2 => _rigidbody2D.velocity = new Vector2(vector2.x, _rigidbody2D.velocity.y), _rigidbody2D.velocity, Vector2.zero, _onGround ? 0.1f : 0.5f);
        }

        private void Dash()
        {
            if (GameState == GameState.Paused) return;
            if (_dashCooldown > 0) return;
            GetComponent<PlayerCombat>().immune = true;
            _drag = _dashPower;
            AudioManager.PlayAudio(ESounds.Dash);
            LeanTween.value(gameObject, f =>
            {
                if (Math.Abs(f - 1) < 0.01)
                {
                    GetComponent<PlayerCombat>().immune = false;
                }
                _drag = f;
            }, _drag, 1,_dashDuration);
            _dashCooldown = _maxDashCooldown;
        }

        private void Fall()
        {
            if (GameState == GameState.Paused) return;
            _rigidbody2D.gravityScale *= 4;
        }

        private void FallCancel()
        {
            if (GameState == GameState.Paused) return;
            _rigidbody2D.gravityScale = 4;
        }

        private void Jump()
        {
            if (GameState == GameState.Paused) return;
            // if we are on the ground reset all jump count to 0
            if (_onGround)
            {
                _jumpCount = 0;
                _wallJumpCount = 0;
            }

            // if is not touching wall or is on the ground
            if (!_touchingWall || _onGround)
            {
                // if jump count is above 2 we don't proceed
                if (_jumpCount >= 2)
                {
                    return;
                }

                // jump
                _rigidbody2D.velocity = new Vector2(0, 15 * _jumpPower);
                _jumpCount++;
                _jumped = true;
            }
            // if we are on a wall and we are falling
            else if (_touchingWall && _rigidbody2D.velocity.y < 0)
            {
                // if we are on a different wall than we wall-jumped last time reset jump count to 0
                // if not we don't proceed.
                if (_wallJumpCount >= 1)
                {
                    if (_lastJumpedWall != _adjacentWall)
                    {
                        _wallJumpCount = 0;
                    }
                    else
                    {
                        return;
                    }
                }

                // jump
                _rigidbody2D.velocity = new Vector2(0, 15 * _jumpPower);
                _wallJumpCount++;
                _jumped = true;
                _lastJumpedWall = _adjacentWall;
            }
        }
        
        private void CancelMovement(InputAction.CallbackContext arg0, GameEvent arg1)
        {
            arg1.Cancelled = IsBehaviourPlaying;
        }

        private void CancelMovement(InputAction.CallbackContext arg0, DataContainingEvent<float> arg1)
        {
            arg1.Cancelled = IsBehaviourPlaying;
        }
        #endregion

        public void Save(BinaryWriter binaryWriter)
        {
            ((Vector2) transform.position).SaveToBinary(binaryWriter);
        }

        public void Load(BinaryReader binaryReader)
        {
            var transform1 = transform;
            transform1.position = ((Vector2) transform1.position).LoadFromBinary(binaryReader);
        }

        public static void LoadEscapeLevel()
        {
            FindObjectOfType<LevelManager>().LoadLevel(7, true);
        }
    }
}