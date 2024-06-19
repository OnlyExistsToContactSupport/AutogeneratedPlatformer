using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    private bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }

            isPaused = !isPaused;
        }
    }
    public void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Unpause()
    {
        panel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Settings()
    {

    }
    public void Quit()
    {
        Application.Quit();
    }
}
