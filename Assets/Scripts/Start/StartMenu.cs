using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("GameScene");
    }
    public void OpenSettings()
    {
        // Load the settings game scene
        SceneManager.LoadScene("SettingsScene");
    }
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
