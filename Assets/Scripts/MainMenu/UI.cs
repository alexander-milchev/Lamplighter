using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject wastedScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fillImage;
    [Header("HP stuff")]
    [SerializeField] private Image[] hpImages;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeMagnitude = 10f;

    [Header("Colours")]
    [SerializeField] Color fullColour = Color.green;
    [SerializeField] Color lowColour = Color.green;

    [Header("Fade Settings")]
    [Range(0f, 1f)]
    public float fadeThreshold = 0.95f;
    public float fadeSpeed = 2f;

    private readonly int mainMenuIndex = 0;
    private bool isPaused;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        GameInput.instance.OnEscape += EscMenuOpen;
        PlayerHealth.instance.OnDeath += DeathScreen;
        PlayerHealth.instance.OnTakeDamage += UpdateHP;
        PlayerHealth.instance.OnRespawn += ResetHP;
        GameInput.instance.OnEndLevel += WinScreen;

        fuelSlider.minValue = 0;
        fuelSlider.maxValue = 1f;

        canvasGroup = fuelSlider.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = fuelSlider.gameObject.AddComponent<CanvasGroup>();
        UpdateSlider();
    }

    private void Update()
    {
        UpdateSlider();
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;

        GameInput.instance.OnEscape -= EscMenuOpen;
        PlayerHealth.instance.OnDeath -= DeathScreen;
        PlayerHealth.instance.OnTakeDamage -= UpdateHP;
        PlayerHealth.instance.OnRespawn -= ResetHP;
        GameInput.instance.OnEndLevel -= WinScreen;
    }

    private void EscMenuOpen(object sender, EventArgs e)
    {
        TogglePause();
    }

    private void UpdateHP(object sender, EventArgs e)
    {
        int hp = PlayerHealth.instance.GetCurrentHP();

        for (int i = 0; i < hpImages.Length; i++)
        {
            int reversedIndex = hpImages.Length - 1 - i;
            bool shouldBeHidden = reversedIndex >= hp - 1;

            if (shouldBeHidden && hpImages[reversedIndex].gameObject.activeSelf)
            {
                StartCoroutine(ShakeThenHide(hpImages[reversedIndex]));
            }
        }
    }

    private void ResetHP(object sender, EventArgs e)
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            hpImages[i].gameObject.SetActive(true);
        }
    }


    private void TogglePause()
    {
        isPaused = !isPaused;

        pausedScreen.SetActive(isPaused);

        if (isPaused)
        {
            darkOverlay.SetActive(isPaused);
            Time.timeScale = 0f;
        }
        else
        {
            darkOverlay.SetActive(isPaused);
            Time.timeScale = 1f;

        }
    }

    private void DeathScreen(object sender, EventArgs e)
    {
        wastedScreen.SetActive(true);
        darkOverlay.SetActive(true);
        
    }

    private void WinScreen(object sender, EventArgs e)
    {
        winScreen.SetActive(true);
        darkOverlay.SetActive(true);
        int[] coins = GameManager.instance.GetCollectibles();
        foreach (int c in coins)
        {
            if (c > 0)
            {
                if (winScreen.TryGetComponent<WinScreenScript>(out WinScreenScript wss))
                {
                    wss.EnableCoin(c - 1);
                }
            }
        }
    }

    void UpdateSlider()
    {
        float max = LanternController.instance.GetMaxLightMeter();
        float current = LanternController.instance.GetCurrentLightMeter();

        float normalized = (max > 0) ? current / max : 0f;

        fuelSlider.value = normalized;

        float targetAlpha = normalized >= fadeThreshold ? 0f : 1f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        if (fillImage != null)
        {
            Color baseColor = Color.Lerp(lowColour, fullColour, normalized);
            fillImage.color = baseColor * 2.5f;
        }
    }

    public void RespawnButton()
    {
        PlayerHealth.instance.Respawn();
        wastedScreen.SetActive(false);
        winScreen.SetActive(false);
        darkOverlay.SetActive(false);
        for (int i = 0; i < hpImages.Length; i++)
            {
            hpImages[i].gameObject.SetActive(true);
            }
            fuelSlider.gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        TogglePause();
    }

    public void LoadMainMenu()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Next");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator ShakeThenHide(Image image)
    {
        RectTransform rect = image.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPos.x + UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = originalPos.y + UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude);
            rect.anchoredPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
        image.gameObject.SetActive(false);
    }

}
