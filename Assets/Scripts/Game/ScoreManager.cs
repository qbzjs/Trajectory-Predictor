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
    
    public float targetAccuracyKin;
    public List<float> trialAccuracyKin = new List<float>();
    public float totalAccuracyKin;
    public float accuracyKin;
    
    public float targetAccuracyBCI;
    public List<float> trialAccuracyBCI = new List<float>();
    public float totalAccuracyBCI;
    public float accuracyBCI;
    
    public delegate void ScoreAction(float accuracyKin, float accuracyBCI);
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

            if (Settings.instance.currentRunType == RunType.Kinematic){
                trialAccuracyKin.Add(targetAccuracyKin);
                targetAccuracyKin = 0;
                totalAccuracyKin = 0;
                for (int i = 0; i < trialAccuracyKin.Count; i++){
                    totalAccuracyKin = totalAccuracyKin + trialAccuracyKin[i];
                }
                accuracyKin = totalAccuracyKin / trialAccuracyKin.Count; //percentage from total and count
            }
            if (Settings.instance.currentRunType == RunType.Imagined){
                trialAccuracyBCI.Add(targetAccuracyBCI);
                targetAccuracyBCI = 0;
                totalAccuracyBCI = 0;
                for (int i = 0; i < trialAccuracyBCI.Count; i++){
                    totalAccuracyBCI = totalAccuracyBCI + trialAccuracyBCI[i];
                }
                accuracyBCI = totalAccuracyBCI / trialAccuracyBCI.Count; //percentage from total and count
            }
            if (OnScoreAction != null){
                OnScoreAction(accuracyKin, accuracyBCI);
            }
        }
    }
    #endregion

    private void Awake(){
        instance = this;
    }

    void Update(){
        if (targetActive && Settings.instance.currentRunType==RunType.Kinematic){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyKin < feedbackPercentage){
                targetAccuracyKin = feedbackPercentage;
            }
        }
        if (targetActive && Settings.instance.currentRunType==RunType.Imagined){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyBCI < feedbackPercentage){
                targetAccuracyBCI = feedbackPercentage;
            }
        }
    }

    private void LateUpdate(){
        
    }
}
