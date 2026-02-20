using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject soundMenu;
    [SerializeField] private GameObject controlsMenu;

    [Header("Buttons")]
    [SerializeField] private GameObject startButtons; 
    public void Play()
    {
        Debug.Log("Play");
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        startButtons.SetActive(false);
    }

    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        startButtons.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quitting..");
    }
}
