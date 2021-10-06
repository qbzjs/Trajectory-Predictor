using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAO : MonoBehaviour
{
    
    public static DAO instance;

    public BlockSequenceGenerator blockSequence;
    
    private int startingBlock = 101;
    
    public MotionDataFormat motionDataLeftWrist;
    public MotionDataFormat motionDataRightWrist;
    public MotionDataFormat motionData_Head;
    public EyeDataFormat eyeData;

    //public int blockSequence
    public int reachTarget;

    void Awake(){
        instance = this;

    }

    public int StartingBlock
    {
        get { return startingBlock; }
        set { startingBlock = value; }
    }
    public BlockSequenceGenerator BlockSequence 
    {
        get { return blockSequence; }
        set { blockSequence = value; }
    }
    
    
    
    //***** MOTION DATAS *************************************
    public MotionDataFormat MotionData_LeftWrist
    {
        get { return motionDataLeftWrist; }
        set { motionDataLeftWrist = value; }
    }
    public MotionDataFormat MotionData_RightWrist
    {
        get { return motionDataRightWrist; }
        set { motionDataRightWrist = value; }
    }
    public MotionDataFormat MotionData_Head
    {
        get { return motionData_Head; }
        set { motionData_Head = value; }
    }
    public EyeDataFormat EyeData
    {
        get { return eyeData; }
        set { eyeData = value; }
    }
    
    //TODO Make a hand data getter for selected hand from settings
    // public MotionDataFormat MotionData_RightWrist
    // {
    //     get { return motionDataRightWrist; }
    //     set { motionDataRightWrist = value; }
    // }
    
    public int ReachTarget
    {
        get { return reachTarget; }
        set { reachTarget = value; }

    }
}
