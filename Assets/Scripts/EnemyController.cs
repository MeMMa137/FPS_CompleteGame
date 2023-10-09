using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    
    private bool chasing; //chi sta seguendo
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f; //distanza per seguire, distanza per non seguire più, distanza da mantenere tra player e nemico
    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;

    public float keepChasingTime = 5f; //tempo default per arrivare all'ultima posizione del player
    private float chaseCounter; //conta alla rovescia il tempo per arrivare all'ultima posizione del player

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots=2f, timeToShoot=1f; //tempo tra un colpo e l'altro del nemico statico di default, conto alla rovescia sul tempo disponibile per sparare (tra un colpo e l'altro) statico di default, tempo statico di default per "ricaricare" colpi
    private float fireCount, shotWaitCounter, shootTimeCounter;//tempo tra un colpo e l'altro del nemico, conto alla rovescia sul tempo disponibile per sparare (tra un colpo e l'altro), tempo per "ricaricare" colpi


    void Start()
    {
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
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

                shootTimeCounter = timeToShoot; //resetta il tempo di ricarica colpi
                shotWaitCounter = waitBetweenShots;//resetta l'intervallo tra spari
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

            if(shotWaitCounter > 0) //se si trova quindi nel range partirà lo shotWaitCounter
            {
                shotWaitCounter -= Time.deltaTime; //inizia il conto alla rovescia togliendo secondi all'intervallo tra un colpo e l'altro

                if (shotWaitCounter <= 0) //se il tempo di intervallo per sparare finisce
                {
                    shootTimeCounter = timeToShoot;//si resetta il valore del tempo
                }
            }
            else
            {

            

            shootTimeCounter -= Time.deltaTime;

            if(shootTimeCounter > 0)
            {

            

            fireCount -= Time.deltaTime; //abbassa di 1 al sec il count (su 60fps)
            if (fireCount <= 0) //intervallo per sparare finito
            {
                fireCount = fireRate; //resetta il count

                firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.2f, 0f)); //sposta la visuale del nemico leggermente piu in alto, perche di base guarderebbe il blocco collegato al ground

                        //controlla l'angolo tra la visione del nemico e la posizione del player
                        Vector3 targetDir = PlayerController.instance.transform.position - transform.position; //sottrae la posizione del giocatore a quella del nemico
                        float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);//calcola l'angolo (prende la posizione del player e la sua, ed indica l'asse, calcolandone la differenza)

                        if(Mathf.Abs(angle) < 30f) //se l'angolo tra il player e il nemico è minore di 30 gradi (Abs = anche se negativo, viene percepito come positivo)
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation); //crea una copia del proiettile nella pistola
                        }
                        else
                        {
                            shotWaitCounter = waitBetweenShots; //aspetta a sparare
                        }

                        
            }

                    agent.destination = transform.position; //quando sta sparando, il nemico non si muoverà

            }
            else
            {
                shotWaitCounter = waitBetweenShots;
            }

           }

        }

        
    }
}
