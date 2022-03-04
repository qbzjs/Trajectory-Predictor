using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetMagnitudeTracker : MonoBehaviour{
    
    [Header("Feedback DEBUG")] 
    public bool showFeedbackCanvas;
    public Canvas feedbackUI;
    
    public Slider feedbackSlider;
    public TextMeshProUGUI feedbackText;
    
    public float feedbackPercentage;
    public float feedbackAmplitude;

    
    [Header("Tracked Hand")] 
    public Vector3 handPosition;
    
    [Header("Values")] 
    
    public Transform target;
    [Range(0, 2f)] public float feedbackMultiplier = 1;
    public Vector3 startPoint;
    //private Vector3 targetOffset = new Vector3(0.31f, -0.55f, -2.7f);
    public float startDistance;
    public float currentDistance;
    private float p;
    public float percentageFromTarget;
    public float percentageToTarget;
    
    //depreciated
    private Vector3 endPoint;
    private Vector3 endPointOffset = new Vector3(0, 0, -0.15f);
    private float startToEndDistance;
    private float distanceS;
    private float distanceE;
    private float difference;


    private void OnEnable(){
        if (DAO.instance != null){
            //get start point at hand position when enabled
            startPoint = DAO.instance.motionDataRightWrist.position;
        }

        //depreciated
        // endPoint = target.position - endPointOffset;
        // startToEndDistance = (startPoint-endPoint).magnitude;
        
        
        feedbackUI.gameObject.SetActive(showFeedbackCanvas);
        
        startDistance = Vector3.Distance(startPoint, target.position);
    }

    void Start()
    {
        //this was to test
        //startPoint = start.position;
        
        //depreciated
        // endPoint = target.position - endPointOffset;;
        // startToEndDistance = (startPoint-endPoint).magnitude;
        
        feedbackUI.gameObject.SetActive(showFeedbackCanvas);
        
        startDistance = Vector3.Distance(startPoint, target.position);
    }

    //call as one shot!
    // public void TrackMagnitude()
    // {
    //     if (DAO.instance != null)
    //     {
    //         handPosition = DAO.instance.MotionData_RightWrist.position;
    //         startPoint = handPosition;
    //     }
    // }
    // public void TrackMagnitude(Transform t)
    // {
    //     if (DAO.instance != null)
    //     {
    //         handPosition = DAO.instance.MotionData_RightWrist.position;
    //         startPoint = handPosition;
    //     }
    // }
    void Update()
    {

        if (TrackedObjectReference.instance != null){
            handPosition = TrackedObjectReference.instance.currentTrackedObject.position;
        }

        //depreciated
        // distanceS = (startPoint - handPosition).magnitude;
        // distanceE = (endPoint- handPosition).magnitude;
        // difference = distanceS - distanceE;
        // percentageToTarget = (difference / startToEndDistance) *100;
        
        
        //method--------------
        currentDistance = Vector3.Distance(TrackedObjectReference.instance.currentTrackedObject.position, target.transform.position);

        percentageFromTarget =(currentDistance / startDistance) * 100f; // for inverse method (100% is max distance away)
        
        if (currentDistance<=startDistance){
            p = (currentDistance - startDistance) / startDistance * 100;
            percentageToTarget = -p; //p is negative - flip positive
        }
        
        if (percentageToTarget < 0)
        {
            percentageToTarget = 0;
        }
        if (percentageToTarget > 100)
        {
            percentageToTarget = 100;
        }

        feedbackPercentage = percentageToTarget;
        feedbackAmplitude = percentageToTarget / 100;

        
        feedbackSlider.value = feedbackAmplitude;
        feedbackText.text = Mathf.RoundToInt(feedbackPercentage).ToString() + "%";
    }

}
