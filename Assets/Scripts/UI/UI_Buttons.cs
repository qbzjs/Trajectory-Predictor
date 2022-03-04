using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Buttons : MonoBehaviour
{

    public GameObject objectPools;
    private UI_DisplayObjects displayObjects;
    
    public bool settingsActive;
    public bool scoreActive;
    public bool statsActive;

    void Awake()
    {
        displayObjects = objectPools.GetComponent<UI_DisplayObjects>();
    }

    private void Start(){
        settingsActive = true;
        scoreActive = true;
        statsActive = true;
        ToggleSettings(); 
        ToggleScore(); 
        ToggleStats(); 
    }

    public void ToggleSettings() {
        if (settingsActive) {
            displayObjects.settingsPanel.SetActive(false);
            settingsActive = false;
        }
        else {
            displayObjects.settingsPanel.SetActive(true);
            settingsActive = true;
        }
        Settings.instance.active = settingsActive;
    }
    public void ToggleScore() {
        if (scoreActive) {
            displayObjects.scorePanel.SetActive(false);
            scoreActive = false;
        }
        else {
            displayObjects.scorePanel.SetActive(true);
            scoreActive = true;
        }
    }
    public void ToggleStats() {
        if (statsActive) {
            displayObjects.statsPanel.SetActive(false);
            statsActive = false;
        }
        else {
            displayObjects.statsPanel.SetActive(true);
            statsActive = true;
        }
    }
}
