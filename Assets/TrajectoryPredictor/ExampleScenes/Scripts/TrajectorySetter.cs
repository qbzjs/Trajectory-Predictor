using System;
using System.Collections;
using System.Collections.Generic;
using Filters;
using Unity.Labs.SuperScience.Example;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Labs.SuperScience.Example{
    public class TrajectorySetter : MonoBehaviour
    {
        public Vector3 resistance;
        public float forceMultiplier = 1f;
        public Vector3 velocity;
        public Vector3 direction;
        public float speed;
        public float magnitude = 0f;
        
        
        public Vector3 originPoint;

        public Text infoText;

        //create a trajectory predictor in code
        private TrajectoryPredictor trajectoryPredictor;

        private KalmanFilter kFilter;
        
        [Header("KALMAN FILTER")]
        public bool useFilter = false;
        [SerializeField] [Range(0, 4)] public float Q_measurementNoise;// measurement noise

        [SerializeField] [Range(0, 100)] public float R_EnvironmentNoize; // environment noise
        [SerializeField] [Range(0, 100)] public float F_facorOfRealValueToPrevious; // factor of real value to previous real value
        [SerializeField] [Range(0.1f, 100)] public float H_factorOfMeasuredValue; // factor of measured value to real value

        [Header("PREDICTION POINTS")]
        private List<Vector3> predictionPoints = new List<Vector3>();

        void Start(){
            if (!gameObject.GetComponent<TrajectoryPredictor>()){
                trajectoryPredictor = gameObject.AddComponent<TrajectoryPredictor>();
            }
            else{
                trajectoryPredictor = gameObject.GetComponent<TrajectoryPredictor>();
            }

            trajectoryPredictor.drawDebugOnPrediction = true;
            trajectoryPredictor.reuseLine = true; //set this to true so the line renderer gets reused every frame on prediction
            trajectoryPredictor.accuracy = 0.99f;
            trajectoryPredictor.lineWidth = 0.02f;
            //trajectoryPredictor.iterationLimit = 600;
            //trajectoryPredictor.lineStartColor = Color.green;
            
            kFilter = new KalmanFilter(RealValueToPreviousRealValue: F_facorOfRealValueToPrevious, MeasuredToRealValue: H_factorOfMeasuredValue, mesurementNoize: Q_measurementNoise, environmentNoize: R_EnvironmentNoize);
            //kFilter = new KalmanFilter();
            kFilter.State = 0; //Setting first (non filtered) value to 0 for example;
        }

        private void Update(){
            velocity = gameObject.GetComponent<PhysicsData>().m_MotionData.Velocity;
            direction = gameObject.GetComponent<PhysicsData>().m_MotionData.Direction;
            speed = gameObject.GetComponent<PhysicsData>().m_MotionData.Speed;
            if (useFilter){
                velocity = KalmanFilter(velocity);
                direction = KalmanFilter(direction);
                speed = KalmanFilter(speed);
            }
            magnitude = velocity.magnitude * speed*forceMultiplier;

            originPoint = GetComponent<MotionTracker>().motionObject.transform.position;
        }

        void LateUpdate(){
            if (GetComponent<MotionTracker>().inMotion){
                //set line duration to delta time so that it only lasts the length of a frame
                trajectoryPredictor.debugLineDuration = Time.unscaledDeltaTime;
                //tell the predictor to predict a 3d line. this will also cause it to draw a prediction line
                //because drawDebugOnPredict is set to true
                //trajectoryPredictor.Predict3D(originPoint.position, originPoint.forward * forceMultiplier, Physics.gravity);
                trajectoryPredictor.Predict3D(originPoint, direction * forceMultiplier, resistance);
                //trajectoryPredictor.Predict3D(originPoint.position, direction * magnitude, Physics.gravity);

                //this static method can be used as well to get line info without needing to have a component and such
                //TrajectoryPredictor.GetPoints3D(launchPoint.position, launchPoint.forward * force, Physics.gravity);
                
                predictionPoints = trajectoryPredictor.predictionPoints;
            }
            else{
                
                trajectoryPredictor.Predict3D(originPoint, Vector3.zero, Vector3.zero);
                
                predictionPoints = trajectoryPredictor.predictionPoints;
            }

            //info text stuff
            if (infoText){
                //this will check if the predictor has a hitinfo and then if it does will update the onscreen text
                //to say the name of the object the line hit;
                if (trajectoryPredictor.hitInfo3D.collider)
                    infoText.text = "Hit Object: " + trajectoryPredictor.hitInfo3D.collider.gameObject.name;
            }
            
            // Debug.Log(trajectoryPredictor.);
            
            
        }
        
        private Vector3 KalmanFilter(Vector3 value) {
            float vX = kFilter.FilterValue(value.x); //applying filter
            float vY = kFilter.FilterValue(value.y); //applying filter
            float vZ = kFilter.FilterValue(value.z); //applying filter

            Vector3 filteredVector = new Vector3(vX, vY, vZ);
            return filteredVector;
        }
        private float KalmanFilter(float value) {
            float v = kFilter.FilterValue(value); //applying filter
            return v;
        }
    }
}
