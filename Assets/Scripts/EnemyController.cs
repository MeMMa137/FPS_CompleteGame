using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody theRB;
    private bool chasing; //chi sta seguendo
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f; //distanza per seguire, distanza per non seguire più, distanza da mantenere tra player e nemico
    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;

    public float keepChasingTime = 5f; //tempo default per arrivare all'ultima posizione del player
    private float chaseCounter; //conta alla rovescia il tempo per arrivare all'ultima posizione del player



    void Start()
    {
        startPoint = transform.position;
    }


    void Update()
    {
        targetPoint = PlayerController.instance.transform.position; //targetPoint sarà la posizione del player
        targetPoint.y = transform.position.y; //targetPoint manterrà la propria altezza nonostante sia la posizione del player

        if (!chasing) //se non sta già inseguendo
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase) //se la distanza tra il nemico e il player è minore della distanza minima per poter essere seguito
            {
                chasing = true; //attiva l'inseguimento
            }

            if (chaseCounter > 0) //se tempo non scaduto
            {
                chaseCounter -= Time.deltaTime; //conta alla rovescia, 1 sec alla volta (60fps)

                if(chaseCounter <= 0) //se tempo scaduto
                 {
                   agent.destination = startPoint; //il nemico torna alla posizione di default
                 }
            }
            
        }
        else
        {
            //transform.LookAt(targetPoint); //guarda verso il player

            //theRB.velocity = transform.forward * moveSpeed; //si sposterà in avanti, essendo che sta guardando verso il player, allora si muoverà in quella direzione

            if(Vector3.Distance(transform.position, targetPoint) > distanceToStop)//se la distanza tra il nemico e il player è maggiore della distanza di "sicurezza"
            {
                agent.destination = targetPoint; //continua a seguire il player
            }
            else
            {
                agent.destination = transform.position; //si muove nella sua posizione attuale, quindi sta fermo
            }

            


            if(Vector3.Distance(transform.position, targetPoint) > distanceToChase) //se la distanza tra il nemico e il player è maggiore della distanza minima per poter essere seguito
            {
                chasing = false; // NON attiva l'inseguimento

                chaseCounter = keepChasingTime; //resetta il conto alla rovescia 
            }
        }

        
    }
}
