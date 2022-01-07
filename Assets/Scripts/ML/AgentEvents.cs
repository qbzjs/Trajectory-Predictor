using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class AgentEvents : MonoBehaviour
{
    [SerializeField] private ArmReachAgent armReachAgent;
    
    private void OnEnable(){
        GameManager.OnTrialAction += GameManagerOnOnTrialAction;
    }
    private void OnDisable(){
        GameManager.OnTrialAction -= GameManagerOnOnTrialAction;
    }
    private void GameManagerOnOnTrialAction(TrialEventType eventType, int targetnum, float lifetime, int index, int total){
        if (eventType == TrialEventType.Initialise){
            armReachAgent.EndEpisodeExternal();
        }
        if (eventType == TrialEventType.PostTrialPhase){
            armReachAgent.EndEpisodeExternal();
        }
    }
}
