using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;

public class UI_DisplayText : MonoBehaviour {
    public static UI_DisplayText instance;

    public TextMeshProUGUI pauseDisplay;
    public TextMeshProUGUI statusDisplay;
    public TextMeshPro statusDisplay_Wrld;
    public TextMeshProUGUI runProgressDisplay;
    public TextMeshPro runProgressDisplay_Wrld;
    public TextMeshProUGUI blockProgressDisplay;
    public TextMeshPro blockProgressDisplay_Wrld;
    public TextMeshProUGUI trialProgressDisplay;
    public TextMeshPro trialProgressDisplay_Wrld;
    public TextMeshProUGUI trialTotalProgressDisplay;
    public TextMeshPro trialTotalProgressDisplay_Wrld;

    public Slider progressSlider;

    public TextMeshProUGUI trialPhaseDisplay;

    //public TextMeshProUGUI scoreDisplay;
    //public TextMeshPro scoreDisplay_Wrld;

    public Color defaultColour = Color.white;
    public Color completeColour = Color.green;


    private void OnEnable(){
        GameManager.OnProgressAction += GameManagerOnProgressAction;
    }
    private void OnDisable(){
        GameManager.OnProgressAction -= GameManagerOnProgressAction;
    }
    private void GameManagerOnProgressAction(GameStatus eventType, float completionPercentage, int run, int runTotal, int block, int blockTotal, int trial, int trialTotal){
        SetProgressSlider(completionPercentage);
    }

    private void Awake() {
        instance = this;
        
        //scoreDisplay.text = "Score : n/a";
        //scoreDisplay_Wrld.text = "Score: n/a";
        blockProgressDisplay.text = "Run: 0 / 0";
        blockProgressDisplay_Wrld.text = "Run: 0 / 0";
        blockProgressDisplay.text = "Block: 0 / 0";
        blockProgressDisplay_Wrld.text = "Block: 0 / 0";
        trialProgressDisplay.text = "Trial: 0 / 0";
        trialProgressDisplay_Wrld.text = "Trial: 0 / 0";
        trialTotalProgressDisplay.text = "Total Trials: 0 / 0";
        trialTotalProgressDisplay_Wrld.text = "Total Trials: 0 / 0";

        trialPhaseDisplay.text = "Trial Phase Detail...";
    }

    void Start() {
        SetStatus(GameStatus.Ready, "System Ready");
    }
    

    public void SetRunProgress(int c, int t) {
        runProgressDisplay.text = "Run: " + c.ToString() + " / " + t.ToString();
        runProgressDisplay_Wrld.text = "Run: " + c.ToString() + " / " + t.ToString();
    }
    public void SetBlockProgress(int c, int t) {
        blockProgressDisplay.text = "Block: " + c.ToString() + " / " + t.ToString();
        blockProgressDisplay_Wrld.text = "Block: " + c.ToString() + " / " + t.ToString();
    }
    public void SetTrialProgress(int c, int t) {
        trialProgressDisplay.text = "Trial: " + c.ToString() + " / " + t.ToString();
        trialProgressDisplay_Wrld.text = "Trial: " + c.ToString() + " / " + t.ToString();
    }
    public void SetTrialTotalProgress(int c, int t) {
        trialTotalProgressDisplay.text = "Total Trials: " + c.ToString() + " / " + t.ToString();
        trialTotalProgressDisplay_Wrld.text = "Total Trials: " + c.ToString() + " / " + t.ToString();
    }

    public void SetProgressSlider(float p){
        progressSlider.value = p;
    }
    
    public void SetTrialPhaseDetail(string s){
        trialPhaseDisplay.text = s;
    }

    public void SetPause(string p){
        pauseDisplay.text = p;
    }
    
    public void SetStatus(GameStatus s, string t) {
        statusDisplay.text = t;
        statusDisplay_Wrld.text = t;
        switch (s) {
            case GameStatus.Orientation:
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.Preparation:
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.Ready :
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.Countdown :
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.RunningTrials :
                statusDisplay.color = completeColour;
                statusDisplay_Wrld.color = completeColour;
                break;
            case GameStatus.Complete :
                statusDisplay.color = completeColour;
                statusDisplay_Wrld.color = completeColour;
                break;
        }
    }
}