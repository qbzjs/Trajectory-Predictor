using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAO : MonoBehaviour
{
    
    public static DAO instance;

    public MotionDataFormat motionData_LeftHand;
    public MotionDataFormat motionData_RightHand;
    public MotionDataFormat motionData_Head;

    public int reachTarget;

    void Awake(){
        instance = this;

    }

    public MotionDataFormat MotionData_LeftHand
    {
        get { return motionData_LeftHand; }
        set { motionData_LeftHand = value; }
    }
    public MotionDataFormat MotionData_RightHand
    {
        get { return motionData_RightHand; }
        set { motionData_RightHand = value; }
    }
    public MotionDataFormat MotionData_Head
    {
        get { return motionData_Head; }
        set { motionData_Head = value; }
    }

    public int ReachTarget
    {
        get { return reachTarget; }
        set { reachTarget = value; }

    }
}
