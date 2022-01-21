using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BCI_ControlSignal : MonoBehaviour
{
    public static BCI_ControlSignal instance;

    private BCI_ControlMixer controlMixer;
    
    [Header("BCI CONTROL")] 
    public bool simulateValues;
    
    [Space(4)]
    [Range(0, 100)] 
    public int controlAssistPercentage; // silder between 0-1 for percentage of used bci signal control
    private float assistance; // between 0-1 for percentage of used bci signal control
    private float ax;
    private float ay;
    private float az;

    [Header("BCI DATA")] 
    public Vector3 controlVectorRaw; //raw input signal
    public Vector3 targetVector;
    public Vector3 controlVectorAssisted; //descriminated of used predicted
    public Vector3 controlVectorRefined; //final output signal
    private Vector3 cv;
    
    [Header("MODIFIERS")]
    [Range(0,2f)]
    public float smoothDamping = 0.3F;
    [Range(0, 10f)] public float magnitudeMultiplier = 1f;
    [Space(4)]
    public bool invertX;
    [Range(0, 2f)] public float magnitudeMultiplierX = 1f;
    public bool invertY;
    [Range(0, 2f)] public float magnitudeMultiplierY = 1f;
    public bool invertZ;
    [Range(0, 2f)] public float magnitudeMultiplierZ = 1f;

    #region Data Subscribe

    private void OnEnable(){
        UDPClient.OnBCI_Data += UDPClientOnBCI_Data;
        TargetedMotionReference.OnTargetVelocity += ArmReachControllerOnTargetVelocity;
    }
    private void OnDisable(){
        UDPClient.OnBCI_Data -= UDPClientOnBCI_Data;
        TargetedMotionReference.OnTargetVelocity -= ArmReachControllerOnTargetVelocity;
    }
    private void UDPClientOnBCI_Data(float x, float y, float z){
        if (!simulateValues){
            controlVectorRaw = new Vector3(x, y, z);
        }
    }
    private void ArmReachControllerOnTargetVelocity(Vector3 targetVelocity){
        if (!simulateValues){
            targetVector = targetVelocity;
        }
    }

    #endregion
    
    void Awake(){
        instance = this;
    }

    private void Start(){
        controlMixer = new BCI_ControlMixer();
    }

    void Update(){
        
        if (simulateValues){
            //override raw control vector
            //controlVectorRaw = new Vector3(1, 1, 1);
        }
        
        #region Control Assistance

        Settings.instance.SetAssistance(controlAssistPercentage);//do this for now but change to make settings ui set control value
        
        assistance = controlAssistPercentage / 100f;
        
        //assist control as float
        ax = controlMixer.AssistControl(controlVectorRaw.x, targetVector.x, assistance);
        ay = controlMixer.AssistControl(controlVectorRaw.y, targetVector.y, assistance);
        az = controlMixer.AssistControl(controlVectorRaw.z, targetVector.z, assistance);
        controlVectorAssisted = new Vector3(ax, ay, az);

        //assist control as a vector
        //controlVectorAssisted = controlMixer.AssistControl(controlVectorRaw, targetVector, assistance);
        #endregion

        //invert per axis - TODO CHECK THIS WORKS!!!
        if (invertX){
            magnitudeMultiplierX = -magnitudeMultiplierX;
        }
        if (invertY){
            magnitudeMultiplierY = -magnitudeMultiplierY;
        }
        if (invertZ){
            magnitudeMultiplierZ = -magnitudeMultiplierZ;
        }
        
        //scale the control magnitude
        cv = controlVectorAssisted * magnitudeMultiplier;
        cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
        
        //assign control to a vector for use in other classes 
        controlVectorRefined = cv;
        
    }

    //DEPRECIATED CONTROL METHODS
    // private void LateUpdate(){
    //     if (controlType == BCI_ControlType.Translate){
    //         cv = controlVectorAssisted * magnitudeMultiplier; //modify the vector
    //         cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
    //         
    //         controlObject.transform.position += new Vector3(cv.x,cv.y,cv.x);
    //     }
    //
    //     if (controlType == BCI_ControlType.Velocity){
    //         cv = controlVectorAssisted * magnitudeMultiplier; //modify the vector
    //         cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
    //
    //         if (!brakes){
    //             controlObject.velocity += new Vector3(cv.x, cv.y, cv.z);
    //         }
    //         else{
    //             controlObject.velocity -= new Vector3(cv.x, cv.y, cv.z) * 2;
    //         }
    //         
    //     }
    //
    //     if (controlType == BCI_ControlType.ForceVelocity){
    //         cv = controlVectorAssisted * magnitudeMultiplier; //modify the vector
    //         cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
    //
    //         if (!brakes){
    //             controlObject.AddForce(cv.x, cv.y, cv.z);
    //         }
    //         else{
    //             controlObject.AddForce(-cv.x*2, -cv.y*2, -cv.z*2);
    //         }
    //     }
    //
    //     if (controlType == BCI_ControlType.Position){
    //         //controlObject.transform.Translate(cv.x,cv.y,cv.z);
    //         cv = controlVectorAssisted * magnitudeMultiplier; //modify the vector
    //         cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);
    //
    //         controlObject.transform.position = cv;
    //     }
    // }
}