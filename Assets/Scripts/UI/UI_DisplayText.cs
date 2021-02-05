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
    public TextMeshProUGUI progressDisplay;
    public TextMeshProUGUI scoreDisplay;

    public Color defaultColour = Color.white;
    public Color completeColour = Color.green;

    private void Awake() {
        instance = this;
    }

    void Start() {
        scoreDisplay.text = "Score : n/a";
        progressDisplay.text = "Trial 0/0";
        statusDisplay.text = "Game Ready";
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

    public void SetProgress(int c, int t) {
        progressDisplay.text = "Trial : " + c.ToString() + " / " + t.ToString();
    }

    public void SetStatus(GameStatus s, string t) {
        statusDisplay.text = t;
        switch (s) {
            case GameStatus.Ready :
                statusDisplay.color = completeColour;
                break;
            case GameStatus.Running :
                statusDisplay.color = defaultColour;
                break;
            case GameStatus.Complete :
                statusDisplay.color = completeColour;
                break;
        }
    }
}