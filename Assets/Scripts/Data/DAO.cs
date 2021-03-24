using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAO : MonoBehaviour
{
    
    public static DAO instance;

    public MotionDataFormat motionData;
    void Awake(){
        instance = this;

    }

    public MotionDataFormat IMUData
    {
        get { return motionData; }
        set { motionData = value; }
    }
}
