﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") //se il player collide con l'oggetto
        {
            PlayerHealthController.instance.HealPlayer(healAmount); //chiama la funzione per aumentare vita

            Destroy(gameObject); //distrugge l'oggetto della croce
        }
    }
}
