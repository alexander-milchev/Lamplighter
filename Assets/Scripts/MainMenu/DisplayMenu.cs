using UnityEngine;
using UnityEngine.UI;

public class DisplayMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;
    [SerializeField] private Toggle screenShakeToggle;
    [SerializeField] private Slider gammaSlider;
    
    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Apply()
    {
        
    }
}
