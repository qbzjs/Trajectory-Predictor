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
    
    const int startingBlock = 1001;

    public int runTotal = 2;
    public int runIndex = 0;
    
    public float postRunWaitPeriod = 1f;
    
    public int interRunRestPeriod;
    
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
        runStatus = GameStatus.Ready;
        runsComplete = false;
        InitialiseRun();
    }

    //call from settings as well so updates every change
    public void InitialiseRun(){
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
        
            yield return new WaitForSeconds(postRunWaitPeriod);
            
            runStatus = GameStatus.RunComplete;
            UpdateRunStatus(runStatus,runTotal,runIndex);

            //Debug.Log("RUN END ________________________________________________");
            UpdateRunStatus(GameStatus.Debug,runTotal,runIndex);
        
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
        if (OnRunAction != null && status!=GameStatus.Debug){
            OnRunAction(status, total, index);
        }

        if (status != GameStatus.Debug){
            gameManager.RunTracker(status,total,index);
        }
        
        if (gameManager.debugTimingDetailed){
            Debug.Log("------ Run Status: " + status + "Run Progress: " + index+"/"+total);
        }

        if (gameManager.debugTimingSimple && status == GameStatus.Debug){
            Debug.Log("RUN END ________________________________________________");
        }
    }
    
    // private void SendUDP_byte(int t){
    //     Debug.Log("Run Trigger Sent: " + t);
    //     UDPClient.instance.SendData((byte)t);
    // }

    #endregion
    
}