using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Enemy Stats", menuName = "Game/New Enemy Stats")]
    public class EnemyStats : ScriptableObject
    {
        [Header("Combat")]
        public uint maxHealth;
        public uint attackDamage;
        public float attackRadius;
        
        [Header("Movements")]
        public float movementSpeed = 1f;
        public float waypointTolerance = 2f;
        public float seekRadius = float.PositiveInfinity;
        
        [Header("Aesthetics")] 
        public Sprite enemySprite;
    }
}