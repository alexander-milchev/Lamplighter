using System;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject startCheckpoint;
    
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
    }

    private void Start()
    {
        lastCheckpoint = startCheckpoint;
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public Vector2 GetSpawnPos()
    {
        return lastCheckpoint.transform.position;
    }

}
