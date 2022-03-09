using UnityEngine;

namespace Dialogue.Scripts.DataKeepers
{
    public class DialogueDataKeeperTwo : Singleton<DialogueDataKeeperTwo>, IDataKeeper
    {
        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}