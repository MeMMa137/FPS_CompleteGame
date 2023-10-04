﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovements : MonoBehaviour
{

    public bool shouldMove, shouldRotate;

    public float moveSpeed, rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            transform.position += new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime; //sposta l'oggetto
        }
    }
}
