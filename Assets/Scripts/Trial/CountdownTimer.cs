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
    public TextMeshPro countdownDisplay;

    public bool timerEnabled = false;
    public float timer;
    public int timerDisplay;

    void Awake()
    {
        instance = this;
        countdownDisplay.text = "";
    }

    public void SetCountdown(int t)
    {
        timer = t;
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
                countdownDisplay.text = timerDisplay.ToString();
            }
            if (timerDisplay == 0)
            {
                countdownDisplay.text = "";
            }
            if (timer <= 0)
            {
                countdownDisplay.text = "";
                TrialManager.instance.StartBlock();
                timerEnabled = false;
                timer = 0;
                //done
            }
        }
    }
}
