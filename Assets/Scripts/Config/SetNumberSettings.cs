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
            SetNumber(Settings.instance.repetitions);
        }
        if (trialSettingValue == TrialSettingsValue.StartDelay) {
            SetNumber(Settings.instance.startDelay);
        }
        if (trialSettingValue == TrialSettingsValue.InterBlockRestPeriod) {
            SetNumber(Settings.instance.interBlockRestPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMin) {
            SetNumber(Settings.instance.restDurationMin);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMax) {
            SetNumber(Settings.instance.restDurationMax);
        }
        if (trialSettingValue == TrialSettingsValue.TargetDuration) {
            SetNumber(Settings.instance.targetDuration);
        }
    }
    private void SetNumber(int n) {
        numberText.text = n.ToString();
    }

    public void PlusNumber() {
        int n = int.Parse(numberText.text);
        n++;
        if (n > max)
        {
            n = max;
        }     
        numberText.text = n.ToString();
        numberText.color = Settings.instance.highlightColour;
        SetValue(n);
        StartCoroutine(DefaultColour());
    }

    public void MinusNumber() {
        int n = int.Parse(numberText.text);
        n--;
        if (n < min)
        {
            n = min;
        }     
        numberText.text = n.ToString();
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
            Settings.instance.SetRepetitions(v);
        }
        if (trialSettingValue == TrialSettingsValue.StartDelay) {
            Settings.instance.SetStartDelay(v);
        }
        if (trialSettingValue == TrialSettingsValue.InterBlockRestPeriod) {
            Settings.instance.SetInterBlockRestPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMin) {
            Settings.instance.SetRestDurationMin(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestDurationMax) {
            Settings.instance.SetRestDurationMax(v);
        }
        if (trialSettingValue == TrialSettingsValue.TargetDuration) {
            Settings.instance.SetTargetDuration(v);
        }
    }
}
