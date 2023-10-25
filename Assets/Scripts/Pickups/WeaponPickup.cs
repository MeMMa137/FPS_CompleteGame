using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public string theGun;
    private bool collected; //verifica se l'oggetto è stato preso, per evitare bug
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected) //se il player collide con l'oggetto, e non è ancora stato preso(evita bug)
        {
            PlayerController.instance.AddGun(theGun); //chiama la funzione per prendere l'arma presa

            Destroy(gameObject); //distrugge l'oggetto 

            collected = true; //lo segnerà come preso

            AudioManager.instance.PlaySXF(4);

        }
    }
}
