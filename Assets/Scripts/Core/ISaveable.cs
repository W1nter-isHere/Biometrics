using System.IO;

namespace Core
{
    public interface ISaveable
    {
        void Save(BinaryWriter binaryWriter);

        void Load(BinaryReader binaryReader);
    }
}