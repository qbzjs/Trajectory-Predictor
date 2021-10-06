using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;

public class SetNumberSettings : MonoBehaviour {

    public TrialSettingsValue trialSettingValue;
    
    public TextMeshProUGUI numberText;
    
    public int min = 0;
    public int max = 10;

    private string valueDisplay = "";
    
    private Color defaultColour;

    private void Start() {
        defaultColour = numberText.color;
        Initialise();
    }

    private void Initialise() {
        if (trialSettingValue == TrialSettingsValue.TrialBlocks) {
            SetNumber(Settings.instance.trialBlocks);
        }
        if (trialSettingValue == TrialSettingsValue.Repetitions) {
            valueDisplay = "<size=-10>x";
            SetNumber(Settings.instance.repetitions);
        }
        if (trialSettingValue == TrialSettingsValue.StartDelay) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.startDelay);
            
        }
        if (trialSettingValue == TrialSettingsValue.InterBlockRestPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.interBlockRestPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMin) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.restDurationMin);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMax) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.restDurationMax);
        }
        // if (trialSettingValue == TrialSettingsValue.TargetDuration) {
        //     valueDisplay = "<size=-10>s";
        //     SetNumber(Settings.instance.targetDuration);
        // }
    }
    private void SetNumber(int n) {
        numberText.text = n.ToString() + valueDisplay;
    }

    public void PlusNumber() {
        string t = numberText.text;
        t = t.Replace("<size=-10>", "");
        t = t.Replace("s", "");
        t = t.Replace("x", "");
        int n = int.Parse(t);
        n++;
        if (n > max)
        {
            n = max;
        }     
        numberText.text = n.ToString() + valueDisplay;
        numberText.color = Settings.instance.highlightColour;
        SetValue(n);
        StartCoroutine(DefaultColour());
    }

    public void MinusNumber() {
        string t = numberText.text;
        t = t.Replace("<size=-10>", "");
        t = t.Replace("s", "");
        t = t.Replace("x", "");
        int n = int.Parse(t);
        n--;
        if (n < min)
        {
            n = min;
        }     
        numberText.text = n.ToString() + valueDisplay;
        numberText.color = Settings.instance.highlightColour;
        SetValue(n);
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

    private void SetValue(int v) {
        if (trialSettingValue == TrialSettingsValue.TrialBlocks) {
            Settings.instance.SetTrialBlocks(v);
        }
        if (trialSettingValue == TrialSettingsValue.Repetitions) {
            valueDisplay = "<size=-10>x";
            Settings.instance.SetRepetitions(v);
        }
        if (trialSettingValue == TrialSettingsValue.StartDelay) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetStartDelay(v);
        }
        if (trialSettingValue == TrialSettingsValue.InterBlockRestPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetInterBlockRestPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMin) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetRestDurationMin(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMax) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetRestDurationMax(v);
        }
        // if (trialSettingValue == TrialSettingsValue.TargetDuration) {
        //     valueDisplay = "<size=-10>s";
        //     Settings.instance.SetTargetDuration(v);
        // }
    }
}
