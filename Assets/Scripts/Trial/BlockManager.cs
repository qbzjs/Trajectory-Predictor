using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;

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

    private void Awake(){
        instance = this;
    }

    void Start()
    {
        InitialiseTrial();
    }

    public void InitialiseTrial()
    {

    }
    public void InitialiseTrial(int blocks)
    {

    }
    public void SequenceGenerator()
    {
        blockSequence = new BlockSequenceGenerator();
        blockSequence.GenerateSequence(blockTotal,startingBlock);
    }

    void Update()
    {

    }

    public void StartBlock(){
        
    }

    
    
    private void SendUDP_byte(int t)
    {
        Debug.Log("Block Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }
}
