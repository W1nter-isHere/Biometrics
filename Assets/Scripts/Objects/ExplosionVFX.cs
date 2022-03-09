using UnityEngine;

namespace Objects
{
    public class ExplosionVFX : MonoBehaviour
    {
        private void Start()
        {
            Invoke(nameof(Destroy), 0.25f);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}