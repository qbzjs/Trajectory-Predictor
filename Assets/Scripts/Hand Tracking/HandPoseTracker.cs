using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseTracker : MonoBehaviour
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

        dataWriter = gameObject.GetComponent<DataWriter>();
    }
    
    private void OnEnable()
    {
        TrackingControl.OnSpaceBar += TrackingControl_OnSpaceBar;
    }

    private void OnDisable()
    {
        TrackingControl.OnSpaceBar -= TrackingControl_OnSpaceBar;
    }
    private void TrackingControl_OnSpaceBar(string id)
    {
        if (!trackTrajectory)
        {
            Debug.Log("---Start Tracking---");
            //testID = jointTag + "_" + System.Guid.NewGuid().ToString();
            testID = jointTag + "_" + id;
            elapsedTime = 0;
            trackTrajectory = true;
        }
        else
        {
            Debug.Log("---Stop Tracking---");
            trackTrajectory = false;
            dataWriter.WriteData(testID);
        }
    }



    void FixedUpdate()
    {
    	jointPosition = joint.transform.position;
        jointRotation = joint.transform.eulerAngles;



        if(trackTrajectory)
        {
        	// Debug.Log("POSITION : " + jointPosition + " || " + "ROTATION : " + jointRotation);
        	
            elapsedTime += Time.deltaTime;
            
            TimeSpan t = TimeSpan.FromSeconds( elapsedTime );

            string timeStamp = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            
            dataWriter.WriteTrajectoryData(jointPosition,jointRotation,timeStamp, elapsedTime.ToString("f2"), jointTag);
        }
    }
}
