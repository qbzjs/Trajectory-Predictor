using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class StreakFeedback : MonoBehaviour
{
    public MMFeedbacks targetFeedback;
    
    private void OnEnable(){
        ScoreManager.OnTargetStreakAction += ScoreManagerOnTargetStreakAction;
    }
    private void OnDisable(){
        ScoreManager.OnTargetStreakAction -= ScoreManagerOnTargetStreakAction;
    }
    private void ScoreManagerOnTargetStreakAction(bool streakFeedback, int streakCount){
        if (streakFeedback){
            targetFeedback.PlayFeedbacks();
        }
    }

    void Start(){
        targetFeedback = gameObject.GetComponent<MMFeedbacks>();
        targetFeedback.PlayFeedbacks();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            targetFeedback.PlayFeedbacks();
        }
    }
}
