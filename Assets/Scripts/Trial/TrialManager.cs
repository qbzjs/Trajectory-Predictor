using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class TrialManager : MonoBehaviour
{
    public static TrialManager instance;

    private TrialControls trialControls;

    public int blockTotal = 8;

    public int initialWaitPeriod;
    public int interBlockRestPeriod = 15;
    public int countdown = 10;

    public int blockIndex;

    public BlockSequenceGenerator blockSequence;

    private void Awake()
    {
        instance = this;

        blockSequence = new BlockSequenceGenerator();
        blockSequence.GenerateSequence(blockTotal,101);

        for (int i = 0; i < blockSequence.sequenceStartTrigger.Length; i++){
            Debug.Log(blockSequence.sequenceStartTrigger[i]);
            Debug.Log(blockSequence.sequenceEndTrigger[i]);
        }
        
    }

    void Start()
    {
        trialControls = TrialControls.instance;
        initialWaitPeriod = Settings.instance.startDelay;
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
            Debug.Log("END SESSION!!!");
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
