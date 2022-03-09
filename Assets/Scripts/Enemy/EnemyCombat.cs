using System;
using System.Collections;
using System.IO;
using Core;
using Managers;
using Player;
using SaveLoad;
using UnityEngine;

namespace Enemy
{
    public class EnemyCombat : MonoBehaviour, ISaveable, IDamagable
    {
        [SerializeField] protected EnemyStats stats;

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

        private bool _initiatedDeath;

        private void Start()
        {
            InitializeStats();
            GetComponent<SpriteRenderer>().material.SetFloat(PlayerController.Progress1, 2);
            GetComponent<SpriteRenderer>().material.SetInt(PlayerController.Enabled, 1);
        }

        private void HealthUpdated()
        {
            if (Health <= 0)
            {
                Kill();
            }
        }

        private void Update()
        {
            if (transform.position.y < -10 && !_initiatedDeath)
            {
                Kill();
            }
        }

        public void InitializeStats()
        {
            Health = stats.maxHealth;
        }

        public bool Damage(uint amount)
        {
            Health -= amount;
            return true;
        }

        public void Heal(uint amount)
        {
            Health -= amount;
        }

        private void Kill()
        {
            _initiatedDeath = true;
            StartCoroutine(Death());
            
            IEnumerator Death()
            {
                if (FindObjectOfType<EnemyManager>() != null) FindObjectOfType<EnemyManager>().RemovedEnemy();
                AudioManager.PlayAudio(ESounds.EnemyDeath);
                LeanTween.value(gameObject, f => GetComponent<SpriteRenderer>().material.SetFloat("_Progress", f), 2, 0, 0.2f);
                yield return new WaitForSeconds(0.2f);
                Destroy(gameObject.transform.parent.gameObject);
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
    }
}