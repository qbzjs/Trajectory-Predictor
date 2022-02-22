using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

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
    public Vector3 controlVectorPredicted; //raw input signal
    public Vector3 controlVectorTarget;
    public Vector3 controlVectorAssisted; //descriminated of used predicted
    public Vector3 controlVectorRefined; //modified output signal - final
    //public Vector3 controlVector; //final output
    private Vector3 cv;


    [Header("SMOOTHING")] 
    public bool fadeSmoothing = true; 
    [Range(0,2f)]
    public float smoothDamping = 0.45F;
    private float defaultSmoothing = 0.45f;
    private float targetSmoothing = 0.45f;
    
    [Header("MODIFIERS")]
    [Range(0, 10f)]
    public float magnitudeMultiplier = 1f;
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
            controlVectorPredicted = new Vector3(x, y, z);
            //Debug.Log(controlVectorRaw);
        }
    }
    private void ArmReachControllerOnTargetVelocity(Vector3 targetVelocity){
        if (!simulateValues){
            controlVectorTarget = targetVelocity;
        }
    }

    #endregion
    
    void Awake(){
        instance = this;
    }

    private void Start(){
        defaultSmoothing = smoothDamping; //save the default value
        controlMixer = new BCI_ControlMixer();
    }

    void Update(){
        
        if (simulateValues){
            //override raw control vector
            //controlVectorRaw = new Vector3(1, 1, 1);
        }

        #region Control Assistance

        //Settings.instance.SetAssistance(controlAssistPercentage);//do this for now but change to make settings ui set control value
        
        assistance = controlAssistPercentage / 100f;
        
        //assist control as float
        ax = controlMixer.AssistControl(controlVectorPredicted.x, controlVectorTarget.x, assistance);
        ay = controlMixer.AssistControl(controlVectorPredicted.y, controlVectorTarget.y, assistance);
        az = controlMixer.AssistControl(controlVectorPredicted.z, controlVectorTarget.z, assistance);
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

    private void LateUpdate(){
        if (fadeSmoothing){
            smoothDamping = Mathf.Lerp(smoothDamping, targetSmoothing, Time.deltaTime);
        }
    }

    public void FadeSmoothing(Fade io){
        if (io == Fade.In){
            targetSmoothing = defaultSmoothing;
        }
        if (io == Fade.Out){
            targetSmoothing = 0;
        }
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