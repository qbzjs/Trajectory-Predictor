using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

public class ScoreManager : MonoBehaviour{

    public bool debugScore = false;
    
    public static ScoreManager instance;

    public ScoreSessionDataObject scoreSessionData;
    public ScoreBlockDataObject scoreBlockData;

    private DAO dao;
    private Settings settings;

    [Header("-- SESSION --")]
    
    public int runNumber;
    public int blockNumber;
    public string runType;
    public RunType runTypeEnum;
    
    [Header("----TARGET-----------------------")]
    public bool targetActive;
    
    [Header("CONTROL VECTORS (read only)")]
    [Space(4)]
    [SerializeField]private Vector3 vectorPredicted;
    [SerializeField]private Vector3 vectorTarget;
    //[SerializeField]private Vector3 vectorTrackedObject;
    [SerializeField]private Vector3 vectorAssisted;
    [SerializeField]private Vector3 vectorRefined;
    
    [Header("-- DISTANCE TO TARGET --")]
    [Space(4)]
    public Transform trackedObject;
    public GameObject activeTarget;
    public TargetMagnitudeTracker targetMagnitudeTracker;
    
    [Header("----workings-----------------------")]
    public float feedbackPercentage;

    public float targetDistanceKin;
    public List<float> trialDistanceKin = new List<float>();
    public float totalDistanceKin;

    public float targetDistanceBCI;
    public List<float> trialDistanceBCI = new List<float>();
    public float totalDistanceBCI;
    
    [Header("----scores-----------------------")]
    public float distanceKin;
    public float distanceBCI_assisted;
    public float distanceBCI_unassisted;

    [Header("-- Correlation ACCURACY --")]
    [Space(4)]
    //generic accumulation vectors per trial
    public Vector3 predictedAccumulation; 
    public Vector3 predictedAssistedAccumulation; 
    public Vector3 targetAccumulation;

    [Header("----workings-----------------------")]
    //workings BCI
    public List<Vector3> trialPredictedVector = new List<Vector3>();
    public List<Vector3> trialPredictedAssistedVector = new List<Vector3>();
    public List<Vector3> trialTargetVector = new List<Vector3>();
    public Vector3 trialPredictedSum;
    public Vector3 trialPredictedAssistedSum;
    public Vector3 trialTargetSum;
    public Vector3 predictedTargetDifference;
    public Vector3 predictedAssistedTargetDifference;
    
    [Header("----scores-----------------------")]
    //scores BCI
    public Vector3 correlationPercentage; //ACTUAL SCORE %
    public Vector3 correlationAssistedPercentage; //ACTUAL SCORE %
    public Vector3 correlationPercentage_Display; //DISPLAY
    public Vector3 correlationAssistedPercentage_Display; //DISPLAY
    
    [Header("-- MEAN SQUARE ERROR --")]
    [Space(4)]
    public int frameAccumulation;
    public Vector3 meanSqErrorCurrent;
    public Vector3 meanSqErrorCurrentAssisted;
    public Vector3 meanSqErrorAccumulated;
    public Vector3 meanSqErrorAccumulatedAssisted;
    public Vector3 meanSqErrorAverage;
    public Vector3 meanSqErrorAverageAssisted;
    
    public List<Vector3> meanSqErrorTrialAverage = new List<Vector3>();
    public List<Vector3> meanSqErrorTrialAverageAssisted = new List<Vector3>();
    
    public Vector3 meanSqErrorSum;
    public Vector3 meanSqErrorSumAssisted;
    public Vector3 meanSquareErrorAverage;
    public Vector3 meanSquareErrorAverageAssisted;

    [Header("-- SESSION TOTALS DIFFERENCE --")] [Space(4)]
    public List<float> sdak = new List<float>();
    public List<float> sdaba = new List<float>();
    public List<float> sdabu = new List<float>();
    public float sessionDistanceAccuracyKin;
    public float sessionDistanceAccuracyBCI_Assisted;
    public float sessionDistanceAccuracyBCI_Unassisted;

    [Header("-- SESSION TOTALS CORRELATION --")] [Space(4)]
    public List<Vector3> sck = new List<Vector3>();
    public List<Vector3> scba = new List<Vector3>();
    public List<Vector3> scbu = new List<Vector3>();
    public Vector3 sessionCorrelationKin;
    public float sessionCorrelationKinAvg;
    public Vector3 sessionCorrelationBCI_Assisted;
    public float sessionCorrelationBCIAvg_Assisted;
    public Vector3 sessionCorrelationBCI_Unassisted;
    public float sessionCorrelationBCIAvg_Unassisted;

