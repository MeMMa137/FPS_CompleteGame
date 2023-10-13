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

    public int currentAmmo;
   
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
}
