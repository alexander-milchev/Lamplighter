using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject wastedScreen;
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fillImage;

    [Header("Colours")]
    [SerializeField] Color fullColour = Color.green;
    [SerializeField] Color lowColour = Color.green;

    [Header("Buttons")]
    [SerializeField] private Button mainMenu;

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
    }

    private void EscMenuOpen(object sender, EventArgs e)
    {
        TogglePause();
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
        darkOverlay.SetActive(false);
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

}
