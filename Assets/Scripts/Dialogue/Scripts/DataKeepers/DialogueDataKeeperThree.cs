using UnityEngine;

namespace Dialogue.Scripts.DataKeepers
{
    public class DialogueDataKeeperThree : Singleton<DialogueDataKeeperThree>, IDataKeeper
    {
        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}