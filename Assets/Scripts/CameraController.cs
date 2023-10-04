using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() //
    {
        transform.position = target.position; //posiziona la videocamera sul "target" (punto di riferimento) sul player, cosi da non avere problemi di rendering
        transform.rotation = target.rotation;
    }
}
