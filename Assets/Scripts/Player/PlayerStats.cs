using System.IO;
using Core;
using SaveLoad;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Game/New Player Stats")]
    public class PlayerStats : ScriptableObject, ISaveable
    {
        [Header("Combat")]
        public uint maxHealth;
        public uint attackDamage;
        public float attackRadius;
        public float maxThrowAttackCooldown = 5f;

        [Header("Movements")]
        public float movementSpeed = 1f;
        public float dashPower = 10f;
        public float dashDuration = .2f;
        public float maxDashCooldown = 2f;
        public float jumpPower = 1f;
        
        public void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(maxHealth);
            binaryWriter.Write(attackDamage);
            binaryWriter.Write(attackRadius);

            binaryWriter.Write(movementSpeed);
            binaryWriter.Write(dashPower);
            binaryWriter.Write(dashDuration);
            binaryWriter.Write(maxDashCooldown);
            binaryWriter.Write(jumpPower);
        }

        public void Load(BinaryReader binaryReader)
        {
            maxHealth = binaryReader.ReadUInt32();
            attackDamage = binaryReader.ReadUInt32();
            attackRadius = binaryReader.ReadSingle();

            movementSpeed = binaryReader.ReadSingle();
            dashPower = binaryReader.ReadSingle();
            dashDuration = binaryReader.ReadSingle();
            maxDashCooldown = binaryReader.ReadSingle();
            jumpPower = binaryReader.ReadSingle();
        }
    }
}