using UnityEngine;

public class DisplayMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;
    
    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }
}