    [Header("-- overall average performance --")] [Space(4)]
    public float overallPerformanceBlock;
    public float overallPerformanceSession;

    [Header("TARGET HIT")] 
    public int tOne;
    
    [Header("STREAK")] 
    public bool streaking = false;
    public int targetStreak = 0;
    public int streakCounterKin = 4;
    private int sckDefault; //value to reset the counter to each block
    public int streakFeedbackRepeatKin = 4;
    public int streakCounterImag = 4;
    private int sciDefault; //value to reset the counter to each block
    public int streakFeedbackRepeatImag = 4;
    public float streakBonus = 0;
    public List<float> streaks = new List<float>();
    public int longestStreak;
    public int blockStartStreakScore;
    public int streakBonusBlock;
    public int streakBonusSession;
    
    //todo - record targets hit kinematic and imagined
    public TargetHitFormat targetsHitSession;
    public TargetHitFormat targetsHitBlock;

    #region Broadcast Score Events

    public delegate void ScoreBlockObjectAction(ScoreBlockDataObject sessionScoreData);
    public static event ScoreBlockObjectAction OnScoreBlockObjectAction;
    public delegate void ScoreSessionObjectAction(ScoreSessionDataObject sessionScoreData);
    public static event ScoreSessionObjectAction OnScoreSessionObjectAction;
        
    public delegate void ScoreBlockAction(float distanceAccuracyKin, float distanceAccuracyBCI_Assisted, float distanceAccuracyBCI_Unassisted, 
        Vector3 correlationPercentage,Vector3 correlationAssistedPercentage,
        Vector3 correlationPercentageDisplay,Vector3 correlationAssistedPercentageDisplay, float overallPerformance, int streakBonus);
    public static event ScoreBlockAction OnScoreBlockAction;

    public delegate void ScoreSessionAction(float distanceAccuracyKin, float distanceAccuracyBCI_Assisted, float distanceAccuracyBCI_Unassisted,
        Vector3 correlationKin,float correlationKinAvg,
        Vector3 correlationBCI_Assisted,float correlationBCI_AssistedAvg,
        Vector3 correlationBCI_Unassisted,float correlationBCI_UnassistedAvg,
        Vector3 meanSqErrorSum,Vector3 meanSqErrorSumAssisted,
        Vector3 meanSquareErrorAverage,Vector3 meanSquareErrorAverageAssisted,
        float overallPerformance, int streakBonus);
    public static event ScoreSessionAction OnScoreSessionAction;
    
    public delegate void TargetStreakAction(bool streakFeedback, int streakCount, bool showUI);
    public static event TargetStreakAction OnTargetStreakAction;

    #endregion

    
    #region Subscriptions
    private void OnEnable(){
        InputManager.OnUserInputAction += InputManagerOnUserInputAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject += TrackedObjectReferenceOnTrackedObject;
        BCI_ControlManager.OnControlSignal += BCI_ControlManagerOnControlSignal;
        GameManager.OnProgressAction += GameManagerOnProgressAction;
        TargetFeedbackTrigger.OnTargetHit += TargetFeedbackTriggerOnTargetHit;
    }

    private void InputManagerOnUserInputAction(UserInputType inputType){
        if (inputType == UserInputType.Reset){
            ResetScores();
            ResetSession();
        }
    }

