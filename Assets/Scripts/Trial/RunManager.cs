using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class RunManager : MonoBehaviour
{
    public static RunManager instance;

    private GameManager gameManager;
    private BlockManager blockManager;
    
    public GameStatus runStatus;

    public int runTotal = 2;
    public int runIndex = 0;
    
    public float postRunWaitDuration = 1f;

    public bool runsComplete;
    
    public delegate void RunAction(GameStatus status, int runTotal,int runIndex);
    public static event RunAction OnRunAction;
    

    #region Initialise
    private void Awake(){
        instance = this;
        gameManager = GetComponent<GameManager>();
        blockManager = GetComponent<BlockManager>();
    }

    private void Start(){
        runIndex = 0;
        InitialiseBlock();
    }

    //call from settings as well so updates every change
    public void InitialiseBlock(){
        runStatus = GameStatus.Ready;
        runsComplete = false;
        UpdateRunStatus(runStatus,runTotal,runIndex);
    }
    #endregion
    
    #region User Input
    
    public void StartTrial(){
        if (runIndex < runTotal){
            StartCoroutine(RunBlock());
        }
        else{
            Debug.Log("RUNS FINISHED");
        }
    }
    
    #endregion
    
    #region RunLogic

    private IEnumerator RunBlock(){
        runStatus = GameStatus.Preparation;
        UpdateRunStatus(runStatus,runTotal,runIndex);
        
        blockManager.StartTrial();
        
        yield return new WaitUntil(() => CountdownTimer.instance.timeUp == true);
        
        runStatus = GameStatus.RunningTrials;
        UpdateRunStatus(runStatus,runTotal,runIndex);

        if (blockManager.blockIndex == blockManager.blockTotal-1) //only progress run on last block
        {
            yield return new WaitUntil(() => blockManager.blocksComplete);
        
            yield return new WaitForSeconds(postRunWaitDuration);
            
            runStatus = GameStatus.RunComplete;
            UpdateRunStatus(runStatus,runTotal,runIndex);

            Debug.Log("RUN END ________________________________________________");
        
            if (blockManager.blocksComplete){
                runIndex++;
            }
        
            if (runIndex >= runTotal){
                runsComplete = true;
                runStatus = GameStatus.AllRunsComplete;
                UpdateRunStatus(runStatus,runTotal,runIndex);
                //runIndex = 0;
            }
        }

    }
    #endregion
    
    #region Status & Event Updates

    private void UpdateRunStatus(GameStatus status, int total, int index){
        if (OnRunAction != null){
            OnRunAction(status, total, index);
        }
        gameManager.RunTracker(status,total,index);
        if (gameManager.debug){
            Debug.Log("------ Run Status: " + status + "Run Progress: " + index+"/"+total);
        }
    }
    
    // private void SendUDP_byte(int t){
    //     Debug.Log("Run Trigger Sent: " + t);
    //     UDPClient.instance.SendData((byte)t);
    // }

    #endregion
    
}