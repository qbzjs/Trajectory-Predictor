using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;

public class SetNumberSettings : MonoBehaviour {

    public SettingsValue settingValue;
    
    public TextMeshProUGUI numberText;
    public int min = 0;
    public int max = 10;

    private Color defaultColour;

    private void Start() {
        defaultColour = numberText.color;
        Initialise();
    }

    private void Initialise() {
        if (settingValue == SettingsValue.Repetitions) {
            SetNumber(Settings.instance.repetitions);
        }
        if (settingValue == SettingsValue.StartDelay) {
            SetNumber(Settings.instance.startDelay);
        }
        if (settingValue == SettingsValue.RestDuration) {
            SetNumber(Settings.instance.restDuration);
        }
        if (settingValue == SettingsValue.TargetDuration) {
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
        if (n < 0)
        {
            n = 0;
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
        if (settingValue == SettingsValue.Repetitions) {
            Settings.instance.SetRepetitions(v);
        }
        if (settingValue == SettingsValue.StartDelay) {
            Settings.instance.SetStartDelay(v);
        }
        if (settingValue == SettingsValue.RestDuration) {
            Settings.instance.SetRestDuration(v);
        }
        if (settingValue == SettingsValue.TargetDuration) {
            Settings.instance.SetTargetDuration(v);
        }
    }
}
