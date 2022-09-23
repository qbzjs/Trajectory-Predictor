using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class IndicatorColour : MonoBehaviour
{
    public Renderer indicatorRenderer;
    public Material indicaton;
    public Material target;

    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnTrialAction;
    }
    private void OnDisable(){
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
    }
    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifetime, int index, int total){
        // Debug.Log(eventType.ToString());
        SetIndication(eventType);
    }
    public void SetIndication(TrialEventType trialEvent){
        if (trialEvent == TrialEventType.Fixation || trialEvent == TrialEventType.Indication 
           || trialEvent == TrialEventType.Rest || trialEvent == TrialEventType.TrialComplete 
           || trialEvent == TrialEventType.PostTrialPhase || trialEvent == TrialEventType.Ready){
            indicatorRenderer.material = indicaton;
        }

        if (trialEvent == TrialEventType.TargetPresentation){
            indicatorRenderer.material = target;
        }
    }
}


