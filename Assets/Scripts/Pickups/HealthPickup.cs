using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private bool isCollected;
    public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isCollected) //se il player collide con l'oggetto
        {
            PlayerHealthController.instance.HealPlayer(healAmount); //chiama la funzione per aumentare vita

            Destroy(gameObject); //distrugge l'oggetto della croce

            AudioManager.instance.PlaySXF(5);

        }
    }
}
