using System;
using System.Collections;
using System.IO;
using Core;
using Player;
using SaveLoad;
using UnityEngine;
using Weapons;

namespace Enemy.AI
{
    public class AkEnemy : EnemyBase, ISaveable
    {
        private bool _canMove;
        
        protected override void Start()
        {
            base.Start();
            StartCoroutine(AI());
        }

        private IEnumerator AI()
        {
            // this will stop once the game object is destroyed so it's fine.
            while (true)
            {
                yield return new WaitUntil(() => PlayerController.Instance.Object.GameState != GameState.Paused);
                LookTowardsTarget();
                if (HasLineOfSight)
                {
                    // rapidly shoot 5 times
                    for (var i = 0; i < 5; i++)
                    {
                        Bullet.Spawn(gameObject, BulletTypes.Ak, DirectionOfTarget, attackPoint.position);
                        yield return new WaitForSeconds(.05f);   
                    }

                    yield return new WaitForSeconds(5f);
                }
                else
                {
                    yield return new WaitForSeconds(.1f);
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }
        
        public void Save(BinaryWriter binaryWriter)
        {
            ((Vector2) transform.position).SaveToBinary(binaryWriter);
        }

        public void Load(BinaryReader binaryReader)
        {
            var transform1 = transform;
            transform1.position = ((Vector2) transform1.position).LoadFromBinary(binaryReader);
        }
    }
}