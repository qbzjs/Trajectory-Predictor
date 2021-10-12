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

    const int startingBlock = 101;

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
        InitialiseBlock();
    }
    //call from settings as well so updates every change
    public void InitialiseBlock(){
        blockStatus = GameStatus.Ready;
        blocksComplete = false;
        BlockSequenceGenerator();
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
    }

    #endregion

    #region User Input
    public void StartTrial(){
        if (blockIndex < blockTotal){
            InitialiseBlock();
            StartCoroutine(RunTrialSequence());
        }
    }
    #endregion

    #region Block Logic
    
    private IEnumerator RunTrialSequence(){
        if (useCountdown){
            blockStatus = GameStatus.Countdown;
            UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
            
            CountdownTimer.instance.timeUp = false;
            CountdownTimer.instance.SetCountdown(countdown + 1, visibleCountdownMaximum + 1);
            
            yield return new WaitUntil(() => CountdownTimer.instance.timeUp == true);
            
        }
        
        blockStatus = GameStatus.RunningTrials;
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
        
        trialSequencer.StartTrialSequence();
        yield return new WaitUntil(() => trialSequencer.sequenceComplete);

        yield return new WaitForSeconds(postBlockWaitPeriod);

        blockIndex++;
        
        blockStatus = GameStatus.BlockComplete;
        UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
        
        //Debug.Log("__BLOCK END ______________________________________________");
        UpdateBlockStatus(GameStatus.Debug,blockTotal,blockIndex);

        if (blockIndex >= blockTotal){
            blocksComplete = true;
            blockStatus = GameStatus.AllBlocksComplete;
            //UpdateBlockStatus(blockStatus,blockTotal,blockIndex);
            blockIndex = 0;
        }
    }


    public void BlockSequenceGenerator(){
        blockSequence = new BlockSequenceGenerator();
        blockSequence.GenerateSequence(blockTotal, startingBlock);
        // for (int i = 0; i < blockSequence.sequenceStartTrigger.Length; i++){
        //     Debug.Log(blockSequence.sequenceStartTrigger[i]);
        //     Debug.Log(blockSequence.sequenceEndTrigger[i]);
        // }
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
    
    private void SendUDP_byte(int t){
        Debug.Log("Block Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }

    #endregion

}