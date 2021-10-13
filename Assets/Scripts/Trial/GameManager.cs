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
    public bool automateInput;
    public bool debugTimingSimple = true;
    public bool debugTimingDetailed = true;
    public bool debugUDPTriggers = true;

    private RunManager runManager;
    private BlockManager blockManager;
    private TrialSequencer trialSequencer;

    [Header("-- GAME STATE ----")] 
    public bool trialsActive;
    public bool paused = false; //embed this into coroutines to pause trials

    [Space(10)]
    public GameStatus gameStatus;
    public GameStatus runStatus;
    public GameStatus blockStatus;
    public TrialEventType trialPhase;
    
    [Space(10)]
    [Range(-20, 20)] 
    public float gameSpeed = 0;
    
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
    // const int startingRun = 1001;
    // const int startingBlock = 101;
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
        if (inputType == UserInputType.Stop){
            
        }
        if (inputType == UserInputType.Pause){
            PauseTrials();
        }
        if (inputType == UserInputType.Reset){
            ResetTrials();
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
        UpdateGameStatusUI(GameStatus.Ready);
        PauseTrials(false);
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
        //trialsactive set by blocks - set false after each block completes
        if (!trialsActive && !runManager.runsComplete){
            trialsActive = true;
            UpdateGameStatusUI(GameStatus.RunningTrials);
            runManager.StartTrial();
        }

        if (trialsActive){
            Debug.Log("---Input not available - Trials Started...");
        }
        if (runManager.runsComplete){
            Debug.Log("---Runs Completed - Reset to begin new session...");
        }
    }

    public void PauseTrials(){
        if (!paused){
            paused = true;
            UpdateGameStatusUI("--Paused--");
        }
        else{
            paused = false;
            UpdateGameStatusUI("--Unpaused--");
        }
    }
    public void PauseTrials(bool p){
        if (p){
            paused = true;
            UpdateGameStatusUI("--Paused--");
        }
        else{
            paused = false;
            UpdateGameStatusUI("--Unpaused--");
        }
    }

    #endregion
    

    #region System Functions
    public void ResetTrials(){
        runManager.runIndex = 0;
        blockManager.blockIndex = 0;
        runManager.runsComplete = false;
        blockManager.blocksComplete = false;
        totalTrialsProgress = 0;
        trialsActive = false;
        UpdateProgressionUI();
        Debug.Log("----------Trial Session Reset!------------------");
    }
    //function to reset the loaded game (not applicable yet)
    public void SetGameSpeed(float s){
        gameSpeed = s;
    }
    public float SpeedCheck(float t){
        float ts = t;
        float gs = GameManager.instance.gameSpeed/10;
        if (gs < 0){
            gs = Mathf.Abs(gs);
            ts = ts + gs;
        }
        else if (gs > 0){
            ts = ts - gs;
        }

        if (ts < 0){
            ts = 0;
        }
        return ts;
    }
    public BlockSequenceGenerator TriggerSequenceGenerator(int total, int triggerStart){
        BlockSequenceGenerator sequence = new BlockSequenceGenerator();
        sequence.GenerateSequence(total, triggerStart);
        return sequence;
    }
    public void SendUDP_byte(int t, string n){
        if (debugUDPTriggers){
            Debug.Log(n + " UDP Trigger Sent: " + t);
        }
        UDPClient.instance.SendData((byte)t);
    }
    
    #endregion
    
    #region Trial Tracking

    public void SetTrialActiveStatus(bool t){
        trialsActive = t;
    }
    public void RunTracker(GameStatus status, int total, int index){
        runStatus = status;
        runTotal = total;
        runIndex = index;
        if (status == GameStatus.AllRunsComplete){
            Debug.Log("----------All Runs Completed!------------------");
        }
        UpdateProgressionUI();
    }
    public void BlockTracker(GameStatus status, int total, int index){
        blockStatus = status;
        blockTotal = total;
        blockIndex = index;
        if (status == GameStatus.WaitingForInput){
            SetTrialActiveStatus(false);
            UpdateGameStatusUI(GameStatus.Ready);
            Debug.Log("Waiting for Input");
        }
        
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

    private void UpdateProgressionUI(){
        UI_DisplayText.instance.SetRunProgress(runIndex,runTotal);
        UI_DisplayText.instance.SetBlockProgress(blockIndex,blockTotal);
        UI_DisplayText.instance.SetTrialProgress(trialSequenceIndex,trialSequenceTotal);
        UI_DisplayText.instance.SetTrialTotalProgress(totalTrialsProgress,totalTrials);
        
        string phaseDisplay = "ACTIVE TARGET: " + activeTarget + " - TRIAL PHASE: " + trialPhase.ToString() + " - TIMING: " + activePhaseDuration.ToString("f2") +"s";
        UI_DisplayText.instance.SetTrialPhaseDetail(phaseDisplay);
    }

    private void UpdateGameStatusUI(GameStatus gs){
        if (gs == GameStatus.Ready){
            UI_DisplayText.instance.SetStatus(gs,"System Ready");
        }
        if (gs == GameStatus.RunningTrials){
            UI_DisplayText.instance.SetStatus(gs,"Running Trials");
        }
    }
    private void UpdateGameStatusUI(string p){
        UI_DisplayText.instance.SetPause(p);
        
    }
    #endregion
    

}