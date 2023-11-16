using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public OVRInput.Controller controllerType;

    void Update()
    {
        if (OVRInput.IsControllerConnected(controllerType))
        {
            // Ottieni la posizione e la rotazione del controller
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(controllerType);
            Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controllerType);

            // Imposta la posizione dell'oggetto delle mani in base al controller
            transform.localPosition = controllerPosition;
            transform.localRotation = controllerRotation;
        }
    }
}

