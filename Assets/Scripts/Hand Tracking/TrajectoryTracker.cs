﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Filters;


public class TrajectoryTracker : MonoBehaviour
{
    // public static HandPoseTracker instance;
    
    public bool recordTrajectory = false;
    
    public Transform joint;
    
    public string testID;
    public string jointTag = "Joint Name Here";

    public Vector3 jointPosition;
    public Vector3 jointRotation;
    public float elapsedTime;
    public string timeStamp;
    
    public float velocity;
    public float velocitySmooth;
    public Vector3 acceleration;
    public Vector3 accelerationSmooth;
    public float averageAcceleration;
    public float averageAccelerationSmooth;

    private Vector3 angularAcceleration;
    private Vector3 angularAccelerationSmooth;
    public float averageAngularAcceleration;
    public float averageAngularAccelerationSmooth;

    public string targetTag;
    
    private KalmanFilter m_Filter;
    
    private MotionMath motion;
        
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
        motion = gameObject.AddComponent<MotionMath>();

        targetTag = "";
    }

    void Start(){
        m_Filter = new KalmanFilter();
        m_Filter.State = 0; //Setting first (non filtered) value to 0 for example;
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
        //recordTrajectory = t;

        if (Settings.instance.recordTrajectory){
            if (!recordTrajectory)
            {
                Debug.Log("---- Start Trajectory Tracking : " + jointTag);
                //testID = jointTag + "_" + System.Guid.NewGuid().ToString();
                dataWriter = new DataWriter();
                testID = jointTag + "_" + id;
                elapsedTime = 0;
                recordTrajectory = true;
            }
            else
            {
                Debug.Log("---- Stop Trajectory Tracking : " + jointTag);
                recordTrajectory = false;
                dataWriter.WriteData(testID);
            }
        }

    }
    

    void FixedUpdate()
    {
    	jointPosition = joint.transform.position;
        jointRotation = joint.transform.eulerAngles;
        Vector3 rot = joint.transform.eulerAngles;;
        EulerLimitExtension eulerLimitExtension = new EulerLimitExtension();
        jointRotation = eulerLimitExtension.NormaliseAngle(rot);

        acceleration = motion.GetAcceleration();
        accelerationSmooth = new Vector3(KalmanFilter(acceleration.x), KalmanFilter(acceleration.y), KalmanFilter(acceleration.z));
        averageAcceleration = motion.GetAccelerationAverage();
        averageAccelerationSmooth = KalmanFilter(averageAcceleration);

        //not saved in data writer
        angularAcceleration = motion.GetAngularAcceleration();
        angularAccelerationSmooth = new Vector3(KalmanFilter(angularAcceleration.x), KalmanFilter(angularAcceleration.y), KalmanFilter(angularAcceleration.z));
        averageAngularAcceleration = motion.GetAngularAccelerationAverage();
        averageAngularAccelerationSmooth = KalmanFilter(averageAngularAcceleration);

        // velocity = motion.GetVelocity();
        // velocitySmooth = KalmanFilter(velocity);

        velocity = motion.GetVelocity();
        velocitySmooth = Speed;
        

        if(recordTrajectory)
        {
        	// Debug.Log("POSITION : " + jointPosition + " || " + "ROTATION : " + jointRotation);
        	
            elapsedTime += Time.deltaTime;
            
            TimeSpan t = TimeSpan.FromSeconds( elapsedTime );

            timeStamp = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            
            dataWriter.WriteTrajectoryData(jointPosition,jointRotation, acceleration, accelerationSmooth, averageAcceleration, averageAccelerationSmooth, velocity, velocitySmooth, timeStamp, elapsedTime.ToString("f2"), jointTag, targetTag);
        }
    }

    [Space(20)]
    [SerializeField] [Range(0, 4)] public float Q_measurementNoise = 0.07f;// measurement noise
    [SerializeField] [Range(0, 100)] public float R_EnvironmentNoize = 4.8f; // environment noise
    [SerializeField] [Range(0, 100)] public float F_facorOfRealValueToPrevious = 1f; // factor of real value to previous real value
    [SerializeField] [Range(0.1f, 100)] public float H_factorOfMeasuredValue = 1f; // factor of measured value to real value
    [SerializeField] public float m_StartState;
    [SerializeField] public float m_Covariance = 0.2f;
    
    
    private float KalmanFilter(float value) {
        //var filter = new Filters.KalmanFilter(RealValueToPreviousRealValue: F_facorOfRealValueToPrevious, MeasuredToRealValue: H_factorOfMeasuredValue, mesurementNoize: Q_measurementNoise, environmentNoize: R_EnvironmentNoize);

        float FilteredValue = m_Filter.FilterValue(value); //applying filter
        
        //float FilteredValue = filter.FilterValue(value);

        //KalmanFilterFloat kf = new KalmanFilterFloat();
        //FilteredValue = kf.Filter(value,Q_measurementNoise,R_EnvironmentNoize);
        

        //float v = m_Filter.FilterValue(value);
//        Debug.Log("TestingFilter: Dirty value = " + value + " Filtered value = " + FilteredValue); //printing output

        return FilteredValue;
    }

    private void Update(){
        StartCoroutine(SpeedReckoner());
    }

    void OnEnabled() {
        
    }

    public float Speed;
    public float UpdateDelay;

    private IEnumerator SpeedReckoner() {

        YieldInstruction timedWait = new WaitForSeconds(UpdateDelay);
        Vector3 lastPosition = transform.position;
        float lastTimestamp = Time.time;

        while (enabled) {
            yield return timedWait;

            var deltaPosition = (transform.position - lastPosition).magnitude;
            var deltaTime = Time.time - lastTimestamp;

            if (Mathf.Approximately(deltaPosition, 0f)) // Clean up "near-zero" displacement
                deltaPosition = 0f;

            Speed = deltaPosition / deltaTime;
            // Speed = KalmanFilter(Speed);

            lastPosition = transform.position;
            lastTimestamp = Time.time;
        }
    }
}
