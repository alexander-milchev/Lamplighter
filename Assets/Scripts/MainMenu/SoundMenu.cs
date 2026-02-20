using UnityEngine;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    [SerializeField] private GameObject previous;
    [SerializeField] private GameObject audioSource;
    [SerializeField] private Slider volumeSlide;

    public void Back()
    {
        previous.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Apply()
    {
        if (audioSource.TryGetComponent<AudioSource>(out AudioSource music))
        {
            music.volume = volumeSlide.value;
            Debug.Log(volumeSlide.value);
        }
    }
}
