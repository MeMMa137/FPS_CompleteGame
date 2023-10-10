using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB; // RB = rigid Body

    public GameObject impactEffect;

    public int damage = 1;

    public bool damageEnemy, damagePlayer;

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
        if(other.gameObject.tag == "Enemy" && damageEnemy) //se il proiettile colpisce un oggetto di tipo "Enemy" e ha abilitata la funzione di "damageEnemy"
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage); //allora sarà chiamata la funzione per togliere vita al nemico (DamageEnemy) nella classe "EnemyHealthController"
        }

        if(other.gameObject.tag == "Headshot" && damageEnemy) //Se colpisce la testa del nemico
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2); //il colpo avrà il doppio del danno
            Debug.Log("colpito in testaaaahhh");
        }

        if(other.gameObject.tag == "Player" && damagePlayer) //se il proiettile colpisce un oggetto di tipo "Player" e ha abilitata la funzione di "damagePlayer"
        {
            //Debug.Log("sta colpendo il giocatore a " + transform.position);
            PlayerHealthController.instance.DamagePlayer(damage); //farà danno al player
        }

        Destroy(gameObject); //distrugge QUESTO oggetto, ovvero il proiettile
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation); //crea l'effetto delle particelle distrutte del proiettile
    }

}
