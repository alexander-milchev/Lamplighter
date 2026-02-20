using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject soundMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject previous;
    
    public void NavigateDisplay()
    {
        displayMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void NavigateSound()
    {
        soundMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void NavigateControls()
    {
        controlsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }
}
