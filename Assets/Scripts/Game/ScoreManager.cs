using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ScoreManager : MonoBehaviour{

    public static ScoreManager instance;
    
    public Transform trackedObject;
    public GameObject activeTarget;
    public TargetMagnitudeTracker targetMagnitudeTracker;

    public bool targetActive;

    public float feedbackPercentage;
    public float feedbackAmplitude;
    
    public float targetAccuracy;
    public List<float> trialAccuracy = new List<float>();
    public float totalAccuracy;
    public float accuracy;
    
    public delegate void ScoreAction(float accuracy);
    public static event ScoreAction OnScoreAction;
    
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
            trialAccuracy.Add(targetAccuracy);
            targetAccuracy = 0;
            totalAccuracy = 0;
            for (int i = 0; i < trialAccuracy.Count; i++){
                totalAccuracy = totalAccuracy + trialAccuracy[i];
            }
            
            // this.activeTarget = null;
            // targetMagnitudeTracker = null;
            
            //accuracy = totalAccuracy / trialAccuracy.Count; //average of all saved accuracies...

            accuracy = totalAccuracy / trialAccuracy.Count; //percentage from total and count

            if (OnScoreAction != null){
                OnScoreAction(accuracy);
            }
        }
    }
    #endregion

    private void Awake(){
        instance = this;
    }

    void Update(){
        if (targetActive){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracy < feedbackPercentage){
                targetAccuracy = feedbackPercentage;
            }
        }
    }

    private void LateUpdate(){
        
    }
}
