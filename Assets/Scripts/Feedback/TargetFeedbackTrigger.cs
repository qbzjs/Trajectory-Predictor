using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class TargetFeedbackTrigger : MonoBehaviour
{
    public bool feedbackActive;

    [Header("Feedbacks")]
    /// a MMFeedbacks to play when the Hero starts jumping
    public MMFeedbacks targetFeedback;

    private bool feedbackReady;
    
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
        if (other.GetComponent<Tag>()){
            if (other.GetComponent<Tag>().tag == "fingers" && feedbackReady == true){
                this.gameObject.GetComponent<Collider>().enabled = false;
                feedbackReady = false;
                ScoreManager.instance.AddToStreak();
                Feedback();
            }
        }
    }

    public void Feedback(){
        if (feedbackActive){
            targetFeedback.PlayFeedbacks();
        }
    }
}