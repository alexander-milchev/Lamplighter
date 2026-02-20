using UnityEngine;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private GameObject[] coins;

    public void EnableCoin(int i)
    {
        coins[i].SetActive(true);
    }
}
