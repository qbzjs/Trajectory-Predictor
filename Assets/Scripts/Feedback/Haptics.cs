using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    public bool hapticFlag;
    
    public SteamVR_Action_Vibration hapticAction;
    public SteamVR_Action_Boolean trackpadAction;

    public GameObject activeTarget;
    public TargetMagnitudeTracker targetMagnitudeTracker;
    
    public float duration = 0.1f;
    
    public float feedbackAmplitude;
    public float feedbackPercentage;
    
    private void OnEnable(){
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
    }
    private void OnDisable(){
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 position, Transform activeTarget){
        //trial started
        if (targetPresent){
            hapticFlag = true;
            this.activeTarget = activeTarget.gameObject;
            targetMagnitudeTracker = this.activeTarget.GetComponent<TargetMagnitudeTracker>();
        }
        //Executes after a trial...
        if (restPresent){
            hapticFlag = false;
        }
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)){
            Pulse(1,150,75, SteamVR_Input_Sources.Any);
        }

        if (hapticFlag && targetMagnitudeTracker!=null){
            feedbackAmplitude = targetMagnitudeTracker.feedbackAmplitude;
            feedbackPercentage = targetMagnitudeTracker.feedbackPercentage;
            
            hapticAction.Execute(0, duration, feedbackPercentage ,feedbackAmplitude/2,SteamVR_Input_Sources.Any);
            //Vibrate(0,0.1f, );
        }
        
    }

    private void Vibrate(float startDelay, float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency,amplitude,source);
    }
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency,amplitude,source);
    }
}
