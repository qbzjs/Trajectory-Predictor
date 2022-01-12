using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCI_Controller : MonoBehaviour
{
    public bool controlActive;
    public bool simulatedValues;
    [Header("BCI Data")] 
    public Vector3 velocityPrediction;
    public Vector3 velocitySimulated;

    [Range(0,10f)]
    public float speedMultiplier = 1f;

    public Rigidbody controlObject; //object moved by the BCI - arm will take position from this object

    #region Data Subscribe
    private void OnEnable(){
        UDPClient.OnBCI_Data += UDPClientOnBCI_Data;
    }
    private void OnDisable(){
        UDPClient.OnBCI_Data -= UDPClientOnBCI_Data;
    }
    private void UDPClientOnBCI_Data(float x, float y, float z){
        velocityPrediction = new Vector3(x, y, z);
    }
    #endregion


    void Start(){
    }

    void Update(){
        if (controlActive && !simulatedValues){
            //controlObject.velocity += new Vector3(velocityPrediction.x, velocityPrediction.y, velocityPrediction.z) * speedMultiplier;
            
            velocityPrediction = velocityPrediction * speedMultiplier;
            controlObject.AddForce(velocityPrediction.x, velocityPrediction.y, velocityPrediction.z);
        }

        if (controlActive && simulatedValues){
            //controlObject.velocity += new Vector3(velocitySimulated.x, velocitySimulated.y, velocitySimulated.z) * speedMultiplier;
            velocityPrediction = velocityPrediction * speedMultiplier;
            controlObject.AddForce(velocitySimulated.x, velocitySimulated.y, velocitySimulated.z);
        }
        
        
        
        //try rigidbody velocity with time 
    }
}