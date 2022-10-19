using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EndBlockFeedback : MonoBehaviour
{
    public MMFeedbacks targetFeedback;
    
    private void OnEnable(){
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.BlockComplete){
            targetFeedback.PlayFeedbacks();
        }
    }
    void Start(){
        targetFeedback = gameObject.GetComponent<MMFeedbacks>();
    }

}
