using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenCredits()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=uW3BXm2eQ6M");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME TRIGGERED!"); 
        
        Application.Quit();
    }
}