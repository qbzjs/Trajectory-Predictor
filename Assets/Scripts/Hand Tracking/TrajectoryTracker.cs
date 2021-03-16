﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracker : MonoBehaviour
{
    // public static HandPoseTracker instance;
    
    public bool trackTrajectory = false;
    
    public Transform joint;
    
    public string testID;
    public string jointTag = "Joint Name Here";

    public Vector3 jointPosition;
    public Vector3 jointRotation;
    public float elapsedTime;
    public string timeStamp;
    
    public float velocity;
    public Vector3 acceleration;
    public float averageAcceleration;

    public string targetTag;
    
    private VelocityAcceleration va;
        
    private DataWriter dataWriter;

    void Awake()
    {
    	if(joint==null){
    		joint = this.transform;
    	}
        
        if (jointTag == null)
        {
            jointTag = transform.name;
        }

        // dataWriter = gameObject.GetComponent<DataWriter>();
        dataWriter = new DataWriter();

        // va = new VelocityAcceleration();
        va = gameObject.AddComponent<VelocityAcceleration>();

        targetTag = "";
    }
    
    private void OnEnable(){
        InputManager.OnRecordAction += ToggleTrackingRecord;
        TrialSequence.OnTargetAction += TrialSequenceOnTargetAction;
        TrialSequence.OnTargetRestAction += TrialSequenceOnTargetRestAction;
    }
    private void OnDisable()
    {
        InputManager.OnRecordAction -= ToggleTrackingRecord;
        TrialSequence.OnTargetAction -= TrialSequenceOnTargetAction;
        TrialSequence.OnTargetRestAction -= TrialSequenceOnTargetRestAction;
    }
    
    private void TrialSequenceOnTargetRestAction(int targetnumber){
        targetTag = "rest";
    }

    private void TrialSequenceOnTargetAction(int targetnumber){
        
        targetTag = targetnumber.ToString();
    }

    private void ToggleTrackingRecord(bool t, string id)
    {
        //trackTrajectory = t;

        if (Settings.instance.recordTrajectory){
            if (!trackTrajectory)
            {
                Debug.Log("---- Start Trajectory Tracking : " + jointTag);
                //testID = jointTag + "_" + System.Guid.NewGuid().ToString();
                dataWriter = new DataWriter();
                testID = jointTag + "_" + id;
                elapsedTime = 0;
                trackTrajectory = true;
            }
            else
            {
                Debug.Log("---- Stop Trajectory Tracking : " + jointTag);
                trackTrajectory = false;
                dataWriter.WriteData(testID);
            }
        }

    }



    void FixedUpdate()
    {
    	jointPosition = joint.transform.position;
        jointRotation = joint.transform.eulerAngles;
        acceleration = va.GetAcceleration();
        averageAcceleration = va.GetAccelerationAverage();
        velocity = va.GetVelocity();

        if(trackTrajectory)
        {
        	// Debug.Log("POSITION : " + jointPosition + " || " + "ROTATION : " + jointRotation);
        	
            elapsedTime += Time.deltaTime;
            
            TimeSpan t = TimeSpan.FromSeconds( elapsedTime );

            timeStamp = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            
            dataWriter.WriteTrajectoryData(jointPosition,jointRotation, acceleration, averageAcceleration, velocity, timeStamp, elapsedTime.ToString("f2"), jointTag, targetTag);
        }
    }
}
