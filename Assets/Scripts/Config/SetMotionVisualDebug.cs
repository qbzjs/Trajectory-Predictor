using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMotionVisualDebug : MonoBehaviour
{
    public bool showMotionDebug;
    public bool showReachTargetDebug;
    public bool showKinematicDebug;

    [Header("MOTION VISUALS")]
    public GameObject targetedTrajectoryVisual;
    public GameObject wristTrackerCubeRight;
    public GameObject wristTrackerCubeLeft;
    
    //send event to physics trackers???
    
    [Header("REACH TARGET VISUALS")]
    public GameObject reachTarget;
    public GameObject lerpTarget;

    [Header("KINEMATIC VISUALS")]
    public GameObject upperBodyIK;
    
    void Start()
    {
        SetVisuals();
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Tab)){
            SetVisuals();
        }
    }

    public void SetVisuals(){
        SetMotionDebug(showMotionDebug);
        SetReachTargetDebug(showReachTargetDebug);
        SetKinematicDebug(showKinematicDebug);
    }

    private void SetMotionDebug(bool b){
        targetedTrajectoryVisual.SetActive(b);
        wristTrackerCubeRight.GetComponent<Renderer>().enabled = b;
        wristTrackerCubeLeft.GetComponent<Renderer>().enabled = b;
        //send event to physics data...
    }
    private void SetReachTargetDebug(bool b){
        reachTarget.SetActive(b);
        lerpTarget.SetActive(b);
    }
    private void SetKinematicDebug(bool b){
        upperBodyIK.SetActive(b);
    }
}
