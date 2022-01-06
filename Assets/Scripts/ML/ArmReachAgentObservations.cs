using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class ArmReachAgentObservations : MonoBehaviour{

    [Header("SELECT BCI OR KINEMATIC")]
    public bool trainFromBCI;
    public bool trainFromKinematic;

    [Header("GENERAL")]
    public bool targetPresent;
    public bool restPresent;
    public Vector3 targetPosition;
    
    [Header("BCI - Online")]
    public Vector3 velocityPrediction;
    
    [Header("KINEMATIC")]
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 rotation;
    public Vector3 angularVelocity;

    private DAO dao;

    private void OnEnable(){
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
    }
    private void OnDisable(){
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 targetPosition){
        this.targetPresent = targetPresent;
        this.restPresent = restPresent;
        this.targetPosition = targetPosition;
    }


    void Awake()
    {
        if (trainFromKinematic && trainFromBCI){
            trainFromKinematic = false;
            trainFromBCI = false;
            Debug.Log("Not Training = select BCI or Kinematic training");
        }
    }

    private void Start(){
        if (DAO.instance != null){
            dao = DAO.instance;
        }
    }

    public bool TargetPresent(){
        return targetPresent;
    }
    public bool RestPresent(){
        return restPresent;
    }
    void Update(){
        if (dao){
            if (trainFromBCI){
                //get bci data???
                //velocityPrediction = dao?????
            }

            if (trainFromKinematic){
                position = dao.motionDataRightWrist.position;
                rotation = dao.motionDataRightWrist.rotation;
                velocity = dao.motionDataRightWrist.velocity;
                angularVelocity = dao.motionDataRightWrist.angularVelocity;
            }
        }
    }
}
