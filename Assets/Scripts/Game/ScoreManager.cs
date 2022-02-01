using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

public class ScoreManager : MonoBehaviour{

    public static ScoreManager instance;

    public ScoreDataObject scoreData;
    
    [Header("DISTANCE ACCURACY")]
    public Transform trackedObject;
    public GameObject activeTarget;
    public TargetMagnitudeTracker targetMagnitudeTracker;

    public bool targetActive;

    public float feedbackPercentage;
    public float feedbackAmplitude;
    
    public float targetAccuracyDistanceKin;
    public List<float> trialAccuracyDistanceKin = new List<float>();
    public float totalAccuracyDistanceKin;
    public float accuracyDistanceKin;
    
    public float targetAccuracyDistanceBCI;
    public List<float> trialAccuracyDistanceBCI = new List<float>();
    public float totalAccuracyDistanceBCI;
    public float accuracyDistanceBCI;

    [Header("DIFFERENTIAL ACCURACY")]
    public Vector3 vectorPredicted;
    public Vector3 vectorTarget;
    public Vector3 vectorAssisted;
    public Vector3 vectorRefined;

    [Space(4)]
    public Vector3 predictedTargetedDifference;
    public Vector3 predictedTargetedDifferenceAccumulated;
    
    [Space(4)]
    public float targetAccuracyDifferenceKin;
    public List<float> trialAccuracyDifferenceKin = new List<float>();
    public float totalAccuracyDifferenceKin;
    public float accuracyDifferenceKin;
    
    public Vector3 targetAccuracyDifferenceBCI;
    public List<Vector3> trialAccuracyDifferenceBCI = new List<Vector3>();
    public Vector3 totalAccuracyDifferenceBCI;
    public Vector3 accuracyDifferenceAverageBCI;
    public Vector3 highestDifference;
    public Vector3 accuracyDifferencePercentageBCI;
    
    [Header("MEAN SQUARE")]
    public Vector3 meanSqError;
    public Vector3 meanSqErrorAverage;
    public int frameAccumulation;
    
    public delegate void ScoreAction(float accuracyKin, float accuracyBCI);
    public static event ScoreAction OnScoreAction;
    
