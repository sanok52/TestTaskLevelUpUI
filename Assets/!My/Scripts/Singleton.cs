using System.Collections;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool _dontDestoryOnLoad;

    public static T Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (_dontDestoryOnLoad)
            DontDestroyOnLoad(gameObject);

        AwakeSingleton();
    }

    public virtual void AwakeSingleton()
    { }
}