using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;

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

}
