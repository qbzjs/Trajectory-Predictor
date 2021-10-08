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

    public GameStatus gameStatus;

    const int startingBlock = 101;

    public bool useCountdown = true;
    public int blockTotal = 4;
    public int blockIndex = 0;
    public int countdown = 10;
    public int maxCountdownDisplay;
    public float postBlockWaitDuration = 1f;

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
    }
    void Start(){
        blockIndex = 0;
        InitialiseBlock();
    }
    //call from settings as well so updates every change
    public void InitialiseBlock(){
        gameStatus = GameStatus.Ready;
        blocksComplete = false;
        BlockSequenceGenerator();
        UpdateBlockStatus(gameStatus,blockTotal,blockIndex);
    }

    #endregion

    #region UserInput
    public void StartTrial(){
        //TODO add check if block index is finished
        InitialiseBlock();
        StartCoroutine(RunTrialSequence());
    }
    #endregion

    #region Block Logic
    
    private IEnumerator RunTrialSequence(){
        if (useCountdown){
            gameStatus = GameStatus.Countdown;
            UpdateBlockStatus(gameStatus,blockTotal,blockIndex);
            
            CountdownTimer.instance.timeUp = false;
            CountdownTimer.instance.SetCountdown(countdown + 1, maxCountdownDisplay + 1);
            
            yield return new WaitUntil(() => CountdownTimer.instance.timeUp == true);
            
        }
        
        gameStatus = GameStatus.RunningTrials;
        UpdateBlockStatus(gameStatus,blockTotal,blockIndex);
        
        trialSequencer.StartTrialSequence();
        yield return new WaitUntil(() => trialSequencer.sequenceComplete);
        

        yield return new WaitForSeconds(postBlockWaitDuration);

        blockIndex++;
        
        gameStatus = GameStatus.BlockComplete;
        UpdateBlockStatus(gameStatus,blockTotal,blockIndex);

        if (blockIndex >= blockTotal){
            blocksComplete = true;
            //all blocks finished in run - tell run manager
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
        if (OnBlockAction != null){
            OnBlockAction(status, total, index);
        }
        gameManager.BlockTracker(status,total,index);
        if (gameManager.debug){
            Debug.Log("------ Block Status: " + status + "Block Progress: " + index+"/"+total);
        }
    }
    
    private void SendUDP_byte(int t){
        Debug.Log("Block Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }

    #endregion

}