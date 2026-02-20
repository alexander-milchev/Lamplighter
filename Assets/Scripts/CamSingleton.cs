using UnityEngine;

public class CamSingleton : MonoBehaviour
{
    public static CamSingleton instance;

    private void Awake()
    {
        SingletonPattern();
    }
    
    private void SingletonPattern()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
