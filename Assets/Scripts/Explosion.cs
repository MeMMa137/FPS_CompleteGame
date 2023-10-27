using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public int damage = 25;

    public bool damageEnemy, damagePlayer;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) //quando l'oggetto associato a questo script colliderà con un altro oggetto, allora si attiverà la funizone
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy) //se il proiettile colpisce un oggetto di tipo "Enemy" e ha abilitata la funzione di "damageEnemy"
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage); //allora sarà chiamata la funzione per togliere vita al nemico (DamageEnemy) nella classe "EnemyHealthController"
        }


        if (other.gameObject.tag == "Player" && damagePlayer) //se il proiettile colpisce un oggetto di tipo "Player" e ha abilitata la funzione di "damagePlayer"
        {
            //Debug.Log("sta colpendo il giocatore a " + transform.position);
            PlayerHealthController.instance.DamagePlayer(damage); //farà danno al player
        }

        
    }

}
