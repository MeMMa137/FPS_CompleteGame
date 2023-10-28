using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{

    public string mainMenuScene;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
    public void Resume()
    {
        GameManager.instance.pauseUnpause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("uscito dal gioco bro");
    }

}
