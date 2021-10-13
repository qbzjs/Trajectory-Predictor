using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using JetBrains.Annotations;
using TMPro;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

    private GameManager gameManager;
    private TrialSequencer trialSequencer;

    public GameStatus blockStatus;

    const int startingBlockTrigger = 101;

    public bool useCountdown = true;
    public int blockTotal = 4;
    public int blockIndex = 0;
    public int countdown = 10;
    public int visibleCountdownMaximum;
    public float postBlockWaitPeriod = 1f;

    [Header("---------")]
    public bool blocksComplete = false;

    [HideInInspector] 
    public BlockSequenceGenerator blockSequence;
    
    public delegate void BlockAction(GameStatus status, int blockTotal,int blockIndex);
    public static event BlockAction OnBlockAction;

    #region Initialise

    private void Awake(){
        instance = this;
        gameManager = GetComponent<GameManager>();
        trialSequencer = GetComponent<TrialSequencer>();
        blockTotal = gameManager.blockTotal;
    }
    void Start(){
        blockIndex = 0;
        blockStatus = GameStatus.Ready;
        blocksComplete = false;
        InitialiseBlock();
    }
    //call from settings as well so updates every change
    public void InitialiseBlock(){
        blockSequence = gameManager.TriggerSequenceGenerator(blockTotal, startingBlockTrigger); //generate trigger sequences
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
    }

    #endregion

    #region User Input
    public void StartTrial(){
        if (blockIndex < blockTotal){
            blocksComplete = false;
            InitialiseBlock();
            StartCoroutine(RunTrialSequence());
        }
    }
    #endregion

    #region Block Logic
    
    private IEnumerator RunTrialSequence(){
        
        blockIndex++;
        
        if (useCountdown){
            blockStatus = GameStatus.Countdown;
            UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
            
            CountdownTimer.instance.timeUp = false;
            CountdownTimer.instance.SetCountdown(countdown + 1, visibleCountdownMaximum + 1);
            
            yield return new WaitUntil(() => CountdownTimer.instance.timeUp == true);
            
        }
        
        Debug.Log("--------BlockManager - Started Trial Sequence------------------");
        
        //UDP ON
        gameManager.SendUDP_byte(blockSequence.sequenceStartTrigger[blockIndex-1],"Block Started");

        blockStatus = GameStatus.RunningTrials;
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
        
        trialSequencer.StartTrialSequence();
        yield return new WaitUntil(() => trialSequencer.sequenceComplete);

        yield return new WaitForSeconds(gameManager.SpeedCheck(postBlockWaitPeriod));

        blockStatus = GameStatus.BlockComplete;
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);

        //Debug.Log("__BLOCK END ______________________________________________");
        UpdateBlockStatus(GameStatus.Debug,blockTotal,blockIndex);
        
        //UDP OFF
        gameManager.SendUDP_byte(blockSequence.sequenceEndTrigger[blockIndex-1], "Block Ended");

        if (blockIndex >= blockTotal){
            blocksComplete = true; // tells run manager to progress
            blockStatus = GameStatus.AllBlocksComplete;
            blockIndex = 0;
            UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
        }

        yield return new WaitForSeconds(gameManager.SpeedCheck(gameManager.postRunRestPeriod *4));
        blockStatus = GameStatus.WaitingForInput;
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
        
        //automated block starts???
        if (gameManager.automateInput && !gameManager.trialsActive){
            // yield return new WaitUntil(() => !gameManager.trialsActive);
            // yield return new WaitForSeconds(gameManager.SpeedCheck(gameManager.postRunRestPeriod *2));
            gameManager.StartTrial();
        }
    }

    #endregion


    #region Status & Event Updates

    private void UpdateBlockStatus(GameStatus status, int total, int index){
        if (OnBlockAction != null && status != GameStatus.Debug){
            OnBlockAction(status, total, index);
        }

        if (status != GameStatus.Debug){
            gameManager.BlockTracker(status,total,index);
        }
        
        if (gameManager.debugTimingDetailed){
            Debug.Log("------ Block Status: " + status + "Block Progress: " + index+"/"+total);
        }

        if (gameManager.debugTimingSimple && status == GameStatus.Debug){
            Debug.Log("__BLOCK END ______________________________________________");
        }
    }

    #endregion

}