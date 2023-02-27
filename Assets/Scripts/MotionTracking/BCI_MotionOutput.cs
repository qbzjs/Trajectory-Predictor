using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BCI_MotionOutput : MonoBehaviour
{
    public MotionTag motionType = MotionTag.BCI_Target;

    public Transform motionTrackerObject;
    //public Transform BCI_predictedObject;
    //public Transform BCI_targetObject;
    //public Transform BCI_assistedObject;
    //public Transform BCI_refinedObject;

    private void Awake()
    {
        if(motionTrackerObject == null)
        {
            motionTrackerObject = this.transform;
        }
    }
    #region Subscriptions

    private void OnEnable(){
        BCI_ControlManager.OnControlSignal += BCI_ControlManagerOnControlSignal;
    }

    private void BCI_ControlManagerOnControlSignal(Vector3 cvPredicted, Vector3 cvTarget, Vector3 cvAssisted, Vector3 cvRefined){

        if(motionType == MotionTag.BCI_Target)
        {
            motionTrackerObject.position = cvTarget;
        }
        if (motionType == MotionTag.BCI_Predicted)
        {
            motionTrackerObject.position = cvPredicted;
        }
        if (motionType == MotionTag.BCI_Assisted)
        {
            motionTrackerObject.position = cvAssisted;
        }
        if (motionType == MotionTag.BCI_Refined)
        {
            motionTrackerObject.position = cvRefined;
        }
        //BCI_predictedObject.position = cvPredicted;
        //BCI_targetObject.position = cvTarget;
        //BCI_assistedObject.position = cvAssisted;
        //BCI_refinedObject.position = cvRefined;
    }

    #endregion
}
