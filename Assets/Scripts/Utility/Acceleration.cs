using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    public Vector3 acceleration;
    public Vector3 distancemoved=Vector3.zero;
    public Vector3 lastdistancemoved=Vector3.zero;
    public Vector3 last;
    
    void Start()
    {
        last = transform.position;
    }


    void Update(){   
        distancemoved = (transform.position - last) * Time.deltaTime ;
        acceleration = distancemoved - lastdistancemoved;
        lastdistancemoved = distancemoved;
        last = transform.position;
    }
}
