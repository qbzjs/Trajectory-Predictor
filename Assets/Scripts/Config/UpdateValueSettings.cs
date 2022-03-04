using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using TMPro;

public class UpdateValueSettings : MonoBehaviour
{
    public SessionMetric sessionMetric;
    
    public TextMeshProUGUI numberText;

    public string valueTypeString = "";
    
    private Color defaultColour;

    private void OnEnable(){
        Settings.OnSettingsUpdate += UpdateValue;
    }
    private void OnDisable(){
        Settings.OnSettingsUpdate -= UpdateValue;
    }

    void Start()
    {
        defaultColour = numberText.color;
        UpdateValue();
    }

    private void UpdateValue(){
        if (sessionMetric == SessionMetric.TrialsPerBlock){
            SetValueDisplay(Settings.instance.trialsPerBlock);
        }
        if (sessionMetric == SessionMetric.TrialsPerRun){
            SetValueDisplay(Settings.instance.trialsPerRun);
        }
        if (sessionMetric == SessionMetric.TrialsPerSession){
            SetValueDisplay(Settings.instance.trialsPerSession);
        }

        if (sessionMetric == SessionMetric.EstimatedTrialDuration){
            SetValueDisplay(Settings.instance.estTrialDuration);
        }
        if (sessionMetric == SessionMetric.EstimatedBlockDuration){
            SetValueDisplay(Settings.instance.estBlockDuration);
        }
        if (sessionMetric == SessionMetric.EstimatedRunDuration){
            SetValueDisplay(Settings.instance.estRunDuration);
        }
        if (sessionMetric == SessionMetric.EstimatedSessionDuration){
            SetValueDisplay(Settings.instance.estSessionDuration);
        }
    }
    
    private void SetValueDisplay(int v) {
        numberText.text = v.ToString() + "<size=-2>" + valueTypeString;
        numberText.color = Settings.instance.highlightColour;
        StartCoroutine(DefaultColour());
    }
    private void SetValueDisplay(float v) {
        
        int minutes = (int)v / 60;
        int seconds = (int)v % 60;
        numberText.text = minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString() + "<size=-10>" + valueTypeString;;
        
        if (sessionMetric == SessionMetric.EstimatedTrialDuration){
            numberText.text = v.ToString("F2") + "<size=-2>" + valueTypeString;
        }
        
        numberText.color = Settings.instance.highlightColour;
        StartCoroutine(DefaultColour());
    }
    private IEnumerator DefaultColour()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        numberText.color = defaultColour;
    }
}
