using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{

    public static PlayerController instance; //variabile con inserito l'oggetto, che rimarrà uguale tutto il gioco

    public float moveSpeed, gravityModifier, jumpPower, runSpeed=12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensivity;
    public bool invertX; 
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;

    private void Awake() //funzione chiamata prima di start, quindi caricata 1 sola volta prima del caricamento del gioco
    {
        instance = this; //l'oggetto a cui è associato questo script (player) allora verrà inserito in "instance"
    }

    void Start()
    {
        
    }

    void Update()
    {
        /*moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;*/

        //prendo velocità di y
        float yStore = moveInput.y; //prende le coordinate sul momento di y del player

        Vector3 vertMove=transform.forward * Input.GetAxis("Vertical"); //prende la posizione in cui stiamo guardando (avanti-indietro)
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal"); //prende la posizione in cui stiamo guardando (laterali)

        moveInput = horiMove + vertMove;
        moveInput.Normalize(); //si assicura che, andando in 2 direzioni contemporaneamente, non venga aumentata la velocita'
        
        //camminata e corsa
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore; 

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime; //fisica

        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime; //applicato per evitare di "respawnare" ad un punto precedente
        }


        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0; //verifica se la sfera del ground tocca un oggetto, se viene toccata allora ne conterà uno, .length rileva se è maggiore di 0 (quindi toccata), allora la variabile = true


       //riga di salto mancante, possibile problema

        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        } else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }




        charCon.Move(moveInput * Time.deltaTime); //si sposta

        //controllo rotazione camera
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z); //legge i parametri per la rotazione, adattandoli e inserendoli nella variabile


        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f)); //sposta la visuale sull'asse y


        //Shooting
        if (Input.GetMouseButtonDown(0)) //mouse sinistro
        {
            RaycastHit hit; //linea immaginaria
            if(Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f)) //se la linea immaginaria (raycast), che va dalla posizione della videocamera fino a 50 unità verso il punto in cui sta gaurdando, colpisce un oggetto-
                                                                                   //allora il punto verrà salvato in "hit", e if=true
            {
                if(Vector3.Distance(camTrans.position, hit.point) > 2) //se il player si trova abbastanza lontano da un oggetto (distanza tra player e punto di sparo > 2)
                {
                    firePoint.LookAt(hit.point); //il colpo si direzionerà verso il punto di "hit"
                }
                
            }
            else
            {
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f)); //Se il raycast non individua nessun oggetto, e si spara, allora continuerà verso la direzione della videocamera, dando l'impressione di andare dritto
            }

            Instantiate(bullet, firePoint.position, firePoint.rotation); //creiamo una copia del proiettile, in modo che venga sparato
        }

        //animazione spostamento
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);
    }
}
