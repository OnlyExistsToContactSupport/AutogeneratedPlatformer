using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    // Start is called before the first frame update
    public void Start()
    {
        // Load the saved volume and brightness settings
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    public void OpenSettings()
    {
        // Load the settings scene
        ShowSettingsMenu();
    }
}
