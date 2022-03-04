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
    
    const int startingRunTrigger = 201; // starts from 101 //TODO FIX + 1 TO BLOCK AND RUN
    
    public int runTotal = 2;
    public int runIndex = 0;
    
    public float postRunWaitPeriod = 1f;
    
    [HideInInspector] 
    public BlockSequenceGenerator runSequence;
    
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
        runSequence = gameManager.TriggerSequenceGenerator(runTotal, startingRunTrigger);//generate run trigger sequences
        UpdateRunStatus(runStatus,runTotal,runIndex);
    }
    #endregion
    
    #region User Input
    
    public void StartTrial(){
        if (runIndex <= runTotal){
            runsComplete = false;
            InitialiseRun();
            StartCoroutine(RunBlock());
        }
    }
    
    #endregion
    
    #region RunLogic

    private IEnumerator RunBlock(){

        if (runIndex == 0 || blockManager.blocksComplete){
            runIndex++;
            
            //UDP ON
            gameManager.SendUDP_byte(runSequence.sequenceStartTrigger[runIndex-1],0,GameStatus.RunStarted); //no modifier
            runStatus = GameStatus.RunStarted;
            UpdateRunStatus(runStatus,runTotal,runIndex);
            //event
            gameManager.RunEvent(GameStatus.RunStarted,0);
        }

        Debug.Log("----------RunManager - Started Block------------------");

        runStatus = GameStatus.Preparation;
        UpdateRunStatus(runStatus,runTotal,runIndex);

        blockManager.StartTrial();
        
        yield return new WaitUntil(() => CountdownTimer.instance.timeUp == true);
        
        runStatus = GameStatus.RunningTrials;
        UpdateRunStatus(runStatus,runTotal,runIndex);

        // if (blockManager.blockIndex == blockManager.blockTotal-1) //only progress run on last block
        if (blockManager.blockIndex == blockManager.blockTotal) //only progress this logic on last block
        {
            yield return new WaitUntil(() => blockManager.blocksComplete);
        
            yield return new WaitForSeconds(gameManager.SpeedCheck(postRunWaitPeriod));
            
            runStatus = GameStatus.RunComplete;
            UpdateRunStatus(runStatus,runTotal,runIndex);
            //event
            gameManager.RunEvent(GameStatus.RunComplete,0);

            //UDP OFF
            gameManager.SendUDP_byte(runSequence.sequenceEndTrigger[runIndex-1],0,GameStatus.AllRunsComplete); //no modifier
            
            Debug.Log("RUN END ________________________________________________");
            UpdateRunStatus(GameStatus.Debug,runTotal,runIndex);
            
            if (runIndex >= runTotal){
                //UpdateRunStatus(GameStatus.Debug,runTotal,runIndex);//debug
                runsComplete = true;
                runStatus = GameStatus.AllRunsComplete;
                runIndex = 0;
                UpdateRunStatus(runStatus,runTotal,runIndex);
                //event
                gameManager.RunEvent(GameStatus.AllRunsComplete,0);
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

    #endregion
    
}