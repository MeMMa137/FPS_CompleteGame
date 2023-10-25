using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public string cpName;

    void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))//controlla se nel PlayerPrefs esistono questi contenuti (quelli all'interno di HasKey)
        {
            if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName) //verifica se l'ultimo checkpoint salvato corrisponde con questo
            {
                PlayerController.instance.transform.position = transform.position; //teletrasporta qua il player
                Physics.SyncTransforms(); //*non in codice tut, potrebbe causare problemi
                Debug.Log("sei tornato a: " + cpName);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", ""); //se premuto L, tornerà al punto di partenza, non dal chec
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //controlla se ad andare sopra il checkpoint è il player
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName); //salva i dati del checkpoint in modo "forzato"
            Debug.Log("sei sul cp: " + cpName);

            AudioManager.instance.PlaySXF(1);
        }
    }

}
