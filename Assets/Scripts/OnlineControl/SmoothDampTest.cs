using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampTest : MonoBehaviour
{
    public bool active;
    public Transform target;
    public float smoothTime = 0.3F;
    public Vector3 velocity = Vector3.zero;
    public Vector3 targetPosition;

    #region Data Subscribe
    private void OnEnable(){
        UDPClient.OnBCI_Data += UDPClientOnBCI_Data;
    }
    private void OnDisable(){
        UDPClient.OnBCI_Data -= UDPClientOnBCI_Data;
    }
    private void UDPClientOnBCI_Data(float x, float y, float z){
        velocity = new Vector3(x, y, z);
    }
    #endregion

    
    void Update(){
        if (BCI_ControlSignal.instance != null){
            velocity = BCI_ControlSignal.instance.controlVectorRefined;
        }

        if (active){
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        }
        
    }
    

}