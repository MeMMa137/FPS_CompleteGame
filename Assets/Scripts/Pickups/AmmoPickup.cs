using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    private bool collected; //verifica se l'oggetto è stato preso, per evitare bug
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected) //se il player collide con l'oggetto, e non è ancora stato preso(evita bug)
        {
            PlayerController.instance.activeGun.GetAmmo(); //chiama la funzione per prendere colpi

            Destroy(gameObject); //distrugge l'oggetto della croce

            collected = true; //lo segnerà come preso

            AudioManager.instance.PlaySXF(3);

        }
    }
}
