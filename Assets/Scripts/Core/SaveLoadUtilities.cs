using System.IO;
using UnityEngine;

namespace SaveLoad
{
    public static class SaveLoadUtilities
    {
        public static void SaveToBinary(this Vector2 vector2, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(vector2.x);
            binaryWriter.Write(vector2.y);
        }
        
        public static Vector2 LoadFromBinary(this Vector2 vector2, BinaryReader binaryReader)
        {
            vector2.x = binaryReader.ReadSingle();
            vector2.y = binaryReader.ReadSingle();
            return vector2;
        }
    }
}