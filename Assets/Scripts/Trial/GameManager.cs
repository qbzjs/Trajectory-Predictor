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

    [Range(-0.01f, 0.01f)] 
    public float gameSpeed = 0;
    
    public GameStatus gameStatus;
    public GameStatus runStatus;
    public GameStatus blockStatus;
    public TrialEventType trialPhase;

    [Header("Game Progression")]
    public int runIndex;
    public int runTotal;
    public int blockIndex;
    public int blockTotal;
    public int trialIndex;
    public int trialTotal;
    
    public int targetNumber;
    
    public float phaseDuration;
    
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
    #endregion


    #region User Input

    public void StartTrial(){
        runManager.StartTrial();
    }

    #endregion

    private void Update(){
        GameSpeed(gameSpeed); //needs to be input from slider or event
    }

    #region System Functions
    //function to reset the timing
    public void PauseTrials(bool t){
        paused = t;
    }
    public void GameSpeed(float speed){
        runManager.postRunWaitDuration = runManager.postRunWaitDuration += speed;
        
        blockManager.postBlockWaitDuration = blockManager.postBlockWaitDuration += speed;

        trialSequencer.preTrialWaitDuration = trialSequencer.preTrialWaitDuration += speed;
        trialSequencer.fixationDuration = trialSequencer.fixationDuration += speed;
        trialSequencer.arrowDuration = trialSequencer.arrowDuration += speed;
        trialSequencer.observationDuration = trialSequencer.observationDuration += speed;
        trialSequencer.targetDuration = trialSequencer.targetDuration += speed;
        trialSequencer.restDuration = trialSequencer.restDuration += speed;
        trialSequencer.restDurationMin = trialSequencer.restDurationMin += speed;
        trialSequencer.restDurationMax = trialSequencer.restDurationMax += speed;
        trialSequencer.postTrialWaitDuration = trialSequencer.postTrialWaitDuration += speed;
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
        targetNumber = targetNum;
        trialPhase = eType;
        phaseDuration = dur;
    }


    #endregion
}