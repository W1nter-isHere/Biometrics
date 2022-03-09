using System;
using Core;
using Managers;
using Player;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private uint damage;
        [SerializeField] private uint flySpeed;
        [SerializeField] private float expireTime;

        private GameObject _firer;
        private Vector2 _direction;
        private bool _ignoreEnemy;
        
        public static void Spawn(GameObject firer, BulletTypes bulletTypes, Vector2 direction, Vector2 origPos, bool ignoreEnemy = true)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Weapons/Bullets/" + bulletTypes);
            if (prefab == null) throw new Exception(bulletTypes + " bullet prefab could not be loaded!");
            var bullet = Instantiate(prefab);
            bullet.transform.position = origPos;
            bullet.GetComponent<Bullet>()._ignoreEnemy = ignoreEnemy;
            bullet.GetComponent<Bullet>()._firer = firer;
            bullet.GetComponent<Bullet>()._direction = direction;
            AudioManager.PlayAudio(ESounds.EnemyShoot);
        }

        private void Start()
        {
            Invoke(nameof(Expire), expireTime);
        }

        protected virtual void FixedUpdate()
        {
            if (PlayerController.Instance.Object.GameState == GameState.Paused) return;
            GetComponent<Rigidbody2D>().velocity = _direction.normalized * flySpeed;
        }

        private void Expire()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile")) return;
            if (other.CompareTag("Environment"))
            {
                Expire();
                return;
            }
            if (_ignoreEnemy && other.CompareTag("Enemy")) return;
            if (other.gameObject == _firer) return;
            var enemy = other.GetComponent<IDamagable>();
            if (enemy == null) return;
            if (enemy.Damage(damage))
            {
                Expire();
            }
        }
    }   
}