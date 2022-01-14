using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BCI_ControlSignal : MonoBehaviour
{
    public static BCI_ControlSignal instance;

    [Header("BCI CONTROL")] public bool controlActive;
    public bool simulatedValues;
    public BCI_ControlType controlType;

    [Header("MODIFIERS")] [Range(0, 2f)] public float magnitudeMultiplier = 1f;

    public bool invertX;
    [Range(0, 2f)] public float magnitudeMultiplierX = 1f;
    public bool invertY;
    [Range(0, 2f)] public float magnitudeMultiplierY = 1f;
    public bool invertZ;
    [Range(0, 2f)] public float magnitudeMultiplierZ = 1f;

    public bool useGravity;
    public bool freeze;
    [Space(4)] [Range(0, 10f)] public float mass = 1;
    [Range(0, 10f)] public float drag = 0;

    [Header("BCI DATA")] public Vector3 controlVectorRaw;
    public Vector3 controlVectorRefined;
    private Vector3 cv;


    private Rigidbody controlObject; //object moved by the BCI - arm will take position from this object

    #region Data Subscribe

    private void OnEnable(){
        UDPClient.OnBCI_Data += UDPClientOnBCI_Data;
    }

    private void OnDisable(){
        UDPClient.OnBCI_Data -= UDPClientOnBCI_Data;
    }

    private void UDPClientOnBCI_Data(float x, float y, float z){
        if (!simulatedValues && controlActive){
            controlVectorRaw = new Vector3(x, y, z);
        }
    }

    #endregion


    void Awake(){
        instance = this;
        if (gameObject.GetComponent<Rigidbody>()){
            controlObject = gameObject.GetComponent<Rigidbody>();
        }
        else{
            controlObject = transform.Find("BCI Control Object").GetComponent<Rigidbody>();
        }

        if (controlObject == null){
            Debug.Log("BCI SIGNAL MISSING CONTROL OBJECT (RIGIDBODY)");
        }
    }

    void Update(){
        if (controlActive){
            if (simulatedValues){
                //can change raw vector in inspector
                //get control vector from other location...
                // controlVector = ....
            }

            controlObject.useGravity = useGravity;
            controlObject.isKinematic = freeze;
            controlObject.mass = mass;
            controlObject.drag = drag;

            if (invertX){
                magnitudeMultiplierX = -magnitudeMultiplierX;
            }

            if (invertY){
                magnitudeMultiplierY = -magnitudeMultiplierY;
            }

            if (invertZ){
                magnitudeMultiplierZ = -magnitudeMultiplierZ;
            }


            if (controlType == BCI_ControlType.Velocity){
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

                controlObject.velocity += new Vector3(cv.x, cv.y, cv.z);
            }

            if (controlType == BCI_ControlType.ForceVelocity){
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

                controlObject.AddForce(cv.x, cv.y, cv.z);
            }

            if (controlType == BCI_ControlType.Position){
                //controlObject.transform.Translate(cv.x,cv.y,cv.z);
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

                controlObject.transform.position = cv;
            }

            controlVectorRefined = cv;
        }
    }
}