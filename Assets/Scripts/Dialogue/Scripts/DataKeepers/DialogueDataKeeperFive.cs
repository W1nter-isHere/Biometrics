using UnityEngine;

namespace Dialogue.Scripts.DataKeepers
{
    public class DialogueDataKeeperFive : Singleton<DialogueDataKeeperFive>, IDataKeeper
    {
        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}