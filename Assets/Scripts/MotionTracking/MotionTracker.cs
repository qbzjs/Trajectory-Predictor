using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Filters;
using Unity.Labs.SuperScience;
//using Unity.Labs.SuperScience.Example;
using Enums;
using TMPro;
using ViveSR.anipal.Eye;

public class MotionTracker : MonoBehaviour
{
    private KalmanFilter m_Filter;
    private MotionMath motion_Depc;
    private PhysicsTracker motion;
    private MotionDataStreaming motionDataStreaming;
    private DataWriter dataWriter;

    public bool recordEnabled; //set in inspector 
    [Space(10)]
    public Transform motionObject;

    public MotionTag motionTag;
    public string sessionTag = "Session Name Here";
    public string fileName = "Name Here";
    private string testID;
    public int targetNumber;
    private string targetTag = "Target Tag Here";
    public string id;

    private bool recordTrajectory = false;

    public bool recordDepreciatedMotion = false;

    [Header("PHYSICS MOTION TRACKING")]
    public MotionDataFormat motionData;

    
    private Vector3 position;
    private Vector3 rotation;
    private float elapsedTime;
    private string timeStamp;

    private float p_speed;
    private Vector3 p_velocity;
    private Vector3 p_acceleration;
    private float p_accelerationStrength;
    private Vector3 p_direction;

    private float p_angularSpeed;
    private Vector3 p_angularVelocity;
    private Vector3 p_angularAcceleration;
    private float p_angularAccelerationStrength;
    private Vector3 p_angularAxis;
    
    [Space(8)]
    [Range(0.01f, 5f)] 
    public float motionThreshold = 0.5f;
    public bool inMotion;

    public TextMeshProUGUI motionDebug;
    
    [Header("MOTION TRACKING ***depreciated")] 
    [Space(10)]
    public float speed;
    public float speedSmooth;
    public Vector3 velocity;
    public Vector3 previousPos;
    public Vector3 acceleration;
    public Vector3 accelerationSmooth;
    public float averageAcceleration;
    public float averageAccelerationSmooth;

    private Vector3 angularAcceleration;
    private Vector3 angularAccelerationSmooth;
    public float averageAngularAcceleration;
    public float averageAngularAccelerationSmooth;


    
    void Awake(){
        if(motionObject==null){
    		motionObject = this.transform;
    	}

        dataWriter = new DataWriter();
        
        targetTag = "";
    }


    void Start(){
        
        m_Filter = new KalmanFilter();
        m_Filter.State = 0; //Setting first (non filtered) value to 0 for example;

        motion = gameObject.GetComponent<PhysicsData>().m_MotionData;
        motion_Depc = gameObject.AddComponent<MotionMath>();

        motionDataStreaming = new MotionDataStreaming();
        
        // id = System.Guid.NewGuid().ToString();
    }
    
    //TODO - fix file name generator from new 'BlockManager' 
    private string GenerateFileName(){
        BlockManager tm = BlockManager.instance;
        sessionTag = Settings.instance.GetSessionInfo();
        string n = "";
        if (motionTag == MotionTag.Null)
        {
            n = transform.name+"_" + sessionTag + "_"+"Block" + tm.blockSequence.sequenceStartTrigger[tm.blockIndex-1].ToString();
        }
        else{
            n = motionTag.ToString() + "_" + sessionTag + "_"+"Block" + tm.blockSequence.sequenceStartTrigger[tm.blockIndex-1].ToString();
        }
        return n;
    }

    private void OnEnable(){
        GameManager.OnBlockAction+=GameManagerOnBlockAction;
        GameManager.OnTrialAction+= GameManagerOnTrialAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction-=GameManagerOnBlockAction;
        GameManager.OnTrialAction-= GameManagerOnTrialAction;
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.BlockStarted){
            // id = System.Guid.NewGuid().ToString();
        }
        //start of trial in block
        if (eventType == GameStatus.CountdownComplete){
            ToggleTrackingRecord(true, id);
        }

