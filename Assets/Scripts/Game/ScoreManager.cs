using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

public class ScoreManager : MonoBehaviour{

    public static ScoreManager instance;

    public ScoreDataObject scoreData;

    private DAO dao;
    
    [Header("CONTROL VECTORS (read only)")]
    [Space(4)]
    [SerializeField]private Vector3 vectorPredicted;
    [SerializeField]private Vector3 vectorTarget;
    [SerializeField]private Vector3 vectorTrackedObject;
    [SerializeField]private Vector3 vectorAssisted;
    [SerializeField]private Vector3 vectorRefined;
    
    [Header("-- DISTANCE ACCURACY --")]
    [Space(4)]
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

    [Header("-- DIFFERENTIAL ACCURACY --")]
    [Space(4)]
    public Vector3 predictedAccumulation; //GENERIC - CURRENT TRIAL
    public Vector3 predictedAssistedAccumulation; //GENERIC - CURRENT TRIAL
    public Vector3 targetAccumulation; //GENERIC - CURRENT TRIAL

    public List<Vector3> trialPredictedVectorKin = new List<Vector3>();
    public List<Vector3> trialPredictedAssistedVectorKin = new List<Vector3>();
    public List<Vector3> trialTargetVectorKin = new List<Vector3>();
    public Vector3 trialPredictedSumKin;
    public Vector3 trialPredictedAssistedSumKin;
    public Vector3 trialTargetSumKin;
    public Vector3 predictedTargetDifferenceKin;
    public Vector3 predictedAssistedTargetDifferenceKin;
    public Vector3 predictedTargetPercentageKin;
    public Vector3 predictedAssistedTargetPercentageKin;
    public Vector3 differentialPercentageKin;
    public Vector3 differentialAssistedPercentageKin;
    
    public List<Vector3> trialPredictedVectorBCI = new List<Vector3>();
    public List<Vector3> trialPredictedAssistedVectorBCI = new List<Vector3>();
    public List<Vector3> trialTargetVectorBCI = new List<Vector3>();
    public Vector3 trialPredictedSumBCI;
    public Vector3 trialPredictedAssistedSumBCI;
    public Vector3 trialTargetSumBCI;
    public Vector3 predictedTargetDifferenceBCI;
    public Vector3 predictedAssistedTargetDifferenceBCI;
    public Vector3 predictedTargetPercentageBCI;
    public Vector3 predictedAssistedTargetPercentageBCI;
    public Vector3 differentialPercentageBCI;
    public Vector3 differentialAssistedPercentageBCI;

    
    // [Header("-- MEAN SQUARE --")]
    // [Space(4)]
    // public Vector3 meanSqError;
    // public Vector3 meanSqErrorAverage;
    // public int frameAccumulation;
    
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
        if (eventType == TrialEventType.PreTrialPhase){
            //get hand here for kinematic...or this is the tracked object???
            
        }
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
            TargetPresented();
        }
        //Executes after a trial...
        if (restPresent){
            targetActive = false;

            TargetRemoved();
            SaveScore(); //after performing score calculation..
        }
    }
    
    private void BCI_ControlManagerOnControlSignal(Vector3 cvPredicted, Vector3 cvTarget, Vector3 cvAssisted, Vector3 cvRefined){
        vectorPredicted = cvPredicted; //this is the raw signal
        vectorTarget = cvTarget;
        vectorTrackedObject = dao.MotionData_ActiveWrist.velocity;
        vectorAssisted = cvAssisted; 
        vectorRefined = cvRefined; //todo calculate differential using assisted / refined
    }
    #endregion

    #region Initialise

    private void Awake(){
        instance = this;
    }

    private void Start(){
        dao = DAO.instance;
        
    }

    #endregion


    void Update(){
        //distance
        if (targetActive && Settings.instance.currentRunType==RunType.Kinematic){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceKin < feedbackPercentage){
                targetAccuracyDistanceKin = feedbackPercentage;
            }

        }
        if (targetActive && Settings.instance.currentRunType==RunType.Imagined){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceBCI < feedbackPercentage){
                targetAccuracyDistanceBCI = feedbackPercentage;
            }
        }

        // if (targetActive){
        //     meanSqError = (vectorTarget - vectorPredicted);
        //     meanSqError = new Vector3(meanSqError.x * meanSqError.x, meanSqError.y * meanSqError.y, meanSqError.z * meanSqError.z);
        //
        //     frameAccumulation++;
        //     meanSqErrorAverage = meanSqError / frameAccumulation;
        // }
        // else{
        //     frameAccumulation = 0;
        // }
        
    }

    private void FixedUpdate(){
        //difference
        if (targetActive && Settings.instance.currentRunType==RunType.Kinematic)
        {
            // Vector3 pa = Utilities.ReturnPositive(vectorPredicted);
            // predictedAccumulation += pa;
            // Vector3 ta = Utilities.ReturnPositive(vectorTarget);
            // targetAccumulation += ta;
            
            predictedAccumulation += vectorPredicted;
            predictedAssistedAccumulation += vectorRefined; //refined is assisted
            targetAccumulation += vectorTrackedObject; //use tracked hand velocity for kinematic
            
        }

        if (targetActive && Settings.instance.currentRunType == RunType.Imagined)
        {
            // Vector3 pa = Utilities.ReturnPositive(vectorPredicted);
            // predictedAccumulation += pa;
            // Vector3 ta = Utilities.ReturnPositive(vectorTarget);
            // targetAccumulation += ta;
            
            predictedAccumulation += vectorPredicted;
            predictedAssistedAccumulation += vectorRefined; //refined is assisted
            targetAccumulation += vectorTarget;
        }
    }

    public void TargetPresented(){
        
    }

    public void TargetRemoved(){
        if (Settings.instance.currentRunType == RunType.Kinematic){
            
            #region DistanceScore
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
            #endregion
            
            //DIFFERENCE----------------
            trialPredictedVectorKin.Add(predictedAccumulation);
            trialPredictedAssistedVectorKin.Add(predictedAssistedAccumulation);
            trialTargetVectorKin.Add(targetAccumulation);
            
            predictedAccumulation = Vector3.zero;
            predictedAssistedAccumulation = Vector3.zero;
            targetAccumulation = Vector3.zero;
            trialPredictedSumKin = Vector3.zero;
            trialPredictedAssistedSumKin = Vector3.zero;
            trialTargetSumKin = Vector3.zero;
            
            //get predicted and predicted assisted to target difference percent
            for (int i = 0; i < trialPredictedVectorKin.Count; i++){
                trialPredictedSumKin = trialPredictedSumKin + trialPredictedVectorKin[i];
                trialPredictedAssistedSumKin = trialPredictedAssistedSumKin + trialPredictedAssistedVectorKin[i];
                trialTargetSumKin = trialTargetSumKin + trialTargetVectorKin[i];
            }

            predictedTargetDifferenceKin = trialTargetSumKin - trialPredictedSumKin;
            predictedAssistedTargetDifferenceKin = trialTargetSumKin - trialPredictedAssistedSumKin;
            //predictedTargetPercentageKin = Utilities.DivideVector(trialPredictedSumKin, trialTargetSumKin) * 100; //todo check vector division??
            predictedTargetPercentageKin = new Vector3(trialPredictedSumKin.x / trialTargetSumKin.x, trialPredictedSumKin.y / trialTargetSumKin.y, trialPredictedSumKin.z / trialTargetSumKin.z) * 100;
            predictedAssistedTargetPercentageKin = new Vector3(trialPredictedAssistedSumKin.x / trialTargetSumKin.x, trialPredictedAssistedSumKin.y / trialTargetSumKin.y, trialPredictedAssistedSumKin.z / trialTargetSumKin.z) * 100;

            //limit % to 200 - 100% is sweet spot for max correlation during a trial
            differentialPercentageKin = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageKin.x,200f), Utilities.SetUpperLimit(predictedTargetPercentageKin.y,200f), Utilities.SetUpperLimit(predictedTargetPercentageKin.z,200f));
            differentialAssistedPercentageKin = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.x,200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.y,200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.z,200f));

            //don't allow percentage below 0
            differentialPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedTargetPercentageKin.x,0f), Utilities.SetLowerLimit(predictedTargetPercentageKin.y,0f), Utilities.SetLowerLimit(predictedTargetPercentageKin.z,0f));
            differentialAssistedPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.x,0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.y,0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.z,0f));
            
            //--------------------------
        }

        if (Settings.instance.currentRunType == RunType.Imagined){
            
            #region DistanceScore

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

            #endregion
            
            //DIFFERENCE----------------
            trialPredictedVectorBCI.Add(predictedAccumulation);
            trialPredictedAssistedVectorBCI.Add(predictedAssistedAccumulation);
            trialTargetVectorBCI.Add(targetAccumulation);
            
            predictedAccumulation = Vector3.zero;
            predictedAssistedAccumulation = Vector3.zero;
            targetAccumulation = Vector3.zero;
            trialPredictedSumBCI = Vector3.zero;
            trialPredictedAssistedSumBCI = Vector3.zero;
            trialTargetSumBCI = Vector3.zero;
            
            //get predicted and predicted assisted to target difference percent
            for (int i = 0; i < trialPredictedVectorBCI.Count; i++){
                trialPredictedSumBCI = trialPredictedSumBCI + trialPredictedVectorBCI[i];
                trialPredictedAssistedSumBCI = trialPredictedAssistedSumBCI + trialPredictedAssistedVectorBCI[i];
                trialTargetSumBCI = trialTargetSumBCI + trialTargetVectorBCI[i];
            }

            predictedTargetDifferenceBCI = trialTargetSumBCI - trialPredictedSumBCI;
            predictedAssistedTargetDifferenceBCI = trialTargetSumBCI - trialPredictedAssistedSumBCI;
            //predictedTargetPercentageBCI = Utilities.DivideVector(trialPredictedSumBCI, trialTargetSumBCI) * 100; //todo check vector division??
            predictedTargetPercentageBCI = new Vector3(trialPredictedSumBCI.x / trialTargetSumBCI.x, trialPredictedSumBCI.y / trialTargetSumBCI.y, trialPredictedSumBCI.z / trialTargetSumBCI.z) * 100;
            predictedAssistedTargetPercentageBCI = new Vector3(trialPredictedAssistedSumBCI.x / trialTargetSumBCI.x, trialPredictedAssistedSumBCI.y / trialTargetSumBCI.y, trialPredictedAssistedSumBCI.z / trialTargetSumBCI.z) * 100;

            //limit % to 200 - 100% is sweet spot for max correlation during a trial
            differentialPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.z, 200f));
            differentialAssistedPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.z, 200f));

            //don't allow percentage below 0
            differentialPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedTargetPercentageBCI.x, 0f), Utilities.SetLowerLimit(predictedTargetPercentageBCI.y, 0f), Utilities.SetLowerLimit(predictedTargetPercentageBCI.z, 0f));
            differentialAssistedPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedAssistedTargetPercentageBCI.x, 0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageBCI.y, 0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageBCI.z, 0f));

            #region MeanSquare

            //positive
            // Vector3 p = new Vector3(
            //     Utilities.ReturnPositive(differentialPercentageBCI.x), 
            //     Utilities.ReturnPositive(differentialPercentageBCI.y), 
            //     Utilities.ReturnPositive(differentialPercentageBCI.z));
            //
            // differentialPercentageBCI = new Vector3(p.x, p.y, p.z);

            //--------------------------

            #endregion

        }
        

        
        //TODO MAKE THE ABOVE FOR BOTH BCI AND KIN...
        //BROADCASST THE SCORE
        //SETUP THE UI

        if (OnScoreAction != null){
            OnScoreAction(accuracyDistanceKin, accuracyDistanceBCI);
        }
    }

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
