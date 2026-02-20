using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DisplayMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;
    [SerializeField] private Toggle screenShakeToggle;
    [SerializeField] private Slider gammaSlider;
    [SerializeField] private GameObject cam;
    
    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Apply()
    {
        if (cam.TryGetComponent<LiftGammaGain>(out LiftGammaGain lgg))
        {
            Debug.Log(lgg.gamma);
        }
    }
}
