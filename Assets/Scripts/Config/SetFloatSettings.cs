using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;

public class SetFloatSettings : MonoBehaviour
{
    public TrialSettingsValue trialSettingValue;

    public TextMeshProUGUI numberText;
   
    public float increment = 0.1f;
    public int min = 0;
    public int max = 10;

    private Color defaultColour;
    
    private string valueDisplay = "";
    
    void Start()
    {
        defaultColour = numberText.color;
        Initialise();
    }

    private void Initialise() {
        if (trialSettingValue == TrialSettingsValue.preTrialWaitPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.preTrialWaitPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.FixationPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.fixationPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.IndicationPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.indicationPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.ObservationPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.observationPeriod);
            SetNumber(Settings.instance.targetPresentationPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.TargetPresentationPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.targetPresentationPeriod);
            SetNumber(Settings.instance.observationPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.RestPeriodMin) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.restPeriodMin);
        }
        if (trialSettingValue == TrialSettingsValue.RestPeriodMax) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.restPeriodMax);
        }
        if (trialSettingValue == TrialSettingsValue.PostTrialWaitPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.postTrialWaitPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.PostBlockWaitPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.postBlockWaitPeriod);
        }
        if (trialSettingValue == TrialSettingsValue.PostRunWaitPeriod) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.postRunWaitPeriod);
        }
    }
    private void SetNumber(float n) {
        numberText.text = n.ToString("F1")+valueDisplay;
    }
    public void PlusNumber()
    {
        string t = numberText.text;
        t = t.Replace("<size=-10>", "");
        t = t.Replace("s", "");
        t = t.Replace("x", "");
        float n = float.Parse(t);
        n = n + increment;
        if (n > max)
        {
            n = max;
        }     
        numberText.text = n.ToString("F1")+valueDisplay;
        numberText.color = Settings.instance.highlightColour;
        SetValue(n);
        StartCoroutine(DefaultColour());
    }

    public void MinusNumber() {
        string t = numberText.text;
        t = t.Replace("<size=-10>", "");
        t = t.Replace("s", "");
        t = t.Replace("x", "");
        float n = float.Parse(t);
        n = n - increment;
        if (n < min)
        {
            n = min;
        }     
        numberText.text = n.ToString("F1") +valueDisplay;
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
    private void SetValue(float v) {
        if (trialSettingValue == TrialSettingsValue.preTrialWaitPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetPreTrialWaitPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.FixationPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetFixationPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.IndicationPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetIndicationPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.ObservationPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetObservationPeriod(v);
            Settings.instance.SetTargetPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.TargetPresentationPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetTargetPeriod(v);
            Settings.instance.SetObservationPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestPeriodMin) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetRestPeriodMin(v);
        }
        if (trialSettingValue == TrialSettingsValue.RestPeriodMax) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetRestPeriodMax(v);
        }
        if (trialSettingValue == TrialSettingsValue.PostTrialWaitPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetPostTrialWaitPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.PostBlockWaitPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetPostBlockWaitPeriod(v);
        }
        if (trialSettingValue == TrialSettingsValue.PostRunWaitPeriod) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetPostRunWaitPeriod(v);
        }
    }
}
