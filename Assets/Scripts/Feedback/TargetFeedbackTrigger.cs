using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using Enums;

public class TargetFeedbackTrigger : MonoBehaviour
{
    public bool feedbackActive;

    [Header("Feedbacks")]
    /// a MMFeedbacks to play when the Hero starts jumping
    public MMFeedbacks targetFeedback;

    private bool feedbackReady;
    private bool targetReady = false;

    private RunType rType = RunType.Imagined;
    private int tNumber;
    
    public delegate void TargetHit(RunType runType, int tNum);
    public static event TargetHit OnTargetHit;
    
    #region Event Subscriptions
    private void OnEnable(){
        TargetManager.OnTargetAction += TargetManagerOnTargetAction;
    }
    private void OnDisable(){
        TargetManager.OnTargetAction -= TargetManagerOnTargetAction;
    }
    private void TargetManagerOnTargetAction(bool targetPresent, bool restPresent, Vector3 position, Transform activeTarget){
        //trial started
        if (targetPresent){
            
            feedbackReady = true;
            feedbackActive = true;
            targetReady = true;
            this.gameObject.GetComponent<Collider>().enabled = true;
        }
        //Executes after a trial...
        if (restPresent){
            if (feedbackReady == true){ //didn;t hit the target during the trial
                ScoreManager.instance.ResetStreak();
            }
            feedbackReady = false;
        }
    }

    #endregion

    
    void Awake(){
        if (targetFeedback == null){
            if (transform.GetChild(0).GetComponent<MMFeedbacks>() != null){
                targetFeedback = transform.GetChild(0).GetComponent<MMFeedbacks>();
            }
        }
    }

    public void OnTriggerEnter(Collider other){
        if (other.GetComponent<Tag>() && targetReady){
            targetReady = false;
            if (other.GetComponent<Tag>().tag == "fingers" && feedbackReady == true){
                this.gameObject.GetComponent<Collider>().enabled = false;
                feedbackReady = false;
                //add target hit score
                if (OnTargetHit != null){
                    OnTargetHit(rType,0);
                }
                ScoreManager.instance.AddToStreak();
                ScoreManager.instance.AddTargetHit();
                Feedback();
            }
        }
    }

    public void Feedback(){
        if (feedbackActive){
            feedbackActive = false;
            targetFeedback.PlayFeedbacks();
        }
    }
}