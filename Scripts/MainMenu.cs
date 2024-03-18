using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour                    //functionality for main menu 
{                          
   public void PuzzleMode()                              //button for puzzle mode. puzzles are set in stone 
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load next scene in the buildIndex 


    }

    public void Quit()                                   //quit the game
    {
        Application.Quit();
    }
}
