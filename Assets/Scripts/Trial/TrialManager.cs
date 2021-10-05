using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class TrialManager : MonoBehaviour
{
    public static TrialManager instance;

    private TrialControls trialControls;

    public int runTotal = 4;
    public int blockTotal = 8;
    public int startingBlock = 101;

    public int initialWaitPeriod;
    public int interBlockRestPeriod = 15;
    public int countdown = 10;

    public int blockIndex;
    public int runIndex = 1;

    [HideInInspector]
    public BlockSequenceGenerator blockSequence;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitialiseTrial();
    }

    public void InitialiseTrial()
    {
        runIndex = 1;
        runTotal = Settings.instance.runTotal;
        blockTotal = Settings.instance.trialBlocks;
        SequenceGenerator();
        
        trialControls = TrialControls.instance;
        initialWaitPeriod = Settings.instance.startDelay;
    }
    public void InitialiseTrial(int blocks)
    {
        blockTotal = blocks;
        SequenceGenerator();
        
        trialControls = TrialControls.instance;
        initialWaitPeriod = Settings.instance.startDelay;
    }
    public void SequenceGenerator()
    {
        blockSequence = new BlockSequenceGenerator();
        blockSequence.GenerateSequence(blockTotal,startingBlock);

        // for (int i = 0; i < blockSequence.sequenceStartTrigger.Length; i++){
        //     Debug.Log(blockSequence.sequenceStartTrigger[i]);
        //     Debug.Log(blockSequence.sequenceEndTrigger[i]);
        // }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RunTrial();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelInvoke();
        }


    }

    private void RunTrial()
    {
          
        InitialWaitPeriod();
    }
    private void InitialWaitPeriod()
    {
        if (blockIndex < 1)
        {
            TrialSequence.instance.Initialise();
            blockIndex++;
            Invoke("Countdown", (initialWaitPeriod - 1) - countdown);
            Settings.instance.Status = GameStatus.Preparation;
            UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Preparation");
        }
        else
        {
            //blockIndex++; //changed in complete function
            Invoke("Countdown", (interBlockRestPeriod - 1) - countdown);
        }

    }
    private void Countdown()
    {
        SendUDP_byte(blockSequence.sequenceStartTrigger[blockIndex-1]);
        SendUDP_byte(0);

        CountdownTimer.instance.SetCountdown(countdown+1);
        UI_DisplayText.instance.SetBlockProgress(blockIndex, blockTotal);
        
        Settings.instance.Status = GameStatus.Countdown;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Countdown");
    }
    public void StartBlock()
    {
        Debug.Log("Start Block");
        TrialControls.instance.SetPlay();
    }

    public void BlockComplete()
    {
        SendUDP_byte(blockSequence.sequenceEndTrigger[blockIndex-1]);
        SendUDP_byte(0);

        blockIndex++;
        if(blockIndex > blockTotal)
        {
            Debug.Log("END RUN!!!");
            runIndex++;  
            TrialSequence.instance.Reset();
            blockIndex = 0;
            UI_DisplayText.instance.SetBlockProgress(blockIndex, blockTotal);
            


            if (runIndex <= runTotal)
            {
                Settings.instance.Status = GameStatus.Preparation;
                UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Preparation");
                Invoke("RunTrial",10f); //todo update invoke time to settings (interrunrestperiod)
                //TODO Display text for participant 
            }
            else
            {
                Settings.instance.Status = GameStatus.Ready;
                UI_DisplayText.instance.SetStatus(Settings.instance.Status, "System Ready - Session Complete");
                //TODO Display text for participant 
            }
        }
        else
        {
            RunTrial();
        }
        //rest period
    }
    
    
    
    private void SendUDP_byte(int t)
    {
        Debug.Log("Block Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }
}
