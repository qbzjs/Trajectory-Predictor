using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour{
    public float v;
    public Vector3 previous;
    public Vector3 current;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        // previous = transform.position;
        
        v = ((transform.position - previous).magnitude) / Time.deltaTime;
        
        previous = transform.position;
    }

    void LateUpdate(){
        // current = transform.position;

        // v = ((current - previous).magnitude) / Time.deltaTime;
    }
}
