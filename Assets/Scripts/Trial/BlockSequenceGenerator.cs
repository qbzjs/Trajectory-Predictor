using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSequenceGenerator : MonoBehaviour
{
    public int blockTotal = 8;
    public int startingBlock = 101;
    public int[] sequenceStartTrigger = new int[0];
    public int[] sequenceEndTrigger = new int[0];

    void Awake()
    {
        GenerateSequence();
    }

    
    void GenerateSequence()
    {
        sequenceStartTrigger = new int[blockTotal];
        sequenceEndTrigger = new int[blockTotal];

        for (int i=0; i< sequenceStartTrigger.Length; i++)
        {
            sequenceStartTrigger[i] = startingBlock + (1*i);
            sequenceEndTrigger[i] = sequenceStartTrigger[i] + 1;

        }
    }
}
