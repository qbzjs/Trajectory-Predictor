using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Filters;
using Unity.Labs.SuperScience;
using Unity.Labs.SuperScience.Example;

public class TrajectoryTracker : MonoBehaviour
{
    private KalmanFilter m_Filter;
    private MotionMath motion;
    private PhysicsTracker motionData;
    private DataWriter dataWriter;
    
    public bool recordTrajectory = false;
    
    public bool recordDepreciated = false;
    
    public Transform joint;
    
    public string testID;
    public string jointTag = "Joint Name Here";
    public string targetTag = "Target Tag Here";

    public Vector3 jointPosition;
    public Vector3 jointRotation;
    public float elapsedTime;
    public string timeStamp;

    [Header("- PHYSICS TRACKING - Speed/Acceleration")] 
    [Space(10)]
    public float p_speed;
    public Vector3 p_velocity;
    public Vector3 p_acceleration;
    public float p_accelerationStrength;
    public Vector3 p_direction;
    
    [Header("- PHYSICS TRACKING - Angular")] 
    [Space(10)]
    public float p_angularSpeed;
    public Vector3 p_angularVelocity;
    public Vector3 p_angularAcceleration;
    public float p_angularAccelerationStrength;
    public Vector3 p_angularAxis;
    
    [Header("- TRACKING - depreciated")] 
    [Space(10)]
    public float speed;
    public float speedSmooth;
    public Vector3 acceleration;
    public Vector3 accelerationSmooth;
    public float averageAcceleration;
    public float averageAccelerationSmooth;

    private Vector3 angularAcceleration;
    private Vector3 angularAccelerationSmooth;
    public float averageAngularAcceleration;
    public float averageAngularAccelerationSmooth;
    
    void Awake(){
        if(joint==null){
    		joint = this.transform;
    	}
        
        if (jointTag == null)
        {
            jointTag = transform.name;
        }
        
        dataWriter = new DataWriter();

        motion = gameObject.AddComponent<MotionMath>();

        targetTag = "";
    }

    void Start(){
        m_Filter = new KalmanFilter();
        m_Filter.State = 0; //Setting first (non filtered) value to 0 for example;

        motionData = gameObject.GetComponent<PhysicsData>().m_MotionData;
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

    private void Update(){


        //Debug.Log("Acc : " + motionData.Acceleration + " Speed : " + motionData.Speed + " Dir : " + motionData.Direction.ToString() + " Vel : " + motionData.Velocity);

    }

    void FixedUpdate()
    {
        //POSITION / ROTATION
        GetPositionRotation();
        
        //PHYSICS MOTION
        GetPhysicsMotion();
        
        //depreciated
        if (recordDepreciated){
            GetMotion_Depreciated();
        }


        if(recordTrajectory){
            
            elapsedTime += Time.deltaTime;
            TimeSpan t = TimeSpan.FromSeconds( elapsedTime );

            timeStamp = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            
            dataWriter.WriteTrajectoryData(timeStamp, elapsedTime.ToString("f2"), jointTag, targetTag,jointPosition, jointRotation,
                p_speed,p_velocity,p_acceleration,p_accelerationStrength,p_direction,
                p_angularSpeed,p_angularVelocity,p_angularAcceleration,p_angularAccelerationStrength,p_angularAxis);

            if (recordDepreciated){
                dataWriter.WriteTrajectoryData(jointPosition, jointRotation, acceleration, accelerationSmooth, averageAcceleration, averageAccelerationSmooth, speed, speedSmooth, timeStamp, elapsedTime.ToString("f2"), jointTag, targetTag);
            }
        }
    }

    public void GetPositionRotation(){
        jointPosition = joint.transform.position;
        jointRotation = joint.transform.eulerAngles;
        Vector3 rot = joint.transform.eulerAngles;;
        EulerLimitExtension eulerLimitExtension = new EulerLimitExtension();
        jointRotation = eulerLimitExtension.NormaliseAngle(rot);
    }

    public void GetPhysicsMotion(){
        p_speed = motionData.Speed;
        p_velocity = motionData.Velocity;
        p_acceleration = motionData.Acceleration;
        p_accelerationStrength = motionData.AccelerationStrength;
        p_direction = motionData.Direction;

        p_angularSpeed = motionData.AngularSpeed;
        p_angularVelocity = motionData.AngularVelocity;
        p_angularAcceleration = motionData.AngularAcceleration;
        p_angularAccelerationStrength = motionData.AngularAccelerationStrength;
        p_angularAxis = motionData.AngularAxis;
    }
    
    private void GetMotion_Depreciated(){
        speed = motion.GetVelocity();
        speedSmooth = KalmanFilter(speed);
        
        acceleration = motion.GetAcceleration();
        accelerationSmooth = new Vector3(KalmanFilter(acceleration.x), KalmanFilter(acceleration.y), KalmanFilter(acceleration.z));
        averageAcceleration = motion.GetAccelerationAverage();
        averageAccelerationSmooth = KalmanFilter(averageAcceleration);

        //not saved in data writer
        angularAcceleration = motion.GetAngularAcceleration();
        angularAccelerationSmooth = new Vector3(KalmanFilter(angularAcceleration.x), KalmanFilter(angularAcceleration.y), KalmanFilter(angularAcceleration.z));
        averageAngularAcceleration = motion.GetAngularAccelerationAverage();
        averageAngularAccelerationSmooth = KalmanFilter(averageAngularAcceleration);
    }
    
//-------------------------------FILTER------------    
    //Filter Variables
    // [Space(20)]
    // [SerializeField] [Range(0, 4)] public float Q_measurementNoise = 0.07f;// measurement noise
    // [SerializeField] [Range(0, 100)] public float R_EnvironmentNoize = 4.8f; // environment noise
    // [SerializeField] [Range(0, 100)] public float F_facorOfRealValueToPrevious = 1f; // factor of real value to previous real value
    // [SerializeField] [Range(0.1f, 100)] public float H_factorOfMeasuredValue = 1f; // factor of measured value to real value
    // [SerializeField] public float m_StartState;
    // [SerializeField] public float m_Covariance = 0.2f;
    
    
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
    
}
