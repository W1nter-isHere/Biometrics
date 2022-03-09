using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Weapons
{
    public class LaserRay : MonoBehaviour
    {
        [SerializeField] private uint damage;
        private Vector2 _origPos;
        private Vector2 _direction;
        private LineRenderer _lineRenderer;
        
        public static void Spawn(Vector2 origPos, Vector2 direction)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Weapons/LaserRay");
            if (prefab == null) throw new Exception("Explosion prefab could not be loaded!");
            var laser = Instantiate(prefab);
            laser.GetComponent<LaserRay>()._origPos = origPos;
            laser.GetComponent<LaserRay>()._direction = direction;
        }

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.SetPosition(0, _origPos);
            StartCoroutine(Behaviour());
        }

        private IEnumerator Behaviour()
        {
            LeanTween.value(gameObject, SetLineWidth, 0.3f, 0.6f, 0.5f);
            foreach (var hits in Physics2D.RaycastAll(_origPos, _direction))
            {
                if (hits.collider == null) continue;
                if (hits.collider.gameObject == gameObject) continue;
                if (hits.collider.gameObject.CompareTag("Enemy")) continue;
                if (hits.collider.gameObject.CompareTag("Environment")) break;
                if (hits.collider.gameObject.CompareTag("Projectile")) break;
                GetComponent<LineRenderer>().SetPosition(1, hits.point);
                hits.collider.gameObject.GetComponent<IDamagable>()?.Damage(damage);
            }
            yield return new WaitForSeconds(.1f);
            Destroy(gameObject);
        }

        private void SetLineWidth(float width)
        {
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;
        }
    }
}