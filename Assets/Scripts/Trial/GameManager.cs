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

    public GameStatus gameStatus;
    public GameStatus runStatus;
    public GameStatus blockStatus;
    public TrialEventType trialPhase;

    [Header("Game Progression")]
    public int runTotal;
    public int runIndex;
    public int blockTotal;
    public int blockIndex;
    public int trialTotal;
    public int trialIndex;
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

    #region TrialTracking

    public void RunTracker(){
        
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