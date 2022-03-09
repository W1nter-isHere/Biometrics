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
    public class BomberEnemy : EnemyBase, ISaveable
    {
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
                    while (Vector2.Distance(Rigidbody2D.position, target.position) > 1)
                    {
                        MoveTowardsTarget(false, false);
                        yield return null;
                    }

                    // if the code reaches here means bomber is close to player
                    Rigidbody2D.velocity = Vector2.zero;
                    Explosion.Spawn(gameObject, attackPoint.position, true, true);
                    yield break;
                }

                yield return new WaitForSeconds(.05f);
            }
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