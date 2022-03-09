using System;
using System.Collections;
using Core;
using Managers;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private uint damage;
        [SerializeField] private uint flySpeed;
        [SerializeField] private float expireTime = 8;

        private Transform _firerTransform;
        private Vector2 _direction;
        private bool _flyingBack = false;

        private void Start()
        {
            Invoke(nameof(AutoReturn), expireTime);
        }

        public static void Spawn(GameObject firer, Vector2 origPos, Vector2 direction)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Weapons/Sword");
            if (prefab == null) throw new Exception("Sword prefab could not be loaded!");
            var sword = Instantiate(prefab);
            sword.transform.position = origPos;
            sword.transform.rotation = Quaternion.Euler(0, 0, -30);
            sword.GetComponent<Sword>()._firerTransform = firer.transform;
            sword.GetComponent<Sword>()._direction = direction;

            AudioManager.PlayAudio(ESounds.SwingSword);
            FindObjectOfType<PlayerCombat>().canAttack = false;
            FindObjectOfType<PlayerCombat>().transform.Find("Sword").gameObject.SetActive(false);
        }

        protected virtual void FixedUpdate()
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            if (_flyingBack)
            {
                Vector2 dir = (_firerTransform.position - transform.position).normalized;
                GetComponent<Rigidbody2D>().velocity = dir.normalized * flySpeed * 2;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = _direction.normalized * flySpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Hit(other);    
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Hit(other);
        }

        private void Hit(Collider2D other)
        {
            if (other.CompareTag("Projectile")) return;
            if (other.CompareTag("Player")) return;
            if (other.CompareTag("Environment"))
            {
                Return();
                return;
            }

            var enemy = other.GetComponent<IDamagable>();
            if (enemy == null) return;
            if (enemy.Damage(damage))
            {
                Return();
            }
        }

        private void AutoReturn()
        {
            if (!_flyingBack) Return();
        }
        
        private void Return()
        {
            _flyingBack = true;
            StartCoroutine(DestroyOnBack());
            
            IEnumerator DestroyOnBack()
            {
                if (transform == null) yield break;
                while (Vector2.Distance(transform.position, _firerTransform.position) > 1)
                {
                    yield return null;
                }
                
                FindObjectOfType<PlayerCombat>().canAttack = true;
                FindObjectOfType<PlayerCombat>().transform.Find("Sword").gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}