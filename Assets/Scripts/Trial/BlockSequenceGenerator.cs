using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSequenceGenerator : MonoBehaviour
{
    public int blockTotal = 8;
    public int startingBlock = 101;
    public int[] sequenceStartTrigger = new int[0];
    public int[] sequenceEndTrigger = new int[0];

    void OnEnable()
    {
        GenerateSequence();
    }

    
    public void GenerateSequence()
    {
        sequenceStartTrigger = new int[blockTotal];
        sequenceEndTrigger = new int[blockTotal];

        for (int i=0; i< sequenceStartTrigger.Length; i++)
        {
            sequenceStartTrigger[i] = startingBlock + (1*i);
            sequenceEndTrigger[i] = sequenceStartTrigger[i] + 10;
        }
    }
    
    public void GenerateSequence(int total, int startBlock)
    {
        blockTotal = total;
        startingBlock = startBlock;

        sequenceStartTrigger = new int[total];
        sequenceEndTrigger = new int[total];

        for (int i=0; i< sequenceStartTrigger.Length; i++)
        {
            sequenceStartTrigger[i] = startingBlock + (1*i);
            sequenceEndTrigger[i] = sequenceStartTrigger[i] + 10;
            Debug.Log(sequenceStartTrigger[i]);
            Debug.Log(sequenceEndTrigger[i]);
        }
    }
}
