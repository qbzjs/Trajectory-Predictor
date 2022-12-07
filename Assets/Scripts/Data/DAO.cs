using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DAO : MonoBehaviour
{
    
    public static DAO instance;
    private Settings settings;

    public BlockSequenceGenerator blockSequence;
    
    private int startingBlock = 101;

    //[HideInInspector]
    private MotionDataFormat motionDataActiveWrist;
    public MotionDataFormat motionDataLeftWrist;
    public MotionDataFormat motionDataRightWrist;
    public MotionDataFormat motionData_Head;
    public EyeDataFormat eyeData;

    //public int blockSequence
    public int currentReachTarget;
    public RunType currentRunType;

    public int num;

    void Awake(){
        instance = this;
    }

    private void Start(){
        settings = Settings.instance;
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
    //active wrist - read only
    public MotionDataFormat MotionData_ActiveWrist
    {
        get{
            if (settings.handedness == Handedness.Left){
                return motionDataLeftWrist;
            }
            else{
                return motionDataRightWrist;
            }
        }
        // set{
        //     motionDataLeftWrist = value;
        //     if (settings.handedness == Handedness.Left){
        //         motionDataActiveWrist = value;
        //     }
        //     else{
        //         motionDataActiveWrist = value;
        //     }
        // }
    }
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
    
    public int CurrentReachTarget
    {
        get { return currentReachTarget; }
        set { currentReachTarget = value; }

    }
    public RunType GetRunType(){
        return currentRunType;
    }
}
