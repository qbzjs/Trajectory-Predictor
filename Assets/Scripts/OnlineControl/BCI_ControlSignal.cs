using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// TODO
/// - ADD BASELINE CORRECTION
/// - CHECK BASELINE MULTIPLIERS
/// - BASELINE CORRECTION ROUNDING OR ....RESET BUTTON IN SETTINGS
/// </summary>
public class BCI_ControlSignal : MonoBehaviour
{
    public static BCI_ControlSignal instance;

    private BCI_ControlMixer controlMixer;
    
    [Header("BCI CONTROL")] 
    public bool simulateValues;

    [Space(4)]
    [Range(0, 100)] 
    public int controlAssistPercentage; // silder between 0-1 for percentage of used bci signal control

    [Range(0, 100)] 
    public int assistanceModifier = 0; //percentage or assistance to remove per run
    
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
    //[HideInInspector]
    public float defaultSmoothing = 0.45f;
    public float targetSmoothing = 0.45f;
    
    [Header("BASELINE CORRECTION")]
    [Header("Offset")]
    [Range(-1f, 1f)] public float baselineOffsetX = 0f;
    [Range(-1f, 1f)] public float baselineOffsetY = 0f;
    [Range(-1f, 1f)] public float baselineOffsetZ = 0f;
    [Header("Magnitude")]
    [Range(0, 10f)]
    public float magnitudeMultiplier = 1f;
    [Space(4)]
    [Range(-5f, 5f)] public float magnitudeMultiplierX = 1f;
    [Range(-5, 5f)] public float magnitudeMultiplierY = 1f;
    [Range(-5, 5f)] public float magnitudeMultiplierZ = 1f;

    //todo - save baseline correction to settings

    #region Event Subscriptions

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
            Debug.Log("BCI: " +controlVectorPredicted);
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
        controlAssistPercentage = Mathf.RoundToInt(Settings.instance.BCI_ControlAssistance);
        smoothDamping = Settings.instance.smoothingSpeed;
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

        #region Baseline Correction

        //invert per axis - TODO CHECK THIS WORKS!!!....
        // magnitudeMultiplierX = -magnitudeMultiplierX;
        // magnitudeMultiplierY = -magnitudeMultiplierY;
        // magnitudeMultiplierZ = -magnitudeMultiplierZ;

        //TODO - add baseline offset here??
        
        //scale the control magnitude on all axis
        cv = controlVectorAssisted * magnitudeMultiplier;
        
        //scale the control magnitude on individual axis
        cv = new Vector3(cv.x * magnitudeMultiplierX, cv.y * magnitudeMultiplierY, cv.z * magnitudeMultiplierZ);

        cv = new Vector3(cv.x + baselineOffsetX, cv.y + baselineOffsetY, cv.z + baselineOffsetZ);
        

        #endregion
        
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