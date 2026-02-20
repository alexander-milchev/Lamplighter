using UnityEngine;

public class SoundMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;

    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }
}
