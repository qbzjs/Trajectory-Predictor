using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using RootMotion.FinalIK;
using UnityEngine;

public class ArmReachController : MonoBehaviour
{
    public bool targetActive; 
    
    public VRIK kinematicVR;
    
    public Transform leftHandReference;
    public Transform rightHandReference;
    
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    
    
    public Vector3 targetPosition;
    public Vector3 homePosition;
    public Vector3 leftTrackerPosition;
    public Vector3 rightTrackerPosition;
    public Vector3 leftTrackerRotation;
    public Vector3 rightTrackerRotation;

    public float lerpDuration;

    public Handedness handSide;

    //smooth damp
    public Transform lerpTarget;
    public Vector3 lerpTargetOffset;
    [Range(0,2f)]
    public float smoothTimeDefault = 0.3F;
    private float smoothTimeVariable;
    private Vector3 velocity = Vector3.zero;
    
    #region Subscriptions
    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
    }
    private void OnDisable(){
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
    }
    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifetime, int index, int total){
        lerpDuration = lifetime;
        if (eventType == TrialEventType.TargetPresentation){
            //move to target
        }
        if (eventType == TrialEventType.Rest){
            //move to home
        }
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 position){
        if (targetPresent){
            targetActive = true;
            //targetPosition = position;
            targetPosition = position + lerpTargetOffset;
            // targetPosition = new Vector3(position.x + )
//            target.position = targetPosition;
        }
        //most likely to be back to tracker position?
        if (restPresent){
            targetActive = false;
            homePosition = position;
  //          targetPosition = homePosition;
        }
    }
    #endregion
    
    void Start()
    {
        
    }
    

    
    void Update(){
        // needs injected from settings...
        handSide = Settings.instance.handedness;

        rightHandTarget.rotation = rightHandReference.rotation;
        leftHandTarget.rotation = leftHandReference.rotation;
        
        if (handSide == Handedness.Left){
            // keep the right tracked/locked if the left is the active trial side
            rightHandTarget.position = rightHandReference.position;
            rightHandTarget.rotation = rightHandReference.rotation;
        }
        if (handSide == Handedness.Right){
            // keep the left tracked/locked if the right is the active trial side
            leftHandTarget.position = leftHandReference.position;
            leftHandTarget.rotation = leftHandReference.rotation;
        }
        
        // set the lerp target position when a trial target is active
        if (targetActive){
            lerpTarget.position = targetPosition;
        }
        // reset the lerp target to hand tracked position
        if (!targetActive){
            if (handSide == Handedness.Left){
                lerpTarget.position = leftHandReference.position;
            }
            if (handSide == Handedness.Right){
                lerpTarget.position = rightHandReference.position;
            }
        }

        //inject velocity from BCI
        if (BCI_ControlSignal.instance != null){
            if (BCI_ControlSignal.instance.controlActive){
                velocity = BCI_ControlSignal.instance.controlVectorRefined;
            }
        }

        if (handSide == Handedness.Left){
            leftHandTarget.position = Vector3.SmoothDamp(leftHandTarget.position, lerpTarget.position, ref velocity, smoothTimeDefault);
        }

        if (handSide == Handedness.Right){
            rightHandTarget.position = Vector3.SmoothDamp(rightHandTarget.position, lerpTarget.position, ref velocity, smoothTimeDefault);
        }
        
        
    }

}