        if (eventType == GameStatus.BlockComplete){
            ToggleTrackingRecord(false, id);
            Debug.Log("-------------------------------------------------");
        }
    }
    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifeTime, int index, int total){
        if (eventType == TrialEventType.TargetPresentation){
            targetNumber = targetNum+1;
        }
        if (eventType == TrialEventType.Rest){
            targetNumber = targetNumber+10;
        }
        if (eventType == TrialEventType.PostTrialPhase){
            targetNumber = 0;
        }
    }




    private void ToggleTrackingRecord(bool t, string id)
    {
        //recordTrajectory = t;

        if (Settings.instance.recordMotionData){
            if (!recordTrajectory && recordEnabled)
            {
                Debug.Log("---- Start Trajectory Motion Capture : " + fileName);
                //testID = jointTag + "_" + System.Guid.NewGuid().ToString();
                dataWriter = new DataWriter();
                fileName = GenerateFileName();
                testID = fileName + "_" + id;
                elapsedTime = 0;
                recordTrajectory = true;
            }
            else
            {
                Debug.Log("---- Stop Trajectory Motion Capture : " + fileName);
                recordTrajectory = false;
                dataWriter.WriteData(testID);
            }
        }

    }
    
    
    private void Update(){
        
        //POSITION / ROTATION
        GetPositionRotation();
        
        //PHYSICS MOTION
        GetPhysicsMotion();
        
        //depreciated
        if (recordDepreciatedMotion)
        {
            GetMotion_Depreciated();
        }

        FormatMotionData();

        inMotion = CheckMotionThreshold();

        if (motionDebug != null)
        {
            if (inMotion)
            {
//              Debug.Log(motionTag.ToString() + " : " + " M O V I N G !!!");
                motionDebug.text = motionTag.ToString() + " : " + " M O V I N G !";
                motionDebug.color = Color.green;
            }
            else
            {
                motionDebug.text = motionTag.ToString() + " : " + " S T A T I O N A R Y !";
                motionDebug.color = Color.grey;
            }
        }
        
        //depreciated velocity
        velocity = (motionObject.position - previousPos) / Time.deltaTime;
       
        //Debug.Log("Acc : " + motionData.Acceleration + " Speed : " + motionData.Speed + " Dir : " + motionData.Direction.ToString() + " Vel : " + motionData.Velocity);
    }

    void LateUpdate()
    {
        previousPos = motionObject.position;
    }

    void FixedUpdate()
    {
        if(recordTrajectory && recordEnabled){
            
            elapsedTime += Time.deltaTime;
            TimeSpan t = TimeSpan.FromSeconds( elapsedTime );

            timeStamp = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);

            // targetTag = DAO.instance.reachTarget.ToString(); //depreciated
            if (targetNumber != 0){
                targetTag = targetNumber.ToString();
            }
            else{
                targetTag = "0"; // NOTE: This is changed to zero so trigger tags are not NAN
            }
            

            dataWriter.WriteTrajectoryData(timeStamp, elapsedTime.ToString("f2"), motionTag.ToString(), targetTag,
                inMotion.ToString(),  motionThreshold,position, rotation,
                p_speed,p_velocity, velocity, p_acceleration,p_accelerationStrength,p_direction,
                p_angularSpeed,p_angularVelocity,p_angularAcceleration,p_angularAccelerationStrength,p_angularAxis);

            

            //needs seconds recorder
            if (recordDepreciatedMotion)
            {
                dataWriter.WriteTrajectoryData(position, rotation, acceleration, accelerationSmooth, averageAcceleration, averageAccelerationSmooth, speed, speedSmooth, timeStamp, elapsedTime.ToString("f2"), sessionTag + "(depreciated)", targetTag);
            }
        }
    }

    private bool CheckMotionThreshold()
    {
        bool motion;
        if (motionData.speed>motionThreshold)
        {
            motion = true;
        }
        else
        {
            motion = false;
        }

        return motion;
    }
    public void GetPositionRotation(){
        position = motionObject.transform.position;
        rotation = motionObject.transform.eulerAngles;
        Vector3 rot = motionObject.transform.eulerAngles;;
        EulerLimitExtension eulerLimitExtension = new EulerLimitExtension();
        rotation = eulerLimitExtension.NormaliseAngle(rot);
    }

    public void GetPhysicsMotion(){
        p_speed = motion.Speed;
        p_velocity = motion.Velocity;
        p_acceleration = motion.Acceleration;
        p_accelerationStrength = motion.AccelerationStrength;
        p_direction = motion.Direction;

        p_angularSpeed = motion.AngularSpeed;
        p_angularVelocity = motion.AngularVelocity;
        p_angularAcceleration = motion.AngularAcceleration;
        p_angularAccelerationStrength = motion.AngularAccelerationStrength;
        p_angularAxis = motion.AngularAxis;
    }
    
    private void GetMotion_Depreciated(){
        //speed = motion_Depc.GetVelocity();
        //speed taken from Recokner
        StartCoroutine(SpeedReckoner());
        speedSmooth = KalmanFilter(speed);
        
        acceleration = motion_Depc.GetAcceleration();
        accelerationSmooth = new Vector3(KalmanFilter(acceleration.x), KalmanFilter(acceleration.y), KalmanFilter(acceleration.z));
        averageAcceleration = motion_Depc.GetAccelerationAverage();
        averageAccelerationSmooth = KalmanFilter(averageAcceleration);

        //not saved in data writer
        angularAcceleration = motion_Depc.GetAngularAcceleration();
        angularAccelerationSmooth = new Vector3(KalmanFilter(angularAcceleration.x), KalmanFilter(angularAcceleration.y), KalmanFilter(angularAcceleration.z));
        averageAngularAcceleration = motion_Depc.GetAngularAccelerationAverage();
        averageAngularAccelerationSmooth = KalmanFilter(averageAngularAcceleration);
    }
    
    private void FormatMotionData()
    {
        motionData.tag = tag;
        motionData.position = position;
        motionData.rotation = rotation;

        motionData.speed = p_speed;
        motionData.velocity = p_velocity;
        motionData.acceleration = p_acceleration;
        motionData.accelerationStrength = p_accelerationStrength;
        motionData.direction = p_direction;

        motionData.angularSpeed = p_angularSpeed;
        motionData.angularVelocity = p_angularVelocity;
        motionData.angularAcceleration = p_angularAcceleration;
        motionData.angularAccelerationStrength = p_angularAccelerationStrength;
        motionData.angularAxis = p_angularAxis;

        motionDataStreaming.SetMotionData(motionTag, motionData);
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


    private float UpdateDelay = 1;

    // private Vector3 CalculateVelocity()
    // {
    //     Vector3 v = Vector3.zero;
    //     Vector3 previous;
    //     velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
    //     previous = motionObject.position;
    //     return v;
    // }
    
    private IEnumerator SpeedReckoner()
    {

        YieldInstruction timedWait = new WaitForSeconds(UpdateDelay);
        Vector3 lastPosition = transform.position;
        float lastTimestamp = Time.time;

        while (enabled)
        {
            yield return timedWait;

            var deltaPosition = (transform.position - lastPosition).magnitude;
            var deltaTime = Time.time - lastTimestamp;

            if (Mathf.Approximately(deltaPosition, 0f)) // Clean up "near-zero" displacement
                deltaPosition = 0f;

            speed = deltaPosition / deltaTime;
            // Speed = KalmanFilter(Speed);

            lastPosition = transform.position;
            lastTimestamp = Time.time;
        }
    }
}
