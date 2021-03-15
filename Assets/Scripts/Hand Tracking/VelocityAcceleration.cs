using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityAcceleration : MonoBehaviour
{
    //velocity
    private float velocity;
    private Vector3 previousPosition;
    
    //acceleration
    private Vector3 acceleration;
    private Vector3 distancemoved=Vector3.zero;
    private Vector3 lastdistancemoved=Vector3.zero;
    private Vector3 lastPosition;

    public float GetVelocity(){
        return velocity;
    }

    public Vector3 GetAcceleration(){
        return acceleration;
    }

    void Update(){
        //velocity
        velocity = ((transform.position - previousPosition).magnitude) / Time.deltaTime;
        previousPosition = transform.position;
        
        //acceleration
        distancemoved = (transform.position - lastPosition) * Time.deltaTime ;
        acceleration = distancemoved - lastdistancemoved;
        lastdistancemoved = distancemoved;
        lastPosition = transform.position;
    }
}
