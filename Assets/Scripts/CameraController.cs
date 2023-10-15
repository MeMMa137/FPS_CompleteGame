using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;

    private float startFOV, targetFOV; //startFOV=campo visivo di default, targetFOV=zoom del campo visivo
    public float zoomSpeed = 1f;

    public Camera theCam;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startFOV = theCam.fieldOfView; //imposta campo visivo iniziale
        targetFOV = startFOV; //targetFOV sarà 0?, ovvero lo zoom
    }

    // Update is called once per frame
    void LateUpdate() //
    {
        transform.position = target.position; //posiziona la videocamera sul "target" (punto di riferimento) sul player, cosi da non avere problemi di rendering
        transform.rotation = target.rotation;

        theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, targetFOV, zoomSpeed*Time.deltaTime); //fieldOfView = campo visivo, qui andiamo ad avvicinare in modo graduale (rallentando sempre di piu) il campo visivo, effetto zoom
    }

    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom; //viene effettuato lo zoom in base all'arma
    }

    public void ZoomOut()
    {

        targetFOV = startFOV; //lo zoom torna al valore iniziale
    }

}
