using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public bool canAutoFire;
    public float fireRate;
    [HideInInspector] //nasconde la casella dall'editor di unity, per evitare che venga modificato
    public float fireCounter;

    public int currentAmmo, pickupAmount; //pickupAmount stabilisce il numero di proiettili da poter prendere per ogni arma

    public Transform firepoint;

    public float zoomAmount;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime; //fa scendere di 1 il valore di fileCounter (60fps)
        }
    }

    public void GetAmmo()
    {
        currentAmmo += pickupAmount; //aumenta il numero di colpi

        UIController.instance.ammoText.text = "COLPI: " + currentAmmo; //aggiorna la barra di munizioni grafica
    }

    

}
