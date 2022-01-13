using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using RootMotion.FinalIK;
using UnityEngine;

public class KinematicArmController : MonoBehaviour
{
    public VRIK kinematicVR;
    
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    
    public Vector3 targetPosition;
    public Vector3 homePosition;

    public float animationDuration;
    
    #region Subscriptions
    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnTrialAction;
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
    }
    private void OnDisable(){
        
    }
    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifetime, int index, int total){
        animationDuration = lifetime;
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 position){
        if (targetPresent){
            targetPosition = position;
        }
        if (restPresent){
            homePosition = position; 
        }
    }
    #endregion
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
