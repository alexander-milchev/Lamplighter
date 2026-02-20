using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    [SerializeField] private int coinID = 1;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            GameManager.instance.CollectCollectible(coinID);
            gameObject.SetActive(false);
        }
    }
}
