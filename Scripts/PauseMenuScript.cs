using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool isPaused = false;                 //bool for if game is paused 
    public GameObject PauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))            // if escape is pressed 
        {
            if(isPaused == true)                         //game is already paused 
            {
                Resume();                                //resume the game
            }
            else
            {
                Pause();                                  //pause the game 
            }

        }
        
    }

    public void Resume()                                          //resume the game
    { 
        isPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;                                      //return game to normal speed
        
    }

    void Pause()                                           //pause the game
    {
        

        isPaused = true;                                   //paused 
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;                               //freeze game


    }

    public void GoToMain()                               // if main menu is selected 
    {
        isPaused = false;                                //no longer paused 
        Time.timeScale = 1f;                             //revert back to normal 
        SceneManager.LoadScene("Menu");                  //load the main menu scene 
    }

    public void Quit()                                   // if quit is selected
    {
        Application.Quit();                              //quit the application
    }


}
