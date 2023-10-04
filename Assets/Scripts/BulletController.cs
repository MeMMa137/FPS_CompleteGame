using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB; // RB = rigid Body

    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed; //sposta il proiettile

        lifeTime -= Time.deltaTime; //utilizza i frame del dispositivo, calcolando il tempo, toglierà quindi alla variabile lifeTime 1 unità al secondo (su 60fps)

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) //quando l'oggetto associato a questo script colliderà con un altro oggetto, allora si attiverà la funizone
    {
        if(other.gameObject.tag == "Enemy") 
        {
            Destroy(other.gameObject); //se il proiettile colpisce un oggetto di tipo "Enemy" allora il nemico si distruggerà
        }

        Destroy(gameObject); //distrugge QUESTO oggetto, ovvero il proiettile
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation); //crea l'effetto delle particelle distrutte del proiettile
    }

}
