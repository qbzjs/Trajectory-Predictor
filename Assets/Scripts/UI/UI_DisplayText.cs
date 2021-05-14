using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;

public class UI_DisplayText : MonoBehaviour {
    public static UI_DisplayText instance;

    public TextMeshProUGUI statusDisplay;
    public TextMeshPro statusDisplay_Wrld;
    public TextMeshProUGUI movementProgressDisplay;
    public TextMeshPro movementProgressDisplay_Wrld;
    public TextMeshProUGUI trialProgressDisplay;
    public TextMeshPro trialProgressDisplay_Wrld;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshPro scoreDisplay_Wrld;

    public Color defaultColour = Color.white;
    public Color completeColour = Color.green;

    private void Awake() {
        instance = this;
    }

    void Start() {
        scoreDisplay.text = "Score : n/a";
        scoreDisplay_Wrld.text = "Score : n/a";
        movementProgressDisplay.text = "Movement 0/0";
        movementProgressDisplay_Wrld.text = "Movement 0/0";
        trialProgressDisplay.text = "Trial 0/0";
        trialProgressDisplay_Wrld.text = "Trial 0/0";
        SetStatus(GameStatus.Ready, "System Ready");
    }

//    public void SetStatus(GameStatus s) {
//        string dis = "";
//        switch (s) {
//            case GameStatus.Ready :
//                dis = "Trail Ready";
//                break;
//            case GameStatus.Running :
//                dis = "Trail Running";
//                break;
//            case GameStatus.Complete :
//                dis = "Trail Complete";
//                break;
//        }
//        statusDisplay.text = dis;
//    }

    public void SetProgressMovement(int c, int t) {
        movementProgressDisplay.text = "Movement : " + c.ToString() + " / " + t.ToString();
        movementProgressDisplay_Wrld.text = "Movement : " + c.ToString() + " / " + t.ToString();
    }
    public void SetProgressTrial(int c, int t) {
        trialProgressDisplay.text = "Trail : " + c.ToString() + " / " + t.ToString();
        trialProgressDisplay_Wrld.text = "Trail : " + c.ToString() + " / " + t.ToString();
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
                statusDisplay.color = completeColour;
                statusDisplay_Wrld.color = completeColour;
                break;
            case GameStatus.Countdown :
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.Running :
                statusDisplay.color = defaultColour;
                statusDisplay_Wrld.color = defaultColour;
                break;
            case GameStatus.Complete :
                statusDisplay.color = completeColour;
                statusDisplay_Wrld.color = completeColour;
                break;
        }
    }
}