using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody theRB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position); //guarda verso il player

        theRB.velocity = transform.forward * moveSpeed; //si sposterà in avanti, essendo che sta guardando verso il player, allora si muoverà in quella direzione
    }
}
