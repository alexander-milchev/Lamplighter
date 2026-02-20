using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject startCheckpoint;
    
    private GameObject lastCheckpoint;

    private int[] collectibles = {-1, -1, -1};

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

    public void CollectCollectible(int coin)
    {
        collectibles[coin - 1] = coin;
    }

    public void DebugCollectibles()
    {
        foreach (int col in collectibles)
        {
            Debug.Log("Coin " + col);
        }
    }
}
