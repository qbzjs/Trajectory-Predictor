using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BCI_Controller : MonoBehaviour
{
    private TargetedMotionReference targetedMotionReference;
    
    public bool controlActive;
    public bool trialBasedControl = true; // whether bci controlled based on trials or always on(debugging)
    public bool applyToKinematics;
    
    private BCI_ControlSignal controlSignal;
    public BCI_ControlType controlType;
    
    private GameObject controlObject;
    private Rigidbody rb; //object moved by the BCI - arm will take position from this object
    
    //[Header("REFERENCES")]
    private Transform targetLeft;
    private Transform targetRight;
    private Vector3 returnPosition;
    private Handedness handSide;
    
    [Header("KINEMATICS")]
    public Transform IK_TargetLeft;
    public Transform IK_TargetRight;

    [Header("RIGIDBODY PROPERTIES")]
    public bool useGravity;
    public bool freeze;
    [Space(4)] [Range(0, 10f)] public float mass = 1;
    [Range(0, 10f)] public float drag = 0;
    
    [Range(0, 0.5f)] public float motionThreshold = 0.1f;
    
    private float dampTime = 0.3F; //overrides from motion reference
    private Vector3 v; //ref velocity for smoothdamp

    private void Awake(){
        targetedMotionReference = gameObject.GetComponent<TargetedMotionReference>();
        targetLeft = targetedMotionReference.leftHandReference;
        targetRight = targetedMotionReference.rightHandReference;
    }

    void Start()
    {
        controlSignal = BCI_ControlSignal.instance;
        controlObject = BCI_ControlManager.instance.controlObject;
        
        if (controlObject.GetComponent<Rigidbody>()){
            rb = controlObject.GetComponent<Rigidbody>();
        }
        else{
            Debug.Log("CONTROL OBJECT MISSING RIGIDBODY");
        }
        
    }
    void Update()
    {
        //rigidbody object settings
        rb.useGravity = useGravity;
        rb.isKinematic = freeze;
        rb.mass = mass;
        rb.drag = drag;

        dampTime = BCI_ControlSignal.instance.smoothDamping;
        handSide = targetedMotionReference.handSide;
        
        if (handSide == Handedness.Left){
            returnPosition = targetLeft.position;
        }

        if (handSide == Handedness.Right){
            returnPosition = targetRight.position;
        }
    }

    private void LateUpdate(){
        if (controlActive){
            if (trialBasedControl){
                TrialBasedControl();
            }
            else{
                ContinuousControl();
            }
        }

        if (applyToKinematics){
            if (handSide == Handedness.Left){
                // keep the right tracked/locked if the left is the active trial side
                IK_TargetRight.transform.position = targetedMotionReference.rightHandTarget.position;
                IK_TargetLeft.transform.position = controlObject.transform.position; // apply the left to the control object
            }
            if (handSide == Handedness.Right){
                // keep the left tracked/locked if the right is the active trial side
                IK_TargetLeft.transform.position = targetedMotionReference.leftHandTarget.position;
                IK_TargetRight.transform.position = controlObject.transform.position; // apply the left to the control object
            }
        }
    }

    private void TrialBasedControl(){
        if (targetedMotionReference.targetActive){
            if (controlType == BCI_ControlType.Velocity){
                rb.velocity = controlSignal.controlVectorRefined;
            }
            if (controlType == BCI_ControlType.ForceVelocity){
                rb.AddForce(controlSignal.controlVectorRefined.x, controlSignal.controlVectorRefined.y,controlSignal.controlVectorRefined.z);
            }
            if (controlType == BCI_ControlType.Translate){
                controlObject.transform.position += new Vector3(controlSignal.controlVectorRefined.x, controlSignal.controlVectorRefined.y, controlSignal.controlVectorRefined.z) * Time.deltaTime;
            }
        }
        else{
            //lerp back to hand reference position 
            rb.velocity = Vector3.zero;
            controlObject.transform.position = Vector3.SmoothDamp(controlObject.transform.position, returnPosition, ref v, dampTime);

        }

    }

    private void ContinuousControl(){
        if (controlType == BCI_ControlType.Velocity){
            rb.velocity = controlSignal.controlVectorRefined;
        }
    }
}
