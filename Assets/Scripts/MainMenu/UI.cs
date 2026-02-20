using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pausedScreen;
    [SerializeField] private GameObject wastedScreen;
    [SerializeField] private GameObject darkOverlay;

    [Header("Buttons")]
    [SerializeField] private Button mainMenu;

    private readonly int mainMenuIndex = 0;
    private bool isPaused;

    private void Start()
    {
        GameInput.instance.OnEscape += EscMenuOpen;
        PlayerHealth.instance.OnDeath += DeathScreen;
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
