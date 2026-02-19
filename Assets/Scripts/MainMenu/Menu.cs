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
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button displayButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button backButton;
    public void Play()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    public void Options()
    {
        
    }
}
