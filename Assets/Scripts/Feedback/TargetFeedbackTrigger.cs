using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

public class TargetFeedbackTrigger : MonoBehaviour
{
    [Header("Feedbacks")]
    /// a MMFeedbacks to play when the Hero starts jumping
    public MMFeedbacks targetFeedback;
    
    void Awake()
    {
        if (targetFeedback == null)
        {
            if (transform.GetChild(0).GetComponent<MMFeedbacks>() != null)
            {
                targetFeedback = transform.GetChild(0).GetComponent<MMFeedbacks>();
            }
        }
    }

    public void Feedback()
    {
        targetFeedback.PlayFeedbacks();
    }
}