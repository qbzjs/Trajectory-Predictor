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

    public TrialSequencer trialSequencer;
    
    const int startingBlock = 101;
    
    public int blockTotal = 4;
    public int blockIndex = 0;
    public int countdown = 10;
    public int maxCountdownDisplay;
    
    [Header("---------")]
    public bool blocksComplete = false;
    
    [HideInInspector]
    public BlockSequenceGenerator blockSequence;

    private void OnEnable(){
        InputManager.OnUserInputAction+= InputManagerOnOnUserInputAction;
    }

    private void InputManagerOnOnUserInputAction(UserInputType inputType){
        throw new NotImplementedException();
    }

    private void Awake(){
        instance = this;
    }

    void Start(){
        InitialiseBlock();
    }
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBlock();
        }
    }

    public void InitialiseBlock(){
        BlockSequenceGenerator();
    }

    public void StartBlock(){
        InitialiseBlock();
        StartCoroutine(RunBlock());
    }
    
    private IEnumerator RunBlock(){
        //TODO - add if for no countdown
        Debug.Log("--------- Countdown Start------------");
        CountdownTimer.instance.timeUp = false;
        CountdownTimer.instance.SetCountdown(countdown+1, maxCountdownDisplay +1);
        yield return new WaitUntil(() => CountdownTimer.instance.timeUp==true);
        Debug.Log("--------Countdown Finished - start block----------- ");
        trialSequencer.StartTrialSequence();
        yield return new WaitUntil(() => trialSequencer.sequenceComplete);
        Debug.Log("--------Sequence Finished ---------- ");
        
        //send block event - user inputs when to start next block
        
        blockIndex++;
        
        if (blockIndex >= blockTotal){
            blocksComplete = true;
            //all blocks finished in run - tell run manager
            blockIndex = 0;
        }
    }
    
    
    public void BlockSequenceGenerator()
    {
        blockSequence = new BlockSequenceGenerator();
        blockSequence.GenerateSequence(blockTotal,startingBlock);
        // for (int i = 0; i < blockSequence.sequenceStartTrigger.Length; i++){
        //     Debug.Log(blockSequence.sequenceStartTrigger[i]);
        //     Debug.Log(blockSequence.sequenceEndTrigger[i]);
        // }
        
    }
    private void SendUDP_byte(int t)
    {
        Debug.Log("Block Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }
}
