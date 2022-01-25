using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ScoreTracker : MonoBehaviour
{

    public Transform trackedObject;
    public GameObject activeTarget;
    public TargetMagnitudeTracker targetMagnitudeTracker;

    public bool targetActive;

    public float feedbackPercentage;
    public float feedbackAmplitude;
    
    #region Subscriptions
    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject += TrackedObjectReferenceOnTrackedObject;
    }
    private void OnDisable(){
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject -= TrackedObjectReferenceOnTrackedObject;
    }
    private void TrackedObjectReferenceOnTrackedObject(Transform trackedObject){
        this.trackedObject = trackedObject;
    }
    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifetime, int index, int total){
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
            this.activeTarget = activeTarget.gameObject;
            targetMagnitudeTracker = this.activeTarget.GetComponent<TargetMagnitudeTracker>();
        }
        if (restPresent){
            targetActive = false;
            // this.activeTarget = null;
            // targetMagnitudeTracker = null;
        }
    }
    #endregion
    
    void Start()
    {
        
    }
    void Update()
    {
        // if (targetActive){
        //     if (Settings.instance.currentRunType == RunType.Kinematic){
        //         if (Settings.instance.handedness == Handedness.Left){
        //             targetMagnitudeTracker.TrackMagnitude(leftHandTracker, RunType.Kinematic);
        //         }
        //         else{
        //             targetMagnitudeTracker.TrackMagnitude(rightHandTracker, RunType.Kinematic);
        //             
        //         }
        //     }
        //     if (Settings.instance.currentRunType == RunType.Imagined){
        //         targetMagnitudeTracker.TrackMagnitude(controlObjectBCI.transform, RunType.Imagined);
        //         
        //     }

        // if(targetActive){
        //     targetMagnitudeTracker.TrackMagnitude(trackedObject);
        //     feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
        //     feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
        // }

    }
}
