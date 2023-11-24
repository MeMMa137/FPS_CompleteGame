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
    public Transform vrCamTrans;

    public float mouseSensivity;
    public bool invertX; 
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    //public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>(); //Crea una lista con tutte le armi
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun; //variabile nella quale si inserisce il numero corrispondente nella lista allGuns per switcharla 


    public Transform adsPoint, gunHolder;
    private Vector3 gunStartPos;
    public float adsSpeed = 2f;

    public GameObject muzzleFlash;

    public AudioSource footstepFast, footstepSlow;

    private float bounceAmount;
    private bool bounce;

    public float maxViewAngle = 60f;


    private void Awake() //funzione chiamata prima di start, quindi caricata 1 sola volta prima del caricamento del gioco
    {
        instance = this; //l'oggetto a cui è associato questo script (player) allora verrà inserito in "instance"
    }

    void Start()
    {
        currentGun--; //andiamo indietro di 1 nelle armi perchè nella funzione switch verrà incrementato di 1, cosa che non vogliamo all'avvio del gioco (in poche parole non cambia arma) 
        SwitchGun();

        gunStartPos = gunHolder.localPosition;
    }

    void Update()
    {

        if (!UIController.instance.pauseScreen.activeInHierarchy && !GameManager.instance.levelEnding)
        {


        //prendo velocità di y
        float yStore = moveInput.y; //prende le coordinate sul momento di y del player

        Vector3 vertMove=transform.forward * Input.GetAxis("Vertical"); //prende la posizione in cui stiamo guardando (avanti-indietro)
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal"); //prende la posizione in cui stiamo guardando (laterali)

        moveInput = horiMove + vertMove;
        moveInput.Normalize(); //si assicura che, andando in 2 direzioni contemporaneamente, non venga aumentata la velocita'
        
        //camminata e corsa
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
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


       

        //Salto
        if (OVRInput.GetDown(OVRInput.Button.One) && canJump)
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
            AudioManager.instance.PlaySXF(8);
        } 
        else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
          moveInput.y = jumpPower;
          canDoubleJump = false;
          AudioManager.instance.PlaySXF(8);
        }

            if (bounce) //evita di creare problemi nel caso il player saltasse sulla bouncepad
            {
                bounce = false;
                moveInput.y = bounceAmount;

                canDoubleJump = true;
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

        float dx = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z); //legge i parametri per la rotazione, adattandoli e inserendoli nella variabile
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x + dx, transform.rotation.eulerAngles.z); //legge i parametri per la rotazione, adattandoli e inserendoli nella variabile

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f)); //sposta la visuale sull'asse y

        if(camTrans.rotation.eulerAngles.x > maxViewAngle && camTrans.rotation.eulerAngles.x < 180f)
        {
            camTrans.rotation = Quaternion.Euler(maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
        }
        else if(camTrans.rotation.eulerAngles.x > 180f && camTrans.rotation.eulerAngles.x < 360f - maxViewAngle)
        {
            camTrans.rotation = Quaternion.Euler(-maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
        }

        muzzleFlash.SetActive(false);

        //Shooting
        Transform ct = vrCamTrans;// camTrans;
        //singolo colpo
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && activeGun.fireCounter <= 0) //mouse sinistro premuto, e limita il numero di colpi
        {
            Debug.Log("premto tasto 1");
            RaycastHit hit; //linea immaginaria
            if(Physics.Raycast(ct.position, ct.forward, out hit, 50f)) //se la linea immaginaria (raycast), che va dalla posizione della videocamera fino a 50 unità verso il punto in cui sta gaurdando, colpisce un oggetto-
                                                                                   //allora il punto verrà salvato in "hit", e if=true
            {
                if(Vector3.Distance(ct.position, hit.point) > 2) //se il player si trova abbastanza lontano da un oggetto (distanza tra player e punto di sparo > 2)
                {
                    firePoint.LookAt(hit.point); //il colpo si direzionerà verso il punto di "hit"
                }
                
            }
            else
            {
                firePoint.LookAt(ct.position + (ct.forward * 30f)); //Se il raycast non individua nessun oggetto, e si spara, allora continuerà verso la direzione della videocamera, dando l'impressione di andare dritto
            }

            //Instantiate(bullet, firePoint.position, firePoint.rotation); //creiamo una copia del proiettile, in modo che venga sparato
            FireShot();
        }

        //ripetizioni colpi
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && activeGun.canAutoFire) //Se mouse sinistro tenuto premuto e la pistola ha l'abilità dello sparo continuo
        {
            if(activeGun.fireCounter <= 0)
            {
                FireShot();
            }
        }


        //switch armi
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) 
        {
            SwitchGun();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.ZoomIn(activeGun.zoomAmount); //quando miriamo, avvia l'effetto zoom
        }

        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        } else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
        }


        if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.ZoomOut(); //se smette di mirare, toglie effetto zoom
        }
       

        //animazione spostamento
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);

        }
    }

    public void FireShot()
    {
        if(activeGun.currentAmmo > 0) //possiamo sparare solo se abbiamo colpi
        {
            activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation); //creiamo una copia del proiettile, in modo che venga sparato

            activeGun.fireCounter = activeGun.fireRate; //il fireCounter sarà uguale al numero di secondi di attesa tra uno sparo e l'altro

            UIController.instance.ammoText.text = "COLPI: " + activeGun.currentAmmo; //aggiorna la barra di munizioni grafica

            muzzleFlash.SetActive(true);
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false); //rimuove l'arma attuale

        currentGun++; //scorre l'arma nella lista allGuns

        if(currentGun >= allGuns.Count) //se il numero della lista allGuns supera il numero effettivo delle armi (3)
        {
            currentGun = 0; //torna alla prima arma
        }
        activeGun = allGuns[currentGun]; //inserisce come arma di partenza quella scelta dalla varialine currentGun
        activeGun.gameObject.SetActive(true); //applica effettivamente l'arma attuale in gioco

        UIController.instance.ammoText.text = "COLPI: " + activeGun.currentAmmo; //aggiorna la barra di munizioni grafica

        firePoint.position = activeGun.firepoint.position; //cambia la posizione del firepoint in base all'arma attuale
    }

    public void AddGun(string gunToAdd)
    {
        //sblocca le armi
        bool gunUnlocked = false;
        if(unlockableGuns.Count > 0) //se il numero di armi ancora da sbloccare è >0
        {
            for(int i=0; i < unlockableGuns.Count; i++)//controlla ogni arma della lista
            {
                if(unlockableGuns[i].gunName == gunToAdd) 
                {
                    gunUnlocked = true;
                    allGuns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i); //rimuove l'arma dalla lista di armi ancora da sbloccare
                    i = unlockableGuns.Count; //esce dal loop
                }
            }
        }

        if (gunUnlocked) //cambia l'arma al player in base a quella sbloccata
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

    public void Bounce(float bounceForce)
    {
        bounceAmount = bounceForce;
        bounce = true;
    }

}
