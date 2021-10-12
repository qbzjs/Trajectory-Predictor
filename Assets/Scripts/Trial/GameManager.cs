using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("-- DEBUGGING ----")]
    public bool debugUI;
    public bool debugTimingSimple = true;
    public bool debugTimingDetailed = true;
    public TextMeshProUGUI trialDebugUI;
    
    private RunManager runManager;
    private BlockManager blockManager;
    private TrialSequencer trialSequencer;
    
    [Header("-- GAME STATE ----")]
    public bool paused = false; //embed this int coroutines to pause trials

    [Space(10)]
    [Range(-5, 5)] 
    public float gameSpeed = 0;
    
    [Space(10)]
    public GameStatus gameStatus;
    public GameStatus runStatus;
    public GameStatus blockStatus;
    public TrialEventType trialPhase;
    
    [Header("-- GAME PROGRESSION ----")]
    public int runIndex;
    public int runTotal;
    public int blockIndex;
    public int blockTotal;
    public int trialSequenceIndex;
    public int trialSequenceTotal;
    public int totalTrialsProgress;
    public int totalTrials;
    [Space(5)]
    public int activeTarget;
    public float activePhaseDuration;


    [Header("-- PARADIGM & SESSION SETUP ----")] 
    public TrialParadigm trialParadigm;
    public int targetCount;
    public int trialRepetitions;
    public SequenceType sequenceType;
    public Handedness handedness;
    public bool actionObservation;
    [Space(5)]
    public int blockCountdown;
    public int visibleCountdown;
    [Space(5)]
    public int interRunRestPeriod;

    [Header("-- TRIAL TIMINGS ----")] 
    public float preTrialWaitPeriod;
    public float fixationPeriod;
    public float indicationPeriod;
    public float observationPeriod;
    public float targetPresentationPeriod;
    public float restPeriodMinimum;
    public float restPeriodMaximum;
    public float postTrialWaitPeriod;
    public float postBlockRestPeriod;
    public float postRunRestPeriod;
    
    [Header("NETWORK TRIGGER")]
    public int UDP_Trigger;
    
    #region Event Subscriptions

    private void OnEnable(){
        InputManager.OnUserInputAction += InputManagerOnUserInputAction;
    }

    private void OnDisable(){
        InputManager.OnUserInputAction += InputManagerOnUserInputAction;
    }

    private void InputManagerOnUserInputAction(UserInputType inputType){
        if (inputType == UserInputType.Start){
            StartTrial();
        }
    }

    #endregion

    #region Initialise
    private void Awake(){
        instance = this;
        runManager = GetComponent<RunManager>();
        blockManager = GetComponent<BlockManager>();
        trialSequencer = GetComponent<TrialSequencer>();
    }
    void Start(){
        totalTrialsProgress = 0;
        // UpdateProgressionUI();
    }
    public void InitialiseSession(){
        // variables initialised from settings then pushed to run, block and trial sequencer
        runManager.runTotal = runTotal;
        blockManager.blockTotal = blockTotal;
        trialSequencer.sequenceLength = trialSequenceTotal;
        
        trialSequencer.targetCount = targetCount;
        trialSequencer.repetitions = trialRepetitions;

        blockManager.countdown = blockCountdown;
        blockManager.visibleCountdownMaximum = visibleCountdown;

        runManager.interRunRestPeriod = interRunRestPeriod;

        trialSequencer.preTrialWaitPeriod = preTrialWaitPeriod;
        trialSequencer.fixationPeriod = fixationPeriod;
        trialSequencer.indicationPeriod = indicationPeriod;
        trialSequencer.observationPeriod = observationPeriod;
        trialSequencer.targetPresentationPeriod = targetPresentationPeriod;
        trialSequencer.restPeriodMin = restPeriodMinimum;
        trialSequencer.restPeriodMax = restPeriodMaximum;
        trialSequencer.postTrialWaitPeriod = postTrialWaitPeriod;
        blockManager.postBlockWaitPeriod = postBlockRestPeriod;
        runManager.postRunWaitPeriod = postRunRestPeriod;

        trialSequencer.actionObservation = actionObservation;
        
        runManager.InitialiseRun();
        blockManager.InitialiseBlock();
        trialSequencer.InitialiseTrial();
        totalTrials = TotalTrials();
        UpdateProgressionUI();
    }


    
    
    #endregion


    #region User Input

    public void StartTrial(){
        runManager.StartTrial();
    }

    #endregion

    private void Update(){
        //GameSpeed(gameSpeed); //needs to be input from slider or event
    }

    #region System Functions
    //function to reset the timing
    public void PauseTrials(bool t){
        paused = t;
    }
    public void ResetTrials(){
        runManager.runIndex = 0;
        blockManager.blockIndex = 0;
        runManager.runsComplete = false;
        blockManager.blocksComplete = false;
    }
    //function to reset the loaded game (not applicable yet)
    public void SetGameSpeed(float s){
        gameSpeed = s;
    }
    #endregion
    
    #region Trial Tracking

    public void RunTracker(GameStatus status, int total, int index){
        runStatus = status;
        runTotal = total;
        runIndex = index;
        UpdateProgressionUI();
    }
    public void BlockTracker(GameStatus status, int total, int index){
        blockStatus = status;
        blockTotal = total;
        blockIndex = index;
        UpdateProgressionUI();
    }
    public void TrialTracker(TrialEventType eType, int total, int index, int targetNum, float dur){
        trialSequenceTotal = total;
        trialSequenceIndex = index;
        activeTarget = targetNum;
        trialPhase = eType;
        activePhaseDuration = dur;
        if (eType == TrialEventType.PostTrialPhase){
            totalTrialsProgress++;
        }
        UpdateProgressionUI();
    }

    private int TotalTrials(){
        totalTrials = Settings.instance.trialsPerSession;
        return totalTrials;
    }

    // private void TotalTrialsProgression(){
    //     totalTrialsProgress++;
    // }

    private void UpdateProgressionUI(){
        if (debugUI){
            trialDebugUI.transform.parent.gameObject.SetActive(true);
            trialDebugUI.text = "ACTIVE TARGET: " + activeTarget + " - TRIAL PHASE: " + trialPhase.ToString() + " - TIMING: " + activePhaseDuration.ToString() +"s";
            UI_DisplayText.instance.SetRunProgress(runIndex,runTotal);
            UI_DisplayText.instance.SetBlockProgress(blockIndex,blockTotal);
            UI_DisplayText.instance.SetTrialProgress(trialSequenceIndex,trialSequenceTotal);
            UI_DisplayText.instance.SetTrialTotalProgress(totalTrialsProgress,totalTrials);
        }else
        {
            trialDebugUI.transform.parent.gameObject.SetActive(false);
        }
    }

    #endregion
    

}