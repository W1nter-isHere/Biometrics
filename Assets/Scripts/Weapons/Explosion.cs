using System;
using Cinemachine;
using Core;
using Managers;
using UnityEngine;

namespace Weapons
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private uint damage;
        [SerializeField] private float radius;
        [SerializeField] private float countDown;

        private GameObject _firer;
        private bool _damageFirer;
        
        public static void Spawn(GameObject firer, Vector2 origPos, bool damageFirer, bool attachToFirer)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Weapons/Explosion");
            if (prefab == null) throw new Exception("Explosion prefab could not be loaded!");
            var explosion = attachToFirer ? Instantiate(prefab, firer.transform) : Instantiate(prefab);
            explosion.transform.position = origPos;
            explosion.GetComponent<Explosion>()._damageFirer = damageFirer;
            explosion.GetComponent<Explosion>()._firer = firer;
        }

        private void Start()
        {
            Invoke(nameof(Explode), countDown);
        }

        public void Explode()
        {
            if (_damageFirer)
            {
                var enemy = _firer.GetComponent<IDamagable>();
                enemy?.Damage(damage);
            }
            
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            
            AudioManager.PlayAudio(ESounds.Explosion);
            foreach (var hit in Physics2D.OverlapCircleAll(transform.position, radius))
            {
                if (hit.CompareTag("Projectile")) continue;
                if (hit.CompareTag("Environment")) continue;
                // see if it is blocked by some environment
                // var raycastHit2D = Array.Find(Physics2D.RaycastAll(transform.position, (hit.transform.position - transform.position).normalized), ray => ray.collider.CompareTag("Environment"));
                // if (raycastHit2D.collider != null) continue;
                DealDamage(hit);
            }
            
            var prefab = Resources.Load<GameObject>("Prefabs/Weapons/ExplosionVFX");
            if (prefab == null) throw new Exception("ExplosionVFX prefab could not be loaded!");
            var explosion =Instantiate(prefab);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
            
            void DealDamage(Collider2D hit)
            {
                var enemy = hit.GetComponent<IDamagable>();
                enemy?.Damage(damage);
                Vector2 direction = hit.transform.position - transform.position;
                if (hit.TryGetComponent(out Rigidbody2D rb2d))
                {
                    rb2d.AddForce(direction.normalized*15, ForceMode2D.Impulse);
                }
            }
        }
    }
}