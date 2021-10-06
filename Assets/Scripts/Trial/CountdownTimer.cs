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
    public int timerDisplay;
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

    public void SetCountdown(int t)
    {
        timer = t;
        timeUp = false;
        timerEnabled = true;
    }
    void Update()
    {
        if (timerEnabled)
        {
            timer -= Time.deltaTime;
            if (timer <= countdown + 1 && timer >= 0)
            {
                timerDisplay = (int)timer % 60;
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
                BlockManager.instance.StartBlock();
                //??? generate event
                //done
            }
        }
    }

    public bool TimerComplete(){
        return timeUp;
    }
}
