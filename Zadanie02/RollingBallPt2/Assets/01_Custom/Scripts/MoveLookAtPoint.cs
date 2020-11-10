﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLookAtPoint : MonoBehaviour {
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y+3, playerTransform.position.z);
    }
}
