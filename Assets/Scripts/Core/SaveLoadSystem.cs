using System.IO;
using System.Linq;
using System.Text;
using Core;
using UnityEngine;

namespace SaveLoad
{
    public static class SaveLoadSystem
    {
        private static readonly string FilePath = Application.persistentDataPath + "/Saves/1.biometrics";
        private static readonly string Path = Application.persistentDataPath + "/Saves/";
        
        /// <summary>
        /// Calls the save method on all MonoBehaviours and ScriptableObject that implements the ISaveable interface
        /// </summary>
        public static void Save()
        {
            var saveablesMb = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<ISaveable>().ToArray();
            var saveablesSo = Resources.FindObjectsOfTypeAll<ScriptableObject>().OfType<ISaveable>().ToArray();
            var saveables = saveablesMb.Concat(saveablesSo).ToArray();

            if (!File.Exists(Path)) Directory.CreateDirectory(Path);
                
            using var fileStream = new FileStream(FilePath, FileMode.Create);
            using var binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8);
            foreach (var saveable in saveables)
            {
                saveable.Save(binaryWriter);
            }
        }
        
        /// <summary>
        /// Calls the load method on all MonoBehaviours and ScriptableObject that implements the ISaveable interface
        /// </summary>
        public static void Load()
        {
            var saveablesMb = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<ISaveable>().ToArray();
            var saveablesSo = Resources.FindObjectsOfTypeAll<ScriptableObject>().OfType<ISaveable>().ToArray();
            var saveables = saveablesMb.Concat(saveablesSo).ToArray();

            if (!File.Exists(Path)) Directory.CreateDirectory(Path);

            using var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate);
            using var binaryReader = new BinaryReader(fileStream, Encoding.UTF8);
            foreach (var saveable in saveables)
            {
                saveable.Load(binaryReader);
            }
        }
    }
}