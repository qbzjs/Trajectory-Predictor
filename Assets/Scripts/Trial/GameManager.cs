using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool debug = true;
    
    private RunManager runManager;
    private BlockManager blockManager;
    private TrialSequencer trialSequencer;
    
    [Header("Game State")]
    public bool paused = false; //embed this int coroutines to pause trials

    [Space(5)]
    [Range(-5, 5)] 
    public float gameSpeed = 0;
    
    [Space(5)]
    public GameStatus gameStatus;
    public GameStatus runStatus;
    public GameStatus blockStatus;
    public TrialEventType trialPhase;
    
    [Header("--Game Progression--")]
    public int runIndex;
    public int runTotal;
    public int blockIndex;
    public int blockTotal;
    public int trialIndex;
    public int trialTotal;
    [Space(5)]
    public int activeTarget;
    public float activePhaseDuration;
    
    [Header("Paradigm & Session Setup")] 
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

    [Header("Trial Timings")] 
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
    
    [Header("Network Trigger")]
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

    }
    public void InitialiseSession(){
        // variables initialised from settings then pushed to run, block and trial sequencer
        runManager.runTotal = runTotal;
        blockManager.blockTotal = blockTotal;
        trialSequencer.sequenceLength = trialTotal;
        
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
    public void GameSpeed(float speed){
        runManager.postRunWaitPeriod = runManager.postRunWaitPeriod += speed;
        
        blockManager.postBlockWaitPeriod = blockManager.postBlockWaitPeriod += speed;

        trialSequencer.preTrialWaitPeriod = trialSequencer.preTrialWaitPeriod += speed;
        trialSequencer.fixationPeriod = trialSequencer.fixationPeriod += speed;
        trialSequencer.indicationPeriod = trialSequencer.indicationPeriod += speed;
        trialSequencer.observationPeriod = trialSequencer.observationPeriod += speed;
        trialSequencer.targetPresentationPeriod = trialSequencer.targetPresentationPeriod += speed;
        trialSequencer.restPeriod = trialSequencer.restPeriod += speed;
        trialSequencer.restPeriodMin = trialSequencer.restPeriodMin += speed;
        trialSequencer.restPeriodMax = trialSequencer.restPeriodMax += speed;
        trialSequencer.postTrialWaitPeriod = trialSequencer.postTrialWaitPeriod += speed;
    }
    public void ResetTrials(){
        runManager.runIndex = 0;
        blockManager.blockIndex = 0;
        runManager.runsComplete = false;
        blockManager.blocksComplete = false;
    }
    //function to reset the loaded game (not applicable yet)
    #endregion
    
    #region Trial Tracking

    public void RunTracker(GameStatus status, int total, int index){
        runStatus = status;
        runTotal = total;
        runIndex = index;
    }
    public void BlockTracker(GameStatus status, int total, int index){
        blockStatus = status;
        blockTotal = total;
        blockIndex = index;
    }
    public void TrialTracker(int total, int index, int targetNum, TrialEventType eType, float dur){
        trialTotal = total;
        trialIndex = index;
        activeTarget = targetNum;
        trialPhase = eType;
        activePhaseDuration = dur;
    }


    #endregion
    
    #region speed setter //return to this concept

    public void SetGameSpeed(float s){
        //print(gameSpeed);
        float lastValue = s / 100;
        gameSpeed = s/100;
        if (s > 0){
            if (gameSpeed > lastValue){
                trialSequencer.preTrialWaitPeriod = trialSequencer.preTrialWaitPeriod + gameSpeed;
                trialSequencer.fixationPeriod = trialSequencer.fixationPeriod + gameSpeed;
                trialSequencer.indicationPeriod = trialSequencer.fixationPeriod + gameSpeed;
                trialSequencer.observationPeriod = trialSequencer.observationPeriod + gameSpeed;
                trialSequencer.targetPresentationPeriod = trialSequencer.targetPresentationPeriod + gameSpeed;
                trialSequencer.restPeriodMin = trialSequencer.restPeriodMin + gameSpeed;
                trialSequencer.restPeriodMax = trialSequencer.restPeriodMax + gameSpeed;
                trialSequencer.postTrialWaitPeriod = trialSequencer.postTrialWaitPeriod + gameSpeed;
                blockManager.postBlockWaitPeriod = blockManager.postBlockWaitPeriod + gameSpeed;
                runManager.postRunWaitPeriod = runManager.postRunWaitPeriod + gameSpeed;
            }
        }
        else{
            if (gameSpeed < lastValue){
                trialSequencer.preTrialWaitPeriod =trialSequencer.preTrialWaitPeriod - gameSpeed;
                trialSequencer.fixationPeriod = trialSequencer.fixationPeriod - gameSpeed;
                trialSequencer.indicationPeriod =trialSequencer.fixationPeriod - gameSpeed;
                trialSequencer.observationPeriod = trialSequencer.observationPeriod - gameSpeed;
                trialSequencer.targetPresentationPeriod = trialSequencer.targetPresentationPeriod - gameSpeed;
                trialSequencer.restPeriodMin = trialSequencer.restPeriodMin - gameSpeed;
                trialSequencer.restPeriodMax = trialSequencer.restPeriodMax - gameSpeed;
                trialSequencer.postTrialWaitPeriod = trialSequencer.postTrialWaitPeriod - gameSpeed;
                blockManager.postBlockWaitPeriod = blockManager.postBlockWaitPeriod - gameSpeed;
                runManager.postRunWaitPeriod = runManager.postRunWaitPeriod - gameSpeed;
            }

        }

        // InitialiseSession();
    }

    #endregion
}