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
        if (trialSettingValue == TrialSettingsValue.TargetDuration) {
            valueDisplay = "<size=-10>s";
            SetNumber(Settings.instance.targetDurationGranular);
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
        n = n + 0.1f;
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
        n = n - 0.1f;
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
        if (trialSettingValue == TrialSettingsValue.TargetDuration) {
            valueDisplay = "<size=-10>s";
            Settings.instance.SetTargetDurationGranular(v);
        }
    }
}
