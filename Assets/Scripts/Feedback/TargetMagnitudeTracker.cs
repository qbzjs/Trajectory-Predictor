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
    public Vector3 endPoint;
    public Vector3 endPointOffset = new Vector3(0, 0, -0.15f);
    public float startToEndDistance;
    public float distanceS;
    public float distanceE;
    public float difference;
    private float percentageToTarget;


    private void OnEnable(){
        if (DAO.instance != null){
            //get start point at hand position when enabled
            startPoint = DAO.instance.motionDataRightWrist.position;
        }

        endPoint = target.position - endPointOffset;
        startToEndDistance = (startPoint-endPoint).magnitude;
        
        feedbackUI.gameObject.SetActive(showFeedbackCanvas);
    }

    void Start()
    {
        //this was to test
        //startPoint = start.position;
        
        endPoint = target.position - endPointOffset;;
        startToEndDistance = (startPoint-endPoint).magnitude;
    }

    //call as one shot!
    public void TrackMagnitude()
    {
        if (DAO.instance != null)
        {
            handPosition = DAO.instance.MotionData_RightWrist.position;
            startPoint = handPosition;
        }
    }
    public void TrackMagnitude(Transform t)
    {
        if (DAO.instance != null)
        {
            handPosition = DAO.instance.MotionData_RightWrist.position;
            startPoint = handPosition;
        }
    }
    void Update()
    {
        // if (DAO.instance != null)
        // {
        //     handPosition = DAO.instance.MotionData_RightWrist.position;
        // }

        if (TrackedObjectReference.instance != null){
            handPosition = TrackedObjectReference.instance.currentTrackedObject.position;
        }

        distanceS = (startPoint - handPosition).magnitude;
        distanceE = (endPoint- handPosition).magnitude;
        difference = distanceS - distanceE;

        percentageToTarget = (difference / startToEndDistance) *100;
        
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
