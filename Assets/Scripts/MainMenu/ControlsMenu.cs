using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;

    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }
}
