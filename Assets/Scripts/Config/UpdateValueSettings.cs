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

    private string valueDisplay = "";
    
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
    }
    
    private void SetValueDisplay(int v) {
        numberText.text = v.ToString() + "<size=-10>" + valueDisplay;
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
