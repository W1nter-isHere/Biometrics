using UnityEngine;

namespace Dialogue.Scripts.DataKeepers
{
    public class DialogueDataKeeperFour : Singleton<DialogueDataKeeperFour>, IDataKeeper
    {
        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}