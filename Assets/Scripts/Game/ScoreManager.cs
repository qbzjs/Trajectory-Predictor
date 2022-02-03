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
    private Settings settings;

    public int runNumber;
    public int blockNumber;
    public string runType;
    
    [Header("CONTROL VECTORS (read only)")]
    [Space(4)]
    [SerializeField]private Vector3 vectorPredicted;
    [SerializeField]private Vector3 vectorTarget;
    //[SerializeField]private Vector3 vectorTrackedObject;
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
    
    public float targetAccuracyDistance;
    public List<float> trialAccuracyDistance = new List<float>();
    public float totalAccuracyDistance;
    public float accuracyDistance;

    [Header("-- Correlation ACCURACY --")]
    [Space(4)]
    //generic accumulation vectors per trial
    public Vector3 predictedAccumulation; 
    public Vector3 predictedAssistedAccumulation; 
    public Vector3 targetAccumulation;

    [Header("workings-----------------------")]
    //workings BCI
    public List<Vector3> trialPredictedVector = new List<Vector3>();
    public List<Vector3> trialPredictedAssistedVector = new List<Vector3>();
    public List<Vector3> trialTargetVector = new List<Vector3>();
    public Vector3 trialPredictedSum;
    public Vector3 trialPredictedAssistedSum;
    public Vector3 trialTargetSum;
    public Vector3 predictedTargetDifference;
    public Vector3 predictedAssistedTargetDifference;
    
    [Header("scores-----------------------")]
    //scores BCI
    public Vector3 correlationPercentage; //ACTUAL SCORE %
    public Vector3 correlationAssistedPercentage; //ACTUAL SCORE %
    public Vector3 correlationPercentage_Display; //DISPLAY
    public Vector3 correlationAssistedPercentage_Display; //DISPLAY
    
    [Header("-- MEAN SQUARE --")]
    [Space(4)]
    public Vector3 meanSqError;
    public Vector3 meanSqErrorAssisted;
    public Vector3 meanSqErrorAccumulated;
    public List<Vector3> meanSqErrorFrames = new List<Vector3>();
    public Vector3 meanSqErrorAverage;
    public int frameAccumulation;
    
    public delegate void ScoreAction(float distanceAccuracyKin, float distanceAccuracyBCI, 
        Vector3 correlationPercentage,Vector3 correlationAssistedPercentage,
        Vector3 correlationPercentageDisplay,Vector3 correlationAssistedPercentageDisplay);
    public static event ScoreAction OnScoreAction;
    
    #region Subscriptions
    private void OnEnable(){
        GameManager.OnBlockAction += GameManagerOnBlockAction;
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject += TrackedObjectReferenceOnTrackedObject;
        BCI_ControlManager.OnControlSignal += BCI_ControlManagerOnControlSignal;
        GameManager.OnProgressAction += GameManagerOnProgressAction;
    }

    private void GameManagerOnBlockAction(GameStatus eventType, float lifetime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.Countdown){
            ResetScores();
        }
        if (eventType == GameStatus.BlockComplete){
            ResetScores();
        }
    }

    private void GameManagerOnProgressAction(GameStatus eventType, float completionPercentage, int run, int runTotal, int block, int blockTotal, int trial, int trialTotal){
        runNumber = run;
        blockNumber = block;
        runType = Settings.instance.currentRunType.ToString();
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
        //vectorTrackedObject = dao.MotionData_ActiveWrist.velocity;
        vectorAssisted = cvAssisted; 
        vectorRefined = cvRefined; 
    }
    #endregion

    #region Initialise

    private void Awake(){
        instance = this;
    }

    private void Start(){
        dao = DAO.instance;
        settings = Settings.instance;
    }

    #endregion


    void Update(){
        //distance
        if (targetActive && settings.currentRunType==RunType.Kinematic){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceKin < feedbackPercentage){
                targetAccuracyDistanceKin = feedbackPercentage;
            }

        }
        if (targetActive && settings.currentRunType==RunType.Imagined){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistanceBCI < feedbackPercentage){
                targetAccuracyDistanceBCI = feedbackPercentage;
            }
        }

        if (targetActive){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            if (targetAccuracyDistance < feedbackPercentage){
                targetAccuracyDistance = feedbackPercentage;
            }
        }

        

        
    }

    private void FixedUpdate(){
        
        if (targetActive)
        {
            //accumulate velocities..
            predictedAccumulation += vectorPredicted;
            predictedAssistedAccumulation += vectorRefined; //refined is assisted
            targetAccumulation += vectorTarget;
            
            //mean square error
            if (targetActive){
                meanSqError = (vectorTarget - vectorPredicted);
                meanSqError = new Vector3(meanSqError.x * meanSqError.x, meanSqError.y * meanSqError.y, meanSqError.z * meanSqError.z);
                
                meanSqErrorAssisted = (vectorTarget - vectorRefined);
                meanSqErrorAssisted = new Vector3(meanSqErrorAssisted.x * meanSqErrorAssisted.x, meanSqErrorAssisted.y * meanSqErrorAssisted.y, meanSqErrorAssisted.z * meanSqErrorAssisted.z);
                
                //todo log mean square error in list and average
                meanSqErrorAccumulated += meanSqError;
                
                meanSqErrorFrames.Add(meanSqError);
                //meanSqError = 
            }
        }
    }

    public void TargetPresented(){
        
    }

    public void TargetRemoved(){

        #region Correlation

                float x = 0; float y = 0; float z = 0;
        float xa = 0; float ya = 0; float za = 0;
        
        
        //DIFFERENCE----------------
        trialPredictedVector.Add(predictedAccumulation);
        trialPredictedAssistedVector.Add(predictedAssistedAccumulation);
        trialTargetVector.Add(targetAccumulation);
            
        predictedAccumulation = Vector3.zero;
        predictedAssistedAccumulation = Vector3.zero;
        targetAccumulation = Vector3.zero;
        trialPredictedSum = Vector3.zero;
        trialPredictedAssistedSum = Vector3.zero;
        trialTargetSum = Vector3.zero;
        
        //get predicted and predicted assisted to target difference percent
        for (int i = 0; i < trialPredictedVector.Count; i++){
            trialPredictedSum = trialPredictedSum + trialPredictedVector[i];
            trialPredictedAssistedSum = trialPredictedAssistedSum + trialPredictedAssistedVector[i];
            trialTargetSum = trialTargetSum + trialTargetVector[i];
        }

        predictedTargetDifference = trialTargetSum - trialPredictedSum;
        predictedAssistedTargetDifference = trialTargetSum - trialPredictedAssistedSum;
        //predictedTargetPercentageBCI = Utilities.DivideVector(trialPredictedSumBCI, trialTargetSumBCI) * 100; //todo check vector division??
        correlationPercentage = new Vector3(trialPredictedSum.x / trialTargetSum.x, trialPredictedSum.y / trialTargetSum.y, trialPredictedSum.z / trialTargetSum.z);
        correlationPercentage = correlationPercentage * 100;
        correlationAssistedPercentage = new Vector3(trialPredictedAssistedSum.x / trialTargetSum.x, trialPredictedAssistedSum.y / trialTargetSum.y, trialPredictedAssistedSum.z / trialTargetSum.z);
        correlationAssistedPercentage = correlationAssistedPercentage * 100;

        //limit % to 200 - 100% is sweet spot for max correlation during a trial // PROBLEMS CASTING IN VECTOR3!! -done with floats now
//            correlationPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.z, 200f));
//            correlationAssistedPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.z, 200f));

        //Limits and casting
        x = correlationPercentage.x; y = correlationPercentage.y; z = correlationPercentage.z;
        
        xa = correlationAssistedPercentage.x; ya = correlationAssistedPercentage.y; za = correlationAssistedPercentage.z;
        
        x = Utilities.SetUpperLimit(x, 200f); y = Utilities.SetUpperLimit(y, 200f); z = Utilities.SetUpperLimit(z, 200f);

        xa = Utilities.SetUpperLimit(xa, 200f); ya = Utilities.SetUpperLimit(ya, 200f); za = Utilities.SetUpperLimit(za, 200f);
        
        correlationPercentage_Display = new Vector3(x, y, z);
        correlationAssistedPercentage_Display = new Vector3(xa, ya, za);
        
        x = Utilities.SetLowerLimit(x, 0f); y = Utilities.SetLowerLimit(y, 0f); z = Utilities.SetLowerLimit(z, 0f);
        
        xa = Utilities.SetLowerLimit(xa, 0f); ya = Utilities.SetLowerLimit(ya, 0f); za = Utilities.SetLowerLimit(za, 0f);

        correlationPercentage_Display = new Vector3(x, y, z);
        correlationAssistedPercentage_Display = new Vector3(xa, ya, za);

        #endregion

        
        
        if (settings.currentRunType == RunType.Kinematic){
            
            #region DistanceScore Kin
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
            
            #region CorrelationScore BCI
            //DIFFERENCE----------------
            // trialPredictedVectorKin.Add(predictedAccumulation);
            // trialPredictedAssistedVectorKin.Add(predictedAssistedAccumulation);
            // trialTargetVectorKin.Add(targetAccumulation);
            //
            // predictedAccumulation = Vector3.zero;
            // predictedAssistedAccumulation = Vector3.zero;
            // targetAccumulation = Vector3.zero;
            // trialPredictedSumKin = Vector3.zero;
            // trialPredictedAssistedSumKin = Vector3.zero;
            // trialTargetSumKin = Vector3.zero;
            //
            // //get predicted and predicted assisted to target difference percent
            // for (int i = 0; i < trialPredictedVectorKin.Count; i++){
            //     trialPredictedSumKin = trialPredictedSumKin + trialPredictedVectorKin[i];
            //     trialPredictedAssistedSumKin = trialPredictedAssistedSumKin + trialPredictedAssistedVectorKin[i];
            //     trialTargetSumKin = trialTargetSumKin + trialTargetVectorKin[i];
            // }
            //
            // predictedTargetDifferenceKin = trialTargetSumKin - trialPredictedSumKin;
            // predictedAssistedTargetDifferenceKin = trialTargetSumKin - trialPredictedAssistedSumKin;
            // //predictedTargetPercentageKin = Utilities.DivideVector(trialPredictedSumKin, trialTargetSumKin) * 100; //todo check vector division??
            // correlationPercentageKin = new Vector3(trialPredictedSumKin.x / trialTargetSumKin.x, trialPredictedSumKin.y / trialTargetSumKin.y, trialPredictedSumKin.z / trialTargetSumKin.z);
            // correlationPercentageKin = correlationPercentageKin * 100;
            // correlationAssistedPercentageKin = new Vector3(trialPredictedAssistedSumKin.x / trialTargetSumKin.x, trialPredictedAssistedSumKin.y / trialTargetSumKin.y, trialPredictedAssistedSumKin.z / trialTargetSumKin.z);
            // correlationAssistedPercentageKin = correlationAssistedPercentageKin * 100;
            //
            // //limit % to 200 - 100% is sweet spot for max correlation during a trial
            // // correlationPercentageKin = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageKin.x,200f), Utilities.SetUpperLimit(predictedTargetPercentageKin.y,200f), Utilities.SetUpperLimit(predictedTargetPercentageKin.z,200f));
            // // correlationAssistedPercentageKin = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.x,200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.y,200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageKin.z,200f));
            // //
            // // //don't allow percentage below 0
            // // correlationPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedTargetPercentageKin.x,0f), Utilities.SetLowerLimit(predictedTargetPercentageKin.y,0f), Utilities.SetLowerLimit(predictedTargetPercentageKin.z,0f));
            // // correlationAssistedPercentageBCI = new Vector3(Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.x,0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.y,0f), Utilities.SetLowerLimit(predictedAssistedTargetPercentageKin.z,0f));
            //
            // //Limits and casting
            // x = correlationPercentageKin.x; y = correlationPercentageKin.y; z = correlationPercentageKin.z;
            //
            // xa = correlationAssistedPercentageKin.x; ya = correlationAssistedPercentageKin.y; za = correlationAssistedPercentageKin.z;
            //
            // x = Utilities.SetUpperLimit(x, 200f); y = Utilities.SetUpperLimit(y, 200f); z = Utilities.SetUpperLimit(z, 200f);
            //
            // xa = Utilities.SetUpperLimit(xa, 200f); ya = Utilities.SetUpperLimit(ya, 200f); za = Utilities.SetUpperLimit(za, 200f);
            //
            // correlationPercentageKin_Display = new Vector3(x, y, z);
            // correlationAssistedPercentageKin_Display = new Vector3(xa, ya, za);
            //
            // x = Utilities.SetLowerLimit(x, 0f); y = Utilities.SetLowerLimit(y, 0f); z = Utilities.SetLowerLimit(z, 0f);
            //
            // xa = Utilities.SetLowerLimit(xa, 0f); ya = Utilities.SetLowerLimit(ya, 0f); za = Utilities.SetLowerLimit(za, 0f);
            //
            // correlationPercentageKin_Display = new Vector3(x, y, z);
            // correlationAssistedPercentageKin_Display = new Vector3(xa, ya, za);
            
            //--------------------------
            #endregion
        }

        if (settings.currentRunType == RunType.Imagined){
            
            #region DistanceScore BCI

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

            #region CorrelationScore BCI

//             //DIFFERENCE----------------
//             trialPredictedVectorBCI.Add(predictedAccumulation);
//             trialPredictedAssistedVectorBCI.Add(predictedAssistedAccumulation);
//             trialTargetVectorBCI.Add(targetAccumulation);
//             
//             predictedAccumulation = Vector3.zero;
//             predictedAssistedAccumulation = Vector3.zero;
//             targetAccumulation = Vector3.zero;
//             trialPredictedSumBCI = Vector3.zero;
//             trialPredictedAssistedSumBCI = Vector3.zero;
//             trialTargetSumBCI = Vector3.zero;
//             
//             //get predicted and predicted assisted to target difference percent
//             for (int i = 0; i < trialPredictedVectorBCI.Count; i++){
//                 trialPredictedSumBCI = trialPredictedSumBCI + trialPredictedVectorBCI[i];
//                 trialPredictedAssistedSumBCI = trialPredictedAssistedSumBCI + trialPredictedAssistedVectorBCI[i];
//                 trialTargetSumBCI = trialTargetSumBCI + trialTargetVectorBCI[i];
//             }
//
//             predictedTargetDifferenceBCI = trialTargetSumBCI - trialPredictedSumBCI;
//             predictedAssistedTargetDifferenceBCI = trialTargetSumBCI - trialPredictedAssistedSumBCI;
//             //predictedTargetPercentageBCI = Utilities.DivideVector(trialPredictedSumBCI, trialTargetSumBCI) * 100; //todo check vector division??
//             correlationPercentageBCI = new Vector3(trialPredictedSumBCI.x / trialTargetSumBCI.x, trialPredictedSumBCI.y / trialTargetSumBCI.y, trialPredictedSumBCI.z / trialTargetSumBCI.z);
//             correlationPercentageBCI = correlationPercentageBCI * 100;
//             correlationAssistedPercentageBCI = new Vector3(trialPredictedAssistedSumBCI.x / trialTargetSumBCI.x, trialPredictedAssistedSumBCI.y / trialTargetSumBCI.y, trialPredictedAssistedSumBCI.z / trialTargetSumBCI.z);
//             correlationAssistedPercentageBCI = correlationAssistedPercentageBCI * 100;
//
//             //limit % to 200 - 100% is sweet spot for max correlation during a trial // PROBLEMS CASTING IN VECTOR3!! -done with floats now
// //            correlationPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.z, 200f));
// //            correlationAssistedPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.z, 200f));
//
//             //Limits and casting
//             x = correlationPercentageBCI.x; y = correlationPercentageBCI.y; z = correlationPercentageBCI.z;
//             
//             xa = correlationAssistedPercentageBCI.x; ya = correlationAssistedPercentageBCI.y; za = correlationAssistedPercentageBCI.z;
//             
//             x = Utilities.SetUpperLimit(x, 200f); y = Utilities.SetUpperLimit(y, 200f); z = Utilities.SetUpperLimit(z, 200f);
//
//             xa = Utilities.SetUpperLimit(xa, 200f); ya = Utilities.SetUpperLimit(ya, 200f); za = Utilities.SetUpperLimit(za, 200f);
//             
//             correlationPercentageBCI_Display = new Vector3(x, y, z);
//             correlationAssistedPercentageBCI_Display = new Vector3(xa, ya, za);
//             
//             x = Utilities.SetLowerLimit(x, 0f); y = Utilities.SetLowerLimit(y, 0f); z = Utilities.SetLowerLimit(z, 0f);
//             
//             xa = Utilities.SetLowerLimit(xa, 0f); ya = Utilities.SetLowerLimit(ya, 0f); za = Utilities.SetLowerLimit(za, 0f);
//
//             correlationPercentageBCI_Display = new Vector3(x, y, z);
//             correlationAssistedPercentageBCI_Display = new Vector3(xa, ya, za);

            #endregion
            
        }
        
        //broadcast the score
        if (OnScoreAction != null){
            OnScoreAction(accuracyDistanceKin, accuracyDistanceBCI,
                correlationPercentage, correlationAssistedPercentage,
                 correlationPercentage_Display, correlationAssistedPercentage_Display);
        }
    }

    #region Reset Scores

    private void ResetScores(){
        targetAccuracyDistanceKin = 0;
        trialAccuracyDistanceKin.Clear();
        totalAccuracyDistanceKin = 0;
        accuracyDistanceKin = 0;

        targetAccuracyDistanceBCI = 0;
        trialAccuracyDistanceBCI.Clear();
        totalAccuracyDistanceBCI = 0;
        accuracyDistanceBCI = 0;

        predictedAccumulation = Vector3.zero;
        predictedAssistedAccumulation = Vector3.zero;
        targetAccumulation = Vector3.zero;
        
        trialPredictedVector.Clear();
        trialPredictedAssistedVector.Clear();
        trialTargetVector.Clear();
        trialPredictedSum = Vector3.zero;
        trialPredictedAssistedSum = Vector3.zero;
        trialTargetSum = Vector3.zero;
        predictedTargetDifference = Vector3.zero;
        predictedAssistedTargetDifference = Vector3.zero;
        correlationPercentage = Vector3.zero; //SCORE
        correlationAssistedPercentage = Vector3.zero; //SCORE
        correlationPercentage_Display = Vector3.zero; //SCORE
        correlationAssistedPercentage_Display = Vector3.zero; //SCORE
        
    }

    #endregion

    
    private void SaveScore(){
        scoreData.name = Settings.instance.sessionName;
        scoreData.sessionNumber = Settings.instance.sessionNumber;

        scoreData.run = runNumber.ToString();
        scoreData.block = blockNumber.ToString();
        scoreData.runType = runType;

        scoreData.assistancePercentage = Settings.instance.BCI_ControlAssistance;
        
        scoreData.distanceAccuracyKinematic = accuracyDistanceKin;
        scoreData.distanceAccuracyBCI = accuracyDistanceBCI;
        
        float a = accuracyDistanceBCI - Settings.instance.BCI_ControlAssistance;
        if (a <= 0){ a = 0; }
        scoreData.distanceAccuracyBCI_Unassisted = a;

        scoreData.correlation = correlationPercentage;
        scoreData.correlationAssisted = correlationAssistedPercentage;
        
        scoreData.correlationDisplay = correlationPercentage_Display;
        scoreData.correlationAssistedDisplay = correlationAssistedPercentage_Display;
        
        JSONWriter jWriter = new JSONWriter();
        jWriter.OutputScoreJSON(scoreData);
        print("score written------------------------");
    }
    
    
}
