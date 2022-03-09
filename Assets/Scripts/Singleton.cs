using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static Singleton<T> Instance { get; protected set; }
    public T Object { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        Object = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}