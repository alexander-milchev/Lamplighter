using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject lastCheckpoint;

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

    public void SetCheckpoint()
    {
        Debug.Log("set checkpoint");
    }
}
