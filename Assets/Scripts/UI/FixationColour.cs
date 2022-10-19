using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class FixationColour : MonoBehaviour
{
    public Renderer fixationRenderer;
    public Material fixationKinematic;
    public Material fixationImagined;
    public Material fixationStreak;

    private RunType runType = RunType.Null;
    
    #region Subscriptions

    private void OnEnable(){
        GameManager.OnBlockAction += GameManagerOnBlockAction;
        ScoreManager.OnTargetStreakAction += ScoreManagerOnTargetStreakAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
        ScoreManager.OnTargetStreakAction -= ScoreManagerOnTargetStreakAction;
    }
    private void ScoreManagerOnTargetStreakAction(bool streakFeedback, int streakCount, bool showUI){
        if (showUI){
            fixationRenderer.material = fixationStreak;
        }
        else{
            runType = Settings.instance.currentRunType;
            if (runType == RunType.Kinematic){
                fixationRenderer.material = fixationKinematic;
            }
            if (runType == RunType.Imagined){
                fixationRenderer.material = fixationImagined;
            }
        }
    }
    
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        SetFixation();
        if (eventType == GameStatus.Countdown){
            SetFixation();
        }
        if (eventType == GameStatus.Initialised){
            SetFixation();
        }
    }
    #endregion

    public void SetFixation(){
        runType = Settings.instance.currentRunType;
        if (runType == RunType.Kinematic){
            fixationRenderer.material = fixationKinematic;
        }
        if (runType == RunType.Imagined){
            fixationRenderer.material = fixationImagined;
        }
    }
}