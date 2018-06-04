using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    void Update()
    {

        if (!GameIsPaused)
       {
            Cursor.lockState = CursorLockMode.Locked;
       }
       else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();   
                //Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                //Cursor.lockState = CursorLockMode.Confined;
            }
            
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()

    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

}