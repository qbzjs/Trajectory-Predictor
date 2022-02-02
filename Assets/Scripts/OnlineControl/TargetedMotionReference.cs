using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Labs.SuperScience;
using UnityEngine;

public class TargetedMotionReference : MonoBehaviour
{
    public bool targetActive; 
    
    private PhysicsTracker physicsData;
    public Vector3 targetedMotionVector;
    
    public Transform leftHandReference; //reference for the tracked wrist postion
    public Transform rightHandReference;
    
    public Transform leftHandTarget; //actual motion transform - used to match with predicted
    public Transform rightHandTarget;
    
    public Vector3 targetPosition;
    public Vector3 homePosition;

    public float trialPhaseLifetime;

    public Handedness handSide;

    //smooth damp
    public Transform lerpTarget;
    public Vector3 lerpTargetOffset;
    private float dampTime = 0.45F;
    private Vector3 v; //ref velocity for smoothdamp

    //EVENTS/DELAGATES TO SEND DATA ACROSS THE GAME
    public delegate void TargetVelocity(Vector3 targetVelocity);
    public static event TargetVelocity OnTargetVelocity;
    
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
        trialPhaseLifetime = lifetime;
        if (eventType == TrialEventType.TargetPresentation){
            //move to target
        }
        if (eventType == TrialEventType.Rest){
            //move to home
        }
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 position, Transform activeTarget){
        if (targetPresent){
            targetActive = true;
            targetPosition = position + lerpTargetOffset;
        }
        //most likely to be back to tracker position?
        if (restPresent){
            targetActive = false;
            homePosition = position;
  //          targetPosition = homePosition;
        }
    }
    #endregion

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

        dampTime = BCI_ControlSignal.instance.smoothDamping;

        //if imagined - take target vector from the reference IK rig hands
        if (Settings.instance.currentRunType == RunType.Imagined){
            if (handSide == Handedness.Left){
                leftHandTarget.position = Vector3.SmoothDamp(leftHandTarget.position, lerpTarget.position, ref v, dampTime);
                //get a tracked velocity
                physicsData = leftHandTarget.gameObject.GetComponent<PhysicsData>().m_MotionData;
                targetedMotionVector = physicsData.Velocity;
            }

            if (handSide == Handedness.Right){
                rightHandTarget.position = Vector3.SmoothDamp(rightHandTarget.position, lerpTarget.position, ref v, dampTime);
                //get a tracked velocity
                physicsData = rightHandTarget.gameObject.GetComponent<PhysicsData>().m_MotionData;
                targetedMotionVector = physicsData.Velocity;
            }
        }
        //if kinematic - take target vector from the tracked wrist
        if (Settings.instance.currentRunType == RunType.Kinematic){
            targetedMotionVector = DAO.instance.MotionData_ActiveWrist.velocity; // velocity of tracked wrist
        }


        //send the target velocity from arm (wrist tracked position to target position)
        if (OnTargetVelocity != null){
            OnTargetVelocity(targetedMotionVector);
        }
        
        
    }

}
