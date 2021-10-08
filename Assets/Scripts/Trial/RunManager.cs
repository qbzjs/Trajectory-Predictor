using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager instance;

    private GameManager gameManager;
    private BlockManager blockManager;

    public int runTotal = 2;
    public int runIndex = 0;

    public bool runsComplete;
    
    private void Awake(){
        instance = this;
        gameManager = GetComponent<GameManager>();
        blockManager = GetComponent<BlockManager>();
    }

    #region User Input
    
    public void StartTrial(){
        StartCoroutine(RunBlock());
    }
    
    #endregion

    #region RunLogic

    private IEnumerator RunBlock(){
        Debug.Log("--------- Run Start------------");
        blockManager.StartTrial();
        yield return new WaitUntil(() => blockManager.blocksComplete);
        Debug.Log("--------Blocks Finished - Ready next run ---------- ");

        //send run event - user inputs when to start next run

        runIndex++;

        if (runIndex >= runTotal){
            runsComplete = true;
            //all runs finished in run - tell run manager
            runIndex = 0;
        }
    }
    #endregion
}