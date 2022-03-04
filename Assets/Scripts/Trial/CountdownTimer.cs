using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer instance; 

    public int initialWaitPeriod;
    public int countdown = 10;
    public TextMeshPro[] countdownDisplayWorld = new TextMeshPro[0];
    public TextMeshProUGUI[] countdownDisplayUI = new TextMeshProUGUI[0];

    public bool timerEnabled = false;
    public float timer;
    public int maxTimerDisplay = 61; //the number where the countdown timer renders on screen
    public int timerDisplay;
    public bool visibleDisplay;
    public bool timeUp;

    void Awake()
    {
        instance = this;
        for (int i = 0; i < countdownDisplayWorld.Length; i++) {
            countdownDisplayWorld[i].text = "";
        }
        for (int i = 0; i < countdownDisplayUI.Length; i++) {
            countdownDisplayUI[i].text = "";
        }
    }

    public void SetCountdown(int t, int maxCountdownDisplay)
    {
        timer = t;
        maxTimerDisplay = maxCountdownDisplay;
        visibleDisplay = false;
        timeUp = false;
        timerEnabled = true;
    }
    void Update()
    {
        if (timerEnabled)
            
        {
            if (GameManager.instance.paused == false){
                timer -= Time.deltaTime * GetGameSpeed();
            }
            
            if (timer <= maxTimerDisplay && timer >= 0) //only show countdown from max of set value
            {
                visibleDisplay = true;//callback
                
                timerDisplay = (int)timer % maxTimerDisplay;
                for (int i = 0; i < countdownDisplayWorld.Length; i++)
                {
                    countdownDisplayWorld[i].text = timerDisplay.ToString();
                }
                for (int i = 0; i < countdownDisplayUI.Length; i++)
                {
                    countdownDisplayUI[i].text = timerDisplay.ToString();
                }
            }
            if (timerDisplay == 0)
            {
                for (int i = 0; i < countdownDisplayWorld.Length; i++)
                {
                    countdownDisplayWorld[i].text = "";
                }
                for (int i = 0; i < countdownDisplayUI.Length; i++)
                {
                    countdownDisplayUI[i].text = "";
                }
            }
            if (timer <= 0)
            {
                for (int i = 0; i < countdownDisplayWorld.Length; i++)
                {
                    countdownDisplayWorld[i].text = "";
                }
                for (int i = 0; i < countdownDisplayUI.Length; i++)
                {
                    countdownDisplayUI[i].text = "";
                }
                
                timerEnabled = false;
                timer = 0;
                timeUp = true;
                //done - 
            }
        }
    }

    public void StopTimer(){
        timerEnabled = false;
        timer = 0;
        timeUp = true;
    }
    public bool TimerComplete(){
        return timeUp;
    }

    private int GetGameSpeed(){
        int s = Mathf.RoundToInt(GameManager.instance.gameSpeed);
        if (s == 0){
            s = 1;
        }
        return s;
    }
}
