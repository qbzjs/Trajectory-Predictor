using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// this class holds tracked objects and retrurns the current tracked object
/// </summary>
/// 
public class TrackedObjectReference : MonoBehaviour
{
    public static TrackedObjectReference instance;
    
    public Transform currentTrackedObject; //the object currently controlled by the user - kinematic or bci control
    
    public Transform leftWrist;
    public Transform rightWrist;
    public Transform controlObjectBCI;

    public delegate void TrackedObject(Transform trackedObject);
    public static event TrackedObject OnTrackedObject;
    
    private void Awake(){
        instance = this;
    }

    private void Start(){
        EnableKinematic();
        if (OnTrackedObject != null){
            OnTrackedObject(currentTrackedObject);
        }
    }

    #region Subscriptions

    private void OnEnable(){
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnRunAction += GameManagerOnRunAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnRunAction -= GameManagerOnRunAction;
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            
        }
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal, RunType runType){
        if (eventType == GameStatus.RunComplete){
            EnableKinematic();
        }
        if (eventType == GameStatus.AllRunsComplete){
            EnableKinematic();
        }
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifetime, int blockIndex, int blockTotal){
        if (eventType==GameStatus.BlockComplete){
            EnableKinematic();
        }
        if (eventType == GameStatus.Countdown){
            //SWITCH HANDS ON COUNTDOWN
            if (Settings.instance.currentRunType == RunType.Imagined){
                EnableBCI();
            }
            if (Settings.instance.currentRunType == RunType.Kinematic){
                EnableKinematic();
            }
        }
    }

    #endregion

    public Transform CurrentTrackedObject(){
        return currentTrackedObject;
    }

    private void EnableKinematic(){
        if (Settings.instance.handedness == Handedness.Left){
            currentTrackedObject = leftWrist;
        }
        if (Settings.instance.handedness == Handedness.Right){
            currentTrackedObject = rightWrist;
        }
        if (OnTrackedObject != null){
            OnTrackedObject(currentTrackedObject);
        }
    }

    private void EnableBCI(){
        currentTrackedObject = controlObjectBCI;
        if (OnTrackedObject != null){
            OnTrackedObject(currentTrackedObject);
        }
    }
}
