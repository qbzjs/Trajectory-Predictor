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
    
    [Range(0, 0.5f)] public float motionThreshold = 0.1f;
    
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

    public bool brakes = false;

    public Rigidbody controlObject; //object moved by the BCI - arm will take position from this object

    [Range(0, 1f)] public float assistance;
    public Vector3 targetVector;
    public Vector3 controlVectorDiscriminated; //percentage of used predicted
    private BCI_ControlMixer controlMixer;
    
    #region Data Subscribe

    private void OnEnable(){
        UDPClient.OnBCI_Data += UDPClientOnBCI_Data;
    }

    private void OnDisable(){
        UDPClient.OnBCI_Data -= UDPClientOnBCI_Data;
    }

    private void UDPClientOnBCI_Data(float x, float y, float z){
        if (!simulatedValues){
            controlVectorRaw = new Vector3(x, y, z);
        }
    }

    #endregion


    void Awake(){
        instance = this;
    }

    private void Start(){
        controlObject = BCI_ControlManager.instance.controlObject;
        controlMixer = new BCI_ControlMixer();
    }

    void Update(){
        float x = controlMixer.DiscriminateAxis(controlVectorRaw.x, targetVector.x, assistance);
        float y = controlMixer.DiscriminateAxis(controlVectorRaw.y, targetVector.y, assistance);
        float z = controlMixer.DiscriminateAxis(controlVectorRaw.z, targetVector.z, assistance);

        controlVectorDiscriminated = new Vector3(x, y, z);

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

            if (controlType == BCI_ControlType.Translate){
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
                
                controlObject.transform.position += new Vector3(cv.x,cv.y,cv.x);
            }

            if (controlType == BCI_ControlType.Velocity){
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

                if (!brakes){
                    controlObject.velocity += new Vector3(cv.x, cv.y, cv.z);
                }
                else{
                    //controlObject.velocity = controlObject.velocity * 0.9f;
                    //controlObject.velocity = Vector3.zero;
                    controlObject.velocity -= new Vector3(cv.x, cv.y, cv.z) * 2;
                }
                
            }

            if (controlType == BCI_ControlType.ForceVelocity){
                cv = controlVectorRaw * magnitudeMultiplier; //modify the vector
                cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

                if (!brakes){
                    controlObject.AddForce(cv.x, cv.y, cv.z);
                }
                else{
                    controlObject.AddForce(-cv.x*2, -cv.y*2, -cv.z*2);
                }
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