using UnityEngine;

namespace Dialogue.Scripts.DataKeepers
{
    public class DialogueDataKeeper : Singleton<DialogueDataKeeper>, IDataKeeper
    {
        public GameObject GetGO()
        {
            return gameObject;
        }
    }
}