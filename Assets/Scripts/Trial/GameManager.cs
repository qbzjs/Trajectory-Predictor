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
    public bool debugGameEvents = true;

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
        

    }

    public void InitialiseComponents(){
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
            InitialiseComponents();
            GameEvent(GameStatus.Initialised);
            SetTrialActiveStatus(true);
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
            GameEvent(GameStatus.Paused);
            UpdateGameStatusUI("--Paused--");
        }
        else{
            paused = false;
            GameEvent(GameStatus.Unpaused);
            UpdateGameStatusUI("--Unpaused--");
        }
    }
    public void PauseTrials(bool p){
        if (p){
            paused = true;
            GameEvent(GameStatus.Paused);
            UpdateGameStatusUI("--Paused--");
        }
        else{
            paused = false;
            GameEvent(GameStatus.Unpaused);
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
        GameEvent(GameStatus.Reset);
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

        if (ts < 0f){
            ts = 0.01f;
        }
        return ts;
    }
    public BlockSequenceGenerator TriggerSequenceGenerator(int total, int triggerStart){
        BlockSequenceGenerator sequence = new BlockSequenceGenerator();
        sequence.GenerateSequence(total, triggerStart);
        return sequence;
    }
    public void SendUDP_byte(int t, string n){
        UDP_Trigger = t;
        if (debugUDPTriggers){
            Debug.Log(n + " UDP Trigger Sent: " + UDP_Trigger);
        }
        UDPClient.instance.SendData((byte)UDP_Trigger);
    }
    
    #endregion
    
    #region Trial Tracking

    public void SetTrialActiveStatus(bool t){
        trialsActive = t;
        if (trialsActive){
            GameEvent(GameStatus.RunningTrials);
            UpdateGameStatusUI(GameStatus.RunningTrials);
        }
        else{
            GameEvent(GameStatus.WaitingForInput);
            UpdateGameStatusUI(GameStatus.Ready);
        }
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
        UpdateProgressionUI();
    }

    public void TrialTracker(TrialEventType eType, int total, int index, int targetNum, float dur){
        trialSequenceTotal = total;
        trialSequenceIndex = index;
        activeTarget = targetNum;
        trialPhase = eType;
        activePhaseDuration = dur;
        if (trialPhase == TrialEventType.PostTrialPhase){
            totalTrialsProgress++;
        }
        UpdateProgressionUI();
    }

    //use as overall status - maybe later..for FSM
    public void SetSystemStatus(GameStatus gs){
        if (gs == GameStatus.WaitingForInput){
            SetTrialActiveStatus(false);
        }
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

    #region Events

    public delegate void GameActions(GameStatus eventType); 
    public static event GameActions OnGameAction;
    
    public delegate void RunActions(GameStatus eventType,float lifeTime, int runIndex, int runTotal);
    public static event RunActions OnRunAction;
    public delegate void BlockActions(GameStatus eventType,float lifeTime, int blockIndex, int blockTotal);
    public static event BlockActions OnBlockAction;

    public delegate void TrialActions(TrialEventType eventType, int targetNum, float lifeTime, int index, int total);
    public static event TrialActions OnTrialAction;

    public void GameEvent(GameStatus e){
        Debug.Log("GAME event: " + e);
        if (OnGameAction != null){
            OnGameAction(e);
        }
    }
    public void RunEvent(GameStatus e, float lifeTime){
        Debug.Log("RUN event: " + e);
        if (OnRunAction != null){
            OnRunAction(e, lifeTime, runIndex, runTotal);
        }

        if (e == GameStatus.RunComplete || e == GameStatus.AllRunsComplete){
            GameEvent(GameStatus.DisplayRunMenu);
        }
    }
    public void BlockEvent(GameStatus e, float lifeTime){
        Debug.Log("RUN event: " + e);
        if (OnBlockAction != null){
            OnBlockAction(e, lifeTime, blockIndex, blockTotal);
        }
        
        if (e == GameStatus.BlockComplete || e == GameStatus.AllBlocksComplete){
            GameEvent(GameStatus.DisplayBlockMenu);
        }
    }
    public void TrialEvent(TrialEventType e, int tNum, float lifeTime, int i, int tot  ){
        Debug.Log("TRIAL event: " + e);
        if (OnTrialAction != null){
            OnTrialAction(e, tNum, lifeTime,i,tot);
        }
    }

    #endregion
}