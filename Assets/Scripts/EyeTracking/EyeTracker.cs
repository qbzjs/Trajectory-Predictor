using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using Enums;
using Tobii.Gaming.Examples.GazePointData;
using UnityEngine.UI;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class EyeTracker : MonoBehaviour
            {
                #region Eye Tracker Variables

                public bool visualiseEyeData;
                
                public int LengthOfRay = 25;

                [SerializeField] private LineRenderer GazeRayRenderer;
                private static EyeData_v2 eyeData = new EyeData_v2();
                private bool eye_callback_registered = false;

                public EyeDataFormat eyeDataFormat;

                static Vector3 gazeDirectionLeft;
                static Vector3 gazeDirectionRight;

                static float xCoord2D;
                static float yCoord2D;

                static float eyeOpennessLeft;
                static float eyeOpennessRight;

                static float pupilDiameterLeft;
                static float pupilDiameterRight;

                public Vector3 rayDirection;

                #endregion


                //debug display
                public TextMeshPro blinkDisplay;
                public TextMeshPro gazeDataDisplay;
                public TextMeshPro eyeOpennessDisplay;
                public TextMeshPro pupilDiameterDisplay;
                
                public TextMeshPro gazeHitObjectDisplay;
                
                private void Start()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull(GazeRayRenderer);

                    // originOffset = new Vector3(Camera.main.transform.position.x,
                    //     Camera.main.transform.position.y + rayOffsetVertical, Camera.main.transform.position.z);
                }

                private void Update()
                {
                    

                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                    
                    if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
                    {
                        SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = true;
                    }
                    else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                    
                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
                    
                    if (eye_callback_registered)
                    {
                        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else return;
                    }
                    else
                    {
                        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else return;
                    }

                    if (visualiseEyeData)
                    {
                        Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                        GazeRayRenderer.SetPosition(0, Camera.main.transform.position  - Camera.main.transform.up * 0.05f); 
                        GazeRayRenderer.SetPosition(1, Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);

                        if (gazeDataDisplay){
                            gazeDataDisplay.text = "Left Gaze Direction : " + gazeDirectionLeft.ToString("F2") + "  :::  Right Gaze Direction : " + gazeDirectionRight.ToString("F2");
                            eyeOpennessDisplay.text = "Left Eye Openness : " + eyeOpennessLeft.ToString("F2") + "  :::  Right Eye Openness : " + eyeOpennessRight.ToString("F2");
                            pupilDiameterDisplay.text = "Left Pupil Diameter : " + pupilDiameterLeft.ToString("F2") + "  :::  Right Pupil Diameter : " + pupilDiameterRight.ToString("F2");
                            if (blinking)
                            {
                                blinkDisplay.text = "BLINKING!";
                            }
                            else
                            {
                                blinkDisplay.text = "NOT BLINKING!";
                            }
                        }
                    }

                    rayDirection = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                    
                    

                }

                private void Release()
                {
                    if (eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                }
                private static void EyeCallback(ref EyeData_v2 eye_data)
                {
                    eyeData = eye_data;
                    gazeDirectionLeft = eyeData.verbose_data.left.gaze_direction_normalized;
                    gazeDirectionRight = eyeData.verbose_data.right.gaze_direction_normalized;

                    eyeOpennessLeft = eyeData.verbose_data.left.eye_openness;
                    eyeOpennessRight = eyeData.verbose_data.right.eye_openness;
                    
                    pupilDiameterLeft = eyeData.verbose_data.left.pupil_diameter_mm;
                    pupilDiameterRight = eyeData.verbose_data.right.pupil_diameter_mm;
                }


                #region Data Writing Variables

                // DATA WRITING *************************************
                private DataWriter dataWriter;
                
                public bool recordEnabled;
                [Space(10)]
                
                private float elapsedTime;
                private string timeStamp;
                
                public MotionTag motionTag;
                public string sessionTag = "Session Name Here";
                public string fileName = "Name Here";
                private string testID;
                public int targetNumber;
                private string targetTag = "Target Tag Here";
                public string id;
                
                private bool recordEyeData = false;
                
                [Space(8)]
                [Range(0, 1f)] 
                public float blinkThreshold = 0.05f;
                public bool blinking;

                #endregion

                

                #region Subscription Events

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
                        if (!recordEyeData && recordEnabled)
                        {
                            //Debug.Log("---- Start Eye Tracking Record : " + fileName);
                            //testID = jointTag + "_" + System.Guid.NewGuid().ToString();
                            dataWriter = new DataWriter();
                            fileName = GenerateFileName();
                            testID = fileName + "_" + id;
                            elapsedTime = 0;
                            recordEyeData = true;
                        }
                        else
                        {
                            print("TODO - VERIFY EYE TRACKING RECORDING");
                            //Debug.Log("---- Stop Eye Tracking Tracking : " + fileName);
                            recordEyeData = false;
                            dataWriter.WriteData(testID);
                        }
                    }

                }               

                #endregion

                private void LateUpdate(){
                    Debug.DrawRay(Camera.main.transform.position, rayDirection, Color.green);
                }

                void FixedUpdate()
                {
                    //RAYCAST TO CHECK TARGETS
                    RaycastHit hit;

                    
                    
                    if (Physics.Raycast(Camera.main.transform.position, rayDirection, out hit)){
                        // if (hit.collider.isTrigger){
                        //     print("Trigger - distance: " + hit.distance + " : " + hit.transform.name);
                        //     gazeHitObjectDisplay.text = "Trigger - distance: " + hit.distance + " : " + hit.transform.name;
                        //     Debug.DrawLine(Camera.main.transform.position, hit.transform.position,Color.green);
                        //     hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
                        // }
                        // else{
                        //     print("Collider - distance: " + hit.distance + " : " + hit.transform.name);
                        //     gazeHitObjectDisplay.text = "Collider - distance: " + hit.distance + " : " + hit.transform.name;
                        //     Debug.DrawLine(Camera.main.transform.position, hit.transform.position,Color.magenta);
                        //     hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                        // }
                        if (hit.collider.isTrigger){
                            if (hit.transform.GetComponent<GazeObectTag>()){
                                gazeHitObjectDisplay.text = "Gaze distance: " + hit.distance + "\n" +" : " + hit.transform.name + ": " + hit.transform.GetComponent<GazeObectTag>().tag;
                                
                                if (hit.transform.GetComponent<GazeObectTag>().tag==""){
                                    gazeHitObjectDisplay.color = Color.grey;
                                }
                                if (hit.transform.GetComponent<GazeObectTag>().tag=="0"){
                                    gazeHitObjectDisplay.color = Color.magenta;
                                }
                                if (hit.transform.GetComponent<GazeObectTag>().tag=="1"||hit.transform.GetComponent<GazeObectTag>().tag=="2"
                                ||hit.transform.GetComponent<GazeObectTag>().tag=="3"||hit.transform.GetComponent<GazeObectTag>().tag=="4"){
                                    gazeHitObjectDisplay.color = Color.cyan;
                                }

                            }
                        }
                        else{
                            gazeHitObjectDisplay.color = Color.grey;
                            gazeHitObjectDisplay.text = "No Target at Gaze";
                        }
                    }
                        
                    
                    blinking = CheckBlinkThreshold();
                    
                    RouteEyeData();
                    
                    if(recordEyeData && recordEnabled){
            
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
                            targetTag = "";
                        }

            
                        // dataWriter.WriteTrajectoryData(timeStamp, elapsedTime.ToString("f2"), sessionTag, targetTag, position, rotation,
                        //     p_speed,p_velocity,p_acceleration,p_accelerationStrength,p_direction,
                        //     p_angularSpeed,p_angularVelocity,p_angularAcceleration,p_angularAccelerationStrength,p_angularAxis);
                        
                        //2D Gaze tracking!!!!!!!!!
                        if (gameObject.GetComponent<EyeTracker2D>()){
                            xCoord2D = gameObject.GetComponent<EyeTracker2D>().xCoord;
                            yCoord2D = gameObject.GetComponent<EyeTracker2D>().yCoord;
                        }
                        
                        dataWriter.WriteEyeData(timeStamp, elapsedTime.ToString("f2"), motionTag.ToString(), targetTag,
                            blinking.ToString(), eyeOpennessLeft, eyeOpennessRight, gazeDirectionLeft, gazeDirectionRight,
                            pupilDiameterLeft, pupilDiameterRight, xCoord2D, yCoord2D);
                    }
                }

                #region Data and General Functions

                               private void RouteEyeData()
                {
                    //format the eyedata for saving
                    eyeDataFormat.blinking = blinking;
                    eyeDataFormat.gazeDirectionLeft = gazeDirectionLeft;
                    eyeDataFormat.gazeDirectionRight = gazeDirectionRight;
                    eyeDataFormat.eyeOpennessLeft = eyeOpennessLeft;
                    eyeDataFormat.eyeOpennessRight = eyeOpennessRight;
                    eyeDataFormat.pupilDiameterLeft = pupilDiameterLeft;
                    eyeDataFormat.pupilDiameterRight = pupilDiameterRight;
                    eyeDataFormat.xCoord2D = xCoord2D;
                    eyeDataFormat.yCoord2D = yCoord2D;
                    
                    if (DAO.instance != null)
                    {
                        DAO.instance.eyeData = eyeDataFormat;
                    }
                }
                private bool CheckBlinkThreshold()
                {
                    bool blink;
                    if (eyeOpennessLeft < blinkThreshold || eyeOpennessRight < blinkThreshold)
                    {
                        blink = true;
                    }
                    else
                    {
                        blink = false;
                    }

                    return blink;
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

                #endregion
 
            }
        }
    }
}
