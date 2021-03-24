using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionMath : MonoBehaviour
{
    //velocity
    private float velocity;
    private Vector3 previousPosition;
    private float vRef;
    
    //acceleration
    private Vector3 acceleration;
    private float averageAcceleration;
    private Vector3 distancemoved=Vector3.zero;
    private Vector3 lastdistancemoved=Vector3.zero;
    private Vector3 lastPosition;

    //angular
    private Vector3 angularAcceleration;

    public bool debug = false;

    public float GetVelocity(){
        return velocity;
    }

    public Vector3 GetAcceleration(){
        return acceleration;
    }
    public float GetAccelerationAverage(){
        //averageAcceleration = (acceleration.x + acceleration.y + acceleration.z) / 3;
        acceleration = acceleration * 25;
        return (acceleration.x + acceleration.y + acceleration.z) / 3;
    }
    public Vector3 GetAngularAcceleration()
    {
        return angularAcceleration;
    }
    public float GetAngularAccelerationAverage()
    {
        //averageAcceleration = (acceleration.x + acceleration.y + acceleration.z) / 3;
        return (angularAcceleration.x + angularAcceleration.y + angularAcceleration.z) / 3;
    }

    void Update(){
        //velocity
        previousPosition = transform.position;
        // previousPosition = transform.position;

        //acceleration
        // distancemoved = (transform.position - lastPosition) * (Time.deltaTime *5);
        // acceleration = distancemoved - lastdistancemoved;
        // lastdistancemoved = distancemoved;
        // lastPosition = transform.position;

        
        Math3D.LinearAcceleration(out acceleration, transform.position,25);
        //averageAcceleration = (acceleration.x + acceleration.y + acceleration.z) / 3;

        // Math3D.AngularAcceleration(out angularAcceleration, transform.rotation, 50);
        
        if (debug){
            Debug.Log("V : " + velocity + " : " + " A : " + acceleration);
        }
    }

    private void LateUpdate(){
        
        velocity = ((transform.position - previousPosition).magnitude) / Time.deltaTime;
    }
}