    private void OnDisable(){
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
        TrackedObjectReference.OnTrackedObject -= TrackedObjectReferenceOnTrackedObject;
        BCI_ControlManager.OnControlSignal -= BCI_ControlManagerOnControlSignal;
        GameManager.OnProgressAction -= GameManagerOnProgressAction;
        TargetFeedbackTrigger.OnTargetHit -= TargetFeedbackTriggerOnTargetHit;
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifetime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.Countdown){
            ResetScores();
        }
        if (eventType == GameStatus.BlockComplete){
            SessionTotals();
            SaveScore();
            ResetScores();
            print("restt block");
            ResetStreakBlock();
            blockStartStreakScore = streakBonusSession;
        }
    }

    private void GameManagerOnProgressAction(GameStatus eventType, float completionPercentage, int run, int runTotal, int block, int blockTotal, int trial, int trialTotal){
        runNumber = run;
        blockNumber = block;
        runType = Settings.instance.currentRunType.ToString();
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

        if (eventType == TrialEventType.TrialComplete){
            if (debugScore){
                print("TRIAL COMPLETE......................");
            }
            
            TargetTotals();
            SessionTotals();
            SaveScore();
            //ResetScores();
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
        }
    }
    
    private void BCI_ControlManagerOnControlSignal(Vector3 cvPredicted, Vector3 cvTarget, Vector3 cvAssisted, Vector3 cvRefined){
        vectorPredicted = cvPredicted; //this is the raw signal
        vectorTarget = cvTarget;
        //vectorTrackedObject = dao.MotionData_ActiveWrist.velocity;
        vectorAssisted = cvAssisted; 
        vectorRefined = cvRefined; 
    }

    private void TargetFeedbackTriggerOnTargetHit(RunType runType, int tNum){
        TargetHitCounter(runType, tNum);
    }
    
    #endregion

    #region Initialise

    private void Awake(){
        instance = this;
    }

    private void Start(){
        dao = DAO.instance;
        settings = Settings.instance;
        sckDefault = streakCounterKin;
        sciDefault = streakCounterImag;
        ResetSession();
    }

    #endregion


    void Update(){
        //distance
        if (targetActive && settings.currentRunType==RunType.Kinematic){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            if (targetDistanceKin < feedbackPercentage){
                targetDistanceKin = feedbackPercentage;
            }

        }
        if (targetActive && settings.currentRunType==RunType.Imagined){
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            if (targetDistanceBCI < feedbackPercentage){
                targetDistanceBCI = feedbackPercentage;
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
                //iterate the frames for averaging
                frameAccumulation++;
                
                //get mean square per frame
                meanSqErrorCurrent = (vectorTarget - vectorPredicted);
                meanSqErrorCurrent = new Vector3(meanSqErrorCurrent.x * meanSqErrorCurrent.x, meanSqErrorCurrent.y * meanSqErrorCurrent.y, meanSqErrorCurrent.z * meanSqErrorCurrent.z);
                
                meanSqErrorCurrentAssisted = (vectorTarget - vectorRefined);
                meanSqErrorCurrentAssisted = new Vector3(meanSqErrorCurrentAssisted.x * meanSqErrorCurrentAssisted.x, meanSqErrorCurrentAssisted.y * meanSqErrorCurrentAssisted.y, meanSqErrorCurrentAssisted.z * meanSqErrorCurrentAssisted.z);
                
                //accumulate the error
                meanSqErrorAccumulated += meanSqErrorCurrent;
                meanSqErrorAccumulatedAssisted += meanSqErrorCurrentAssisted;
                
                //average based on frames passed
                meanSqErrorAverage = meanSqErrorAccumulated / frameAccumulation;
                meanSqErrorAverageAssisted = meanSqErrorAccumulatedAssisted / frameAccumulation;
            }
        }
        else{
            frameAccumulation = 0;
        }
    }

    #region Target Hit Counters
    
    private void TargetPresented(){
        //add target count for target hit score
        targetsHitSession.totalTargetsPresented++;
        targetsHitBlock.totalTargetsPresented++;
        if (DAO.instance.currentRunType == RunType.Kinematic){
            targetsHitSession.kinTargetsPresented++;
            targetsHitBlock.kinTargetsPresented++;
        }
        if (DAO.instance.currentRunType == RunType.Imagined){
            targetsHitSession.imgTargetsPresented++;
            targetsHitBlock.imgTargetsPresented++;
        }
    }
    private void TargetHitCounter(RunType runType, int tNum){
        //count hit targets for kin or img
        if (runType == RunType.Kinematic){
            switch (tNum){
                case 1:
                    targetsHitSession.t1_kin++;
                    targetsHitBlock.t1_kin++;
                    break;
                case 2:
                    targetsHitSession.t2_kin++;
                    targetsHitBlock.t2_kin++;
                    break;
                case 3:
                    targetsHitSession.t3_kin++;
                    targetsHitBlock.t3_kin++;
                    break;
                case 4:
                    targetsHitSession.t4_kin++;
                    targetsHitBlock.t4_kin++;
                    break;
            }
        }
        if (runType == RunType.Imagined){
            switch (tNum){
                case 1:
                    targetsHitSession.t1_img++;
                    targetsHitBlock.t1_img++;
                    break;
                case 2:
                    targetsHitSession.t2_img++;
                    targetsHitBlock.t2_img++;
                    break;
                case 3:
                    targetsHitSession.t3_img++;
                    targetsHitBlock.t3_img++;
                    break;
                case 4:
                    targetsHitSession.t4_img++;
                    targetsHitBlock.t4_img++;
                    break;
            }
        }
    }

    private void TargetTotals(){
        
        targetsHitSession.totalKinHit = targetsHitSession.t1_kin + targetsHitSession.t2_kin + targetsHitSession.t3_kin + targetsHitSession.t4_kin;
        targetsHitSession.totalImgHit = targetsHitSession.t1_img + targetsHitSession.t2_img + targetsHitSession.t3_img + targetsHitSession.t4_img;
        targetsHitSession.totalTargetsHit = targetsHitSession.totalKinHit + targetsHitSession.totalImgHit;
        
        targetsHitBlock.totalKinHit = targetsHitBlock.t1_kin + targetsHitBlock.t2_kin + targetsHitBlock.t3_kin + targetsHitBlock.t4_kin;
        targetsHitBlock.totalImgHit = targetsHitBlock.t1_img + targetsHitBlock.t2_img + targetsHitBlock.t3_img + targetsHitBlock.t4_img;
        targetsHitBlock.totalTargetsHit = targetsHitBlock.totalKinHit + targetsHitBlock.totalImgHit;
        
    }

    #endregion


    #region Streak Score

    //TODO - DEPRECIATED???
    public void AddTargetHit(){
        int t = DAO.instance.CurrentReachTarget;
        runTypeEnum = DAO.instance.GetRunType();
        
        Debug.Log("TARGET: " + t + " :: Run: " + runTypeEnum.ToString());
        if (runTypeEnum == RunType.Kinematic){
            
        }
        if (runTypeEnum == RunType.Imagined){
            
        }
    }
    public void AddToStreak(){
        targetStreak++;

        if (Settings.instance.currentRunType == RunType.Kinematic){
            if (targetStreak == streakCounterKin){
  //              Debug.Log(("streak..."));
                streakCounterKin = targetStreak + streakFeedbackRepeatKin;
                streaking = true;
                if (OnTargetStreakAction != null){
                    OnTargetStreakAction(true, targetStreak, true);
                }
                AccumulateStreak(targetStreak,1);
            }
            else{
                if (streaking){
                    if (OnTargetStreakAction != null){
                        OnTargetStreakAction(false, targetStreak,true);
                    }
                }
                AccumulateStreak(targetStreak,1);
            }

        }
        if (Settings.instance.currentRunType == RunType.Imagined){
            if (targetStreak == streakCounterImag){
//                Debug.Log(("streak..."));
                streakCounterImag = targetStreak + streakFeedbackRepeatImag;
                streaking = true;
                if (OnTargetStreakAction != null){
                    OnTargetStreakAction(true, targetStreak, true);
                }
                AccumulateStreak(targetStreak,4);
            }
            else
            {
                if (streaking){
                    if (OnTargetStreakAction != null){
                        OnTargetStreakAction(false, targetStreak,true);
                    }
                }
                AccumulateStreak(targetStreak,1);
            }
        }

    }
    public void ResetStreak(){
        targetStreak = 0;
        streakCounterKin = sckDefault;
        streakCounterImag = sciDefault;
        streaking = false;
        if (OnTargetStreakAction != null){
            OnTargetStreakAction(streaking, targetStreak, false);
        }
    }
    public void ResetStreakBlock(){
        targetStreak = 0;
        streakCounterKin = 4;
        streakCounterImag = 2;
        streakBonus = 0;
        streaks.Clear();
        longestStreak = 0;
        streakBonusBlock = 0;
        streaking = false;
        
        if (OnTargetStreakAction != null){
            OnTargetStreakAction(streaking, targetStreak, false);
        }
    }

    public void AccumulateStreak(float a, int multiplier){
        streaks.Add(a);
        streakBonus = streaks.Count;
        longestStreak = Mathf.RoundToInt(streaks.Max()-1);
        int bonus = Mathf.RoundToInt(streakBonus + longestStreak) * multiplier;
        streakBonusSession = streakBonusSession + bonus;
        streakBonusBlock = streakBonusSession - blockStartStreakScore;
    }
    #endregion
    
    public void TargetRemoved(){

        #region Correlation Score

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
//      correlationPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedTargetPercentageBCI.z, 200f));
//      correlationAssistedPercentageBCI = new Vector3(Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.x, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.y, 200f), Utilities.SetUpperLimit(predictedAssistedTargetPercentageBCI.z, 200f));

        //Limits and casting
        // x = correlationPercentage.x; y = correlationPercentage.y; z = correlationPercentage.z;
        //
        // xa = correlationAssistedPercentage.x; ya = correlationAssistedPercentage.y; za = correlationAssistedPercentage.z;
        //
        // x = Utilities.SetUpperLimit(x, 200f); y = Utilities.SetUpperLimit(y, 200f); z = Utilities.SetUpperLimit(z, 200f);
        //
        // xa = Utilities.SetUpperLimit(xa, 200f); ya = Utilities.SetUpperLimit(ya, 200f); za = Utilities.SetUpperLimit(za, 200f);
        //
        // correlationPercentage_Display = new Vector3(x, y, z);
        // correlationAssistedPercentage_Display = new Vector3(xa, ya, za);
        //
        // x = Utilities.SetLowerLimit(x, 0f); y = Utilities.SetLowerLimit(y, 0f); z = Utilities.SetLowerLimit(z, 0f);
        //
        // xa = Utilities.SetLowerLimit(xa, 0f); ya = Utilities.SetLowerLimit(ya, 0f); za = Utilities.SetLowerLimit(za, 0f);
        //
        // correlationPercentage_Display = new Vector3(x, y, z);
        // correlationAssistedPercentage_Display = new Vector3(xa, ya, za);

        #endregion

        
        #region Distance To Target Score
        
        if (settings.currentRunType == RunType.Kinematic){

            //DISTANCE----------------
            trialDistanceKin.Add(targetDistanceKin);
            targetDistanceKin = 0;
            totalDistanceKin = 0;
            for (int i = 0; i < trialDistanceKin.Count; i++){
                totalDistanceKin = totalDistanceKin + trialDistanceKin[i];
            }
            distanceKin = totalDistanceKin / trialDistanceKin.Count; //percentage from total and count

            //boost a few percent to account for accuracy loss
            distanceKin = distanceKin + 2;
            if (distanceKin >= 100){
                distanceKin = 100;
            }
            if (distanceKin <= 0){
                distanceKin = 0;
            }
            
            //---------------------------
        }

        if (settings.currentRunType == RunType.Imagined){

            //DISTANCE----------------
            trialDistanceBCI.Add(targetDistanceBCI);
            targetDistanceBCI = 0;
            totalDistanceBCI = 0;
            for (int i = 0; i < trialDistanceBCI.Count; i++){
                totalDistanceBCI = totalDistanceBCI + trialDistanceBCI[i];
            }
            distanceBCI_assisted = totalDistanceBCI / trialDistanceBCI.Count; //percentage from total and count

            //boost a few percent to account for accuracy loss
            distanceBCI_assisted = distanceBCI_assisted + 2;
            //minus assistance for unassisted distance
            distanceBCI_unassisted = distanceBCI_assisted - settings.BCI_ControlAssistance;
            
            if (distanceBCI_assisted >= 100){
                distanceBCI_assisted = 100;
            }
            if (distanceBCI_assisted <= 0){
                distanceBCI_assisted = 0;
            }

            distanceBCI_unassisted = Utilities.SetLowerLimit(distanceBCI_unassisted, 0);

            //---------------------------

        }
        
        #endregion
        
        #region Mean Square Error
        
        //add error to list
        meanSqErrorTrialAverage.Add(meanSqErrorAverage);
        meanSqErrorTrialAverageAssisted.Add(meanSqErrorAverageAssisted);
        meanSqErrorSum = Vector3.zero; //clear average
        meanSqErrorSumAssisted = Vector3.zero; //clear average
        
        //add up the list
        for (int i = 0; i < meanSqErrorTrialAverage.Count; i++){
            meanSqErrorSum = meanSqErrorSum + meanSqErrorTrialAverage[i];
            meanSqErrorSumAssisted = meanSqErrorSumAssisted + meanSqErrorTrialAverageAssisted[i];
        }
        
        //divide the summed list
        meanSquareErrorAverage = meanSqErrorSum / meanSqErrorTrialAverage.Count;
        meanSquareErrorAverageAssisted = meanSqErrorSumAssisted / meanSqErrorTrialAverage.Count;
            
        #endregion

        //todo average the block correlation vectors for overall performance
        //float p = distanceKin + distanceBCI_assisted + distanceBCI_unassisted + correlationPercentage + correlationAssistedPercentage;
        //overallPerformanceBlock = p / 5;
        
        //broadcast the score
        if (OnScoreBlockAction != null){
            OnScoreBlockAction(distanceKin, distanceBCI_assisted, distanceBCI_unassisted,
                correlationPercentage, correlationAssistedPercentage,
                 correlationPercentage_Display, correlationAssistedPercentage_Display,
                overallPerformanceBlock, streakBonusSession);
        }
        
        SaveScore(); //after performing score calculation..
    }

    #region Session Totals

    public void SessionTotals(){
        if (settings.currentRunType == RunType.Kinematic){
            //distance
            sdak.Add(distanceKin);
            sessionDistanceAccuracyKin = 0;
            for (int i = 0; i < sdak.Count; i++){
                sessionDistanceAccuracyKin = sessionDistanceAccuracyKin + sdak[i];
            }
            sessionDistanceAccuracyKin = sessionDistanceAccuracyKin / sdak.Count;
            
            //correlation
            sck.Add(correlationPercentage);

            float x = 0; float y = 0; float z = 0;
            
            sessionCorrelationKin = Vector3.zero;
            
            for (int i = 0; i < sck.Count; i++){
                x = x + sck[i].x;
                y = y + sck[i].y;
                z = z + sck[i].z;
            }
            
            //set limits and outputs here
            Vector3 tmp;
            sessionCorrelationKin = new Vector3(x / sck.Count, y / sck.Count, z / sck.Count);
            tmp = sessionCorrelationKin;
            sessionCorrelationKin = new Vector3(Utilities.SetUpperLowerLimit(tmp.x, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.y, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.z, 200f, 0f));
            sessionCorrelationKinAvg = (sessionCorrelationKin.x + sessionCorrelationKin.y + sessionCorrelationKin.z) / 3;
        }

        if (settings.currentRunType == RunType.Imagined){
            //distance
            sdaba.Add(distanceBCI_assisted);
            sdabu.Add(distanceBCI_unassisted);
            sessionDistanceAccuracyBCI_Assisted = 0;
            sessionDistanceAccuracyBCI_Unassisted = 0;
            for (int i = 0; i < sdaba.Count; i++){
                sessionDistanceAccuracyBCI_Assisted = sessionDistanceAccuracyBCI_Assisted + sdaba[i];
                sessionDistanceAccuracyBCI_Unassisted = sessionDistanceAccuracyBCI_Unassisted + sdabu[i];
            }
            sessionDistanceAccuracyBCI_Assisted = sessionDistanceAccuracyBCI_Assisted / sdaba.Count;
            sessionDistanceAccuracyBCI_Unassisted = sessionDistanceAccuracyBCI_Unassisted / sdabu.Count;
            
            //correlation
            scba.Add(correlationAssistedPercentage);
            scbu.Add(correlationPercentage);
            
            float xa = 0; float ya = 0; float za = 0;
            float xu = 0; float yu = 0; float zu = 0;
            
            sessionCorrelationBCI_Assisted = Vector3.zero;
            sessionCorrelationBCI_Unassisted = Vector3.zero;
            
            for (int i = 0; i < scba.Count; i++){
                xa = xa + scba[i].x;
                ya = ya + scba[i].y;
                za = za + scba[i].z;
                xu = xu + scbu[i].x;
                yu = yu + scbu[i].y;
                zu = zu + scbu[i].z;
            }
            
            //limits and oputputs here
            Vector3 tmp;
            sessionCorrelationBCI_Assisted = new Vector3(xa / scba.Count, ya / scba.Count, za / scba.Count);
            tmp = sessionCorrelationBCI_Assisted;
            sessionCorrelationBCI_Assisted = new Vector3(Utilities.SetUpperLowerLimit(tmp.x, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.y, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.z, 200f, 0f));
            sessionCorrelationBCIAvg_Assisted = (sessionCorrelationBCI_Assisted.x + sessionCorrelationBCI_Assisted.y + sessionCorrelationBCI_Assisted.z) / 3;
            
            sessionCorrelationBCI_Unassisted = new Vector3(xu / scbu.Count, yu / scbu.Count, zu / scbu.Count);
            tmp = sessionCorrelationBCI_Unassisted;
            sessionCorrelationBCI_Unassisted = new Vector3(Utilities.SetUpperLowerLimit(tmp.x, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.y, 200f, 0f), Utilities.SetUpperLowerLimit(tmp.z, 200f, 0f));
            sessionCorrelationBCIAvg_Unassisted = (sessionCorrelationBCI_Unassisted.x + sessionCorrelationBCI_Unassisted.y + sessionCorrelationBCI_Unassisted.z) / 3;
            
            
            
        }

        //all metrics combined - 
        // float p = sessionDistanceAccuracyKin + sessionDistanceAccuracyBCI_Assisted + sessionDistanceAccuracyBCI_Unassisted
        //           + sessionCorrelationKinAvg + sessionCorrelationBCIAvg_Assisted + sessionCorrelationBCIAvg_Unassisted;
        //overallPerformanceSession = p / 6;
        
        //no kinematic metrics
        float p = sessionDistanceAccuracyBCI_Assisted + sessionDistanceAccuracyBCI_Unassisted + sessionCorrelationBCIAvg_Assisted + sessionCorrelationBCIAvg_Unassisted;
        overallPerformanceSession = p / 4;
        //TODO ADD STREAK TO OVERAll performance - streak is shown seperately
        //overallPerformanceSession = overallPerformanceSession + (streakBonus + longestStreak);
        
        if (overallPerformanceSession >= 100){
            overallPerformanceSession = 100;
        }
        if (overallPerformanceSession <= 0){
            overallPerformanceSession = 0;
        }
        
        if (OnScoreSessionAction != null){
            OnScoreSessionAction(sessionDistanceAccuracyKin, sessionDistanceAccuracyBCI_Assisted,sessionDistanceAccuracyBCI_Unassisted,
                sessionCorrelationKin,sessionCorrelationKinAvg,
                sessionCorrelationBCI_Assisted,sessionCorrelationBCIAvg_Assisted,
                sessionCorrelationBCI_Unassisted,sessionCorrelationBCIAvg_Unassisted,
            meanSqErrorSum,meanSqErrorSumAssisted,meanSquareErrorAverage,meanSquareErrorAverageAssisted,
                overallPerformanceSession, streakBonusSession);
            
            //Debug.Log(streakBonusSession);
        }
    }

    #endregion
    
    #region Reset Scores

    //after a block
    private void ResetScores(){
        //reset target counts for block
        targetsHitBlock.totalTargetsPresented = 0;
        targetsHitBlock.totalTargetsHit = 0;
        targetsHitBlock.kinTargetsPresented = 0;
        targetsHitBlock.totalKinHit = 0;
        targetsHitBlock.t1_kin = 0;
        targetsHitBlock.t2_kin = 0;
        targetsHitBlock.t3_kin = 0;
        targetsHitBlock.t4_kin = 0;
        targetsHitBlock.imgTargetsPresented = 0;
        targetsHitBlock.totalImgHit = 0;
        targetsHitBlock.t1_img = 0;
        targetsHitBlock.t2_img = 0;
        targetsHitBlock.t3_img = 0;
        targetsHitBlock.t4_img = 0;
        
        //reset score for block
        targetDistanceKin = 0;
        trialDistanceKin.Clear();
        totalDistanceKin = 0;
        distanceKin = 0;

        targetDistanceBCI = 0;
        trialDistanceBCI.Clear();
        totalDistanceBCI = 0;
        distanceBCI_assisted = 0;

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

    //after a session
    public void ResetSession(){
        //DISTANCE
        sdak.Clear();
        sdaba.Clear();
        sdabu.Clear();
        sessionDistanceAccuracyKin = 0;
        sessionDistanceAccuracyBCI_Assisted = 0;
        sessionDistanceAccuracyBCI_Unassisted = 0;

        //CORRELATION
        sck.Clear();
        scba.Clear();
        scbu.Clear();
        sessionCorrelationKin = Vector3.zero;
        sessionCorrelationKinAvg = 0;
        sessionCorrelationBCI_Assisted = Vector3.zero;
        sessionCorrelationBCIAvg_Assisted = 0;
        sessionCorrelationBCI_Unassisted = Vector3.zero;
        sessionCorrelationBCIAvg_Unassisted = 0;
        
        //MEAN SQUARE
        frameAccumulation = 0;
        meanSqErrorCurrent = Vector3.zero;
        meanSqErrorCurrentAssisted = Vector3.zero;
        meanSqErrorAccumulated = Vector3.zero;
        meanSqErrorAccumulatedAssisted = Vector3.zero;
        meanSqErrorAverage = Vector3.zero;
        meanSqErrorAverageAssisted = Vector3.zero;
        meanSqErrorTrialAverage.Clear();
        meanSqErrorTrialAverageAssisted.Clear();
        meanSqErrorSum = Vector3.zero;
        meanSqErrorSumAssisted = Vector3.zero;
        meanSquareErrorAverage = Vector3.zero;
        meanSquareErrorAverageAssisted = Vector3.zero;
    }

    #endregion

    
    private void SaveScore(){
        //--------BLOCK
        scoreBlockData.name = Settings.instance.sessionName;
        scoreBlockData.sessionNumber = Settings.instance.sessionNumber;

        scoreBlockData.run = runNumber.ToString();
        scoreBlockData.block = blockNumber.ToString();
        scoreBlockData.runType = runType;

        scoreBlockData.assistancePercentage = Settings.instance.BCI_ControlAssistance;
        
        scoreBlockData.distanceAccuracyKinematic = distanceKin;
        scoreBlockData.distanceAccuracyBCI_Assisted = distanceBCI_assisted;
        
        scoreBlockData.distanceAccuracyBCI_Unassisted = distanceBCI_unassisted;
        scoreBlockData.distanceAccuracyBCI_Assisted = distanceBCI_assisted;

        scoreBlockData.correlationUnassisted = correlationPercentage;
        scoreBlockData.correlationAssisted = correlationAssistedPercentage;
        
        scoreBlockData.correlationDisplay = correlationPercentage_Display;
        scoreBlockData.correlationAssistedDisplay = correlationAssistedPercentage_Display;

        scoreBlockData.overallPerformance = overallPerformanceBlock; //not sure this is working..
        scoreBlockData.streakBonus = streakBonusBlock;
        
        //---------SESSION
        scoreSessionData.name = Settings.instance.sessionName;
        scoreSessionData.sessionNumber = Settings.instance.sessionNumber;
        
        scoreSessionData.assistanceDecrease = Settings.instance.assistanceModifier;

        scoreSessionData.distanceAccuracyKin = sessionDistanceAccuracyKin;
        scoreSessionData.distanceAccuracyBCI_Assisted = sessionDistanceAccuracyBCI_Assisted;
        scoreSessionData.distanceAccuracyBCI_Unassisted = sessionDistanceAccuracyBCI_Unassisted;

        scoreSessionData.correlationKin = sessionCorrelationKin;
        scoreSessionData.correlationKinAvg = sessionCorrelationKinAvg;
        scoreSessionData.correlationBCI_Assisted = sessionCorrelationBCI_Assisted;
        scoreSessionData.correlationBCIAvg_Assisted = sessionCorrelationBCIAvg_Assisted;
        scoreSessionData.correlationBCI_Unassisted = sessionCorrelationBCI_Unassisted;
        scoreSessionData.correlationBCIAvg_Unassisted = sessionCorrelationBCIAvg_Unassisted;

        scoreSessionData.meanSqErrorSum = meanSqErrorSum;
        scoreSessionData.meanSqErrorSumAssisted = meanSqErrorSumAssisted;
        scoreSessionData.meanSquareErrorAverage = meanSquareErrorAverage;
        scoreSessionData.meanSquareErrorAverageAssisted = meanSquareErrorAverageAssisted;

        scoreSessionData.overallPerformance = overallPerformanceSession;
        scoreSessionData.streakBonus = streakBonusSession;

        //write JSON
        JSONWriter jWriter = new JSONWriter();
        jWriter.OutputScoreBlockJSON(scoreBlockData);
        if (debugScore){
            print("block score written------------------------");
        }
        
        //jWriter = new JSONWriter();
        jWriter.OutputScoreSessionJSON(scoreSessionData);
        if (debugScore){
            print("session score written------------------------");
        }
        
        jWriter.OutputTargetHitBlockJSON(targetsHitBlock);
        if (debugScore){
            print("target block score written------------------------");
        }
        jWriter.OutputTargetHitSessionSON(targetsHitSession);
        if (debugScore){
            print("target SESSION score written------------------------");
        }

        //SEND SCORE DATA OBJECTS
        if (OnScoreBlockObjectAction!=null){
            OnScoreBlockObjectAction(scoreBlockData);
        }
        if (OnScoreSessionObjectAction!=null){
            OnScoreSessionObjectAction(scoreSessionData);
        }
    }
    
    
}
