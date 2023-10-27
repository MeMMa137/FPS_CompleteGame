using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;

    [HideInInspector]
    public bool levelEnding;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUnpause();
        }
    }
    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo()); //permette di avere un ritardo di scena dopo la morte
        
    }

    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying); //finisce la funzione e aspetta tot secondi
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //carica la scena (livello) attuale
    }

    public void pauseUnpause()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f; //riattiva il gioco dopo il menu

            PlayerController.instance.footstepFast.Play();
            PlayerController.instance.footstepSlow.Play();

        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true); //attiva menu in gioco
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f; //blocca il gioco

            PlayerController.instance.footstepFast.Stop();
            PlayerController.instance.footstepSlow.Stop();
        }
    }

}