    #region Subscriptions
    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject += TrackedObjectReferenceOnTrackedObject;
        BCI_ControlManager.OnControlSignal += BCI_ControlManagerOnControlSignal;
    }
    private void OnDisable(){
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject -= TrackedObjectReferenceOnTrackedObject;
        BCI_ControlManager.OnControlSignal -= BCI_ControlManagerOnControlSignal;
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
        //Executes after a trial...
        if (restPresent){
            targetActive = false;

            if (Settings.instance.currentRunType == RunType.Kinematic){
                //DISTANCE----------------
                trialAccuracyDistanceKin.Add(targetAccuracyDistanceKin);
                targetAccuracyDistanceKin = 0;
                totalAccuracyDistanceKin = 0;
                for (int i = 0; i < trialAccuracyDistanceKin.Count; i++){
                    totalAccuracyDistanceKin = totalAccuracyDistanceKin + trialAccuracyDistanceKin[i];
                }
                accuracyDistanceKin = totalAccuracyDistanceKin / trialAccuracyDistanceKin.Count; //percentage from total and count
                
                //boost a few percent to account for accuracy loss
                accuracyDistanceKin = accuracyDistanceKin + 2;
                if (accuracyDistanceKin >= 100){
                    accuracyDistanceKin = 100;
                }
                //---------------------------
                
                //DIFFERENCE----------------
                
                //--------------------------
            }
            if (Settings.instance.currentRunType == RunType.Imagined){
                //DISTANCE----------------
                trialAccuracyDistanceBCI.Add(targetAccuracyDistanceBCI);
                targetAccuracyDistanceBCI = 0;
                totalAccuracyDistanceBCI = 0;
                for (int i = 0; i < trialAccuracyDistanceBCI.Count; i++){
                    totalAccuracyDistanceBCI = totalAccuracyDistanceBCI + trialAccuracyDistanceBCI[i];
                }
                accuracyDistanceBCI = totalAccuracyDistanceBCI / trialAccuracyDistanceBCI.Count; //percentage from total and count
                
                //boost a few percent to account for accuracy loss
                accuracyDistanceBCI = accuracyDistanceBCI + 2;
                if (accuracyDistanceBCI >= 100){
                    accuracyDistanceBCI = 100;
                }
                //---------------------------
                
                //DIFFERENCE----------------
                targetAccuracyDifferenceBCI = Utilities.ReturnPositive(targetAccuracyDifferenceBCI);
                trialAccuracyDifferenceBCI.Add(targetAccuracyDifferenceBCI);
                targetAccuracyDifferenceBCI=Vector3.zero;
                totalAccuracyDifferenceBCI = Vector3.zero;
                for (int i = 0; i < trialAccuracyDifferenceBCI.Count; i++){
                    totalAccuracyDifferenceBCI = totalAccuracyDifferenceBCI + trialAccuracyDifferenceBCI[i];
                }
                accuracyDifferenceAverageBCI = totalAccuracyDifferenceBCI / trialAccuracyDifferenceBCI.Count;

                highestDifference = Utilities.ReturnMax(trialAccuracyDifferenceBCI);
                
                //get the percentage from the highest and the average
                // accuracyDifferencePercentageBCI = accuracyDifferenceAverageBCI / highestDifference;
                accuracyDifferencePercentageBCI = Utilities.DivideVector(accuracyDifferenceAverageBCI, highestDifference)*100;
                //--------------------------
            }
            if (OnScoreAction != null){
                OnScoreAction(accuracyDistanceKin, accuracyDistanceBCI);
            }
            
            SaveScore();
        }
    }
    
    private void BCI_ControlManagerOnControlSignal(Vector3 cvPredicted, Vector3 cvTarget, Vector3 cvAssisted, Vector3 cvRefined){
        vectorPredicted = cvPredicted;
        vectorTarget = cvTarget;
        vectorAssisted = cvAssisted;
        vectorRefined = cvRefined;
    }
    #endregion

    private void Awake(){
        instance = this;
    }

    void Update(){
        if (targetActive && Settings.instance.currentRunType==RunType.Kinematic){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceKin < feedbackPercentage){
                targetAccuracyDistanceKin = feedbackPercentage;
            }
        }
        if (targetActive && Settings.instance.currentRunType==RunType.Imagined){
            //distance
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceBCI < feedbackPercentage){
                targetAccuracyDistanceBCI = feedbackPercentage;
            }

            //difference
            predictedTargetedDifference = vectorPredicted - vectorTarget;
            if (targetAccuracyDifferenceBCI.magnitude < predictedTargetedDifference.magnitude){
                targetAccuracyDifferenceBCI = predictedTargetedDifference;
            }
            
            
            
            meanSqError = (vectorTarget - vectorPredicted);
            meanSqError = new Vector3(meanSqError.x * meanSqError.x, meanSqError.y * meanSqError.y, meanSqError.z * meanSqError.z);

            frameAccumulation++;
            meanSqErrorAverage = meanSqError / frameAccumulation;
        }

        if (!targetActive){
            frameAccumulation = 0; 
        }
        
    }

    // private Vector3 ReturnPositive(Vector3 v){
    //     float tmpx=0; float tmpy=0; float tmpz=0;
    //     if (v.x < 0){
    //         tmpx = -v.x;
    //     }
    //     else{
    //         tmpx = v.x;
    //     }
    //     if (v.y < 0){
    //         tmpy = -v.y;
    //     }
    //     else{
    //         tmpy = v.y;
    //     }
    //     if (v.z < 0){
    //         tmpz = -v.z;
    //     }
    //     else{
    //         tmpz = v.z;
    //     }
    //
    //     Vector3 pv = new Vector3(tmpx, tmpy, tmpz);
    //     return pv;
    // }

    private void SaveScore(){
        scoreData.name = Settings.instance.sessionName;
        scoreData.sessionNumber = Settings.instance.sessionNumber;
        scoreData.assistancePercentage = Settings.instance.BCI_ControlAssistance;
        scoreData.accuracyKinematic = accuracyDistanceKin;
        scoreData.accuracyBCI = accuracyDistanceBCI;
        
        float a = accuracyDistanceBCI - Settings.instance.BCI_ControlAssistance;
        if (a <= 0){ a = 0; }
        scoreData.accuracyBCI_unassisted = a;
        
        JSONWriter jWriter = new JSONWriter();
        jWriter.OutputScoreJSON(scoreData);
        print("score written------------------------");
    }
    
    
}
