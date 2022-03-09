using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Channels;
using Core;
using Managers;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapons;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerCombat : MonoBehaviour, ISaveable, IDamagable
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private InputChannel inputChannel;
        [SerializeField] private Transform playerCenter;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private Transform throwAttackPoint;
        [SerializeField] private Animator animator;

        public bool immune;
        public bool canAttack = true;

        private uint _health;
        private uint Health
        {
            get => _health;
            set
            {
                _health = value;
                HealthUpdated();
            }
        }

        private GameObject _cooldown;
        private GameObject _deathCounter;
        private float _throwAttackCooldown;
        private bool _attacked;
        private bool _secondaryAttacked;
        private static readonly int Attacked1 = Animator.StringToHash("Attacked1");
        private static readonly int Attacked2 = Animator.StringToHash("Attacked2");
        private int _attackAnimation = -1;
        private static readonly int SecondaryAttacked = Animator.StringToHash("SecondaryAttacked");

        private void Start()
        {
            if (playerStats == null) throw new Exception("Player Stats can not be null!");
            _health = playerStats.maxHealth;
            _cooldown = GameObject.Find("ThrowSwordCooldownText");
            _deathCounter = GameObject.Find("DeathCount");
        }

        private void OnEnable()
        {
            inputChannel.OnAttackPressed += Attack;
            inputChannel.OnSecondaryAttackPressed += SecondaryAttack;
        }

        private void OnDisable()
        {
            inputChannel.OnAttackPressed -= Attack;
            inputChannel.OnSecondaryAttackPressed -= SecondaryAttack;
        }

        private void Update()
        {
            if (PlayerController.Instance == null) return;
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            if (transform.position.y < -10)
            {
                Kill();
                return;
            }

            if (_cooldown == null)
            {
                _cooldown = GameObject.Find("ThrowSwordCooldownText");
            }
            
            if (_cooldown != null)
            {
                _cooldown.SetActive(true);
                if (_throwAttackCooldown > 0)
                {
                    _throwAttackCooldown -= Time.deltaTime;
                }

                if (_throwAttackCooldown >= 0)
                {
                    _cooldown.GetComponent<TextMeshProUGUI>().text = $"{_throwAttackCooldown:0.00}";
                }
            
                if (_throwAttackCooldown <= 0)
                {
                    _cooldown.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            
            animator.SetBool(SecondaryAttacked, _secondaryAttacked);
            if (_attackAnimation == -1) return;
            animator.SetBool(_attackAnimation, _attacked);
            _attackAnimation = -1;
        }

        private void Attack()
        {
            if (_attacked) return;
            if (_secondaryAttacked) return;
            if (!canAttack) return;
            var playerController = PlayerController.Instance.Object;
            if (playerController.GameState == GameState.Paused) return;
            if (!playerController.OnGround) return;
            // wall sliding
            if (playerController.TouchingWall && !playerController.OnGround) return;
            var hits = Physics2D.OverlapCircleAll(attackPoint.position, playerStats.attackRadius);
            foreach (var hit in hits)
            {
                if (hit == null) continue;
                if (hit.gameObject == gameObject) continue;
                if (!hit.gameObject.CompareTag("Enemy")) continue;
                var enemy = hit.gameObject.GetComponent<IDamagable>();
                enemy?.Damage(playerStats.attackDamage);
            }
            
            AudioManager.PlayAudio(ESounds.SwingSword);
            AttackAnim();
        }
        
        private void SecondaryAttack()
        {
            if (_attacked) return;
            if (_secondaryAttacked) return;
            if (!canAttack) return;
            if (_throwAttackCooldown > 0) return;
            var playerController = PlayerController.Instance.Object;
            if (playerController.GameState == GameState.Paused) return;
            
            _secondaryAttacked = true;
            StartCoroutine(StopAttack());

            // wall sliding
            if (playerController.TouchingWall && !playerController.OnGround) return;
            var position = throwAttackPoint.position;
            var mousePos = UnityEngine.Camera.main.ScreenToWorldPoint(inputChannel.MousePosition);
            Sword.Spawn(gameObject, position, (mousePos - position).normalized);

            _throwAttackCooldown = playerStats.maxThrowAttackCooldown;
            
            IEnumerator StopAttack()
            {
                yield return new WaitForSeconds(0.09f);
                _secondaryAttacked = false;
            }
        }

        private void AttackAnim()
        {
            var rand = Random.Range(1, 3) == 1;

            if (rand)
            {
                _attackAnimation = Attacked1;
                _attacked = true;
                StartCoroutine(StopAttack());

                IEnumerator StopAttack()
                {
                    yield return new WaitForSeconds(0.35f);
                    _attackAnimation = Attacked1;
                    _attacked = false;
                }
            }
            else
            {
                _attackAnimation = Attacked2;
                _attacked = true;
                StartCoroutine(StopAttack());

                IEnumerator StopAttack()
                {
                    yield return new WaitForSeconds(0.35f);
                    _attackAnimation = Attacked2;
                    _attacked = false;
                }
            }
        }
        
        public void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(_health);
        }

        public void Load(BinaryReader binaryReader)
        {
            _health = binaryReader.ReadUInt32();
        }

        private void HealthUpdated()
        {
            if (_health == 0)
            {
                Kill();
            }
        }

        public void Kill()
        {
            PlayerPrefs.SetInt("DeathCount", PlayerPrefs.GetInt("DeathCount") + 1);
            if (_deathCounter == null)
            {
                _deathCounter = GameObject.Find("DeathCount");
            }
            _deathCounter.GetComponent<DeathCounter>().UpdateCount();
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.Object.GameState = GameState.Paused;
            }
            FindObjectOfType<LevelManager>().LoadLevel(SceneManager.GetActiveScene().buildIndex, () => Destroy(gameObject), true, 0.8f);
        }
        
        public bool Damage(uint amount)
        {
            if (immune) return false;
            Health -= amount;
            return true;
        }

        public void Heal(uint amount)
        {
            Health += amount;
        }

        public void FullHealth()
        {
            Health = playerStats.maxHealth;
        }
    }
}