using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SetControlSliderSettings : MonoBehaviour
{
    public SliderValue sliderValue;
    public Slider slider;
    public float value;

    private void Awake(){
        slider = gameObject.GetComponent<Slider>();
    }

    private void Start(){
        Initialise();
    }

    private void Initialise(){
        if (sliderValue == SliderValue.Assistance){
            slider.value = Settings.instance.BCI_ControlAssistance;
        }
        if (sliderValue == SliderValue.AssistanceDecrease){
            slider.value = Settings.instance.assistanceDecrease;
        }
        if (sliderValue == SliderValue.ControlMagnitude){
            slider.value = Settings.instance.controlMagnitude;
        }
        if (sliderValue == SliderValue.MagnitudeX){
            slider.value = Settings.instance.controlMagnitudeX;
        }
        if (sliderValue == SliderValue.MagnitudeY){
            slider.value = Settings.instance.controlMagnitudeY;
        }
        if (sliderValue == SliderValue.MagnitudeZ){
            slider.value = Settings.instance.controlMagnitudeZ;
        }
        if (sliderValue == SliderValue.SmoothingSpeed){
            slider.value = Settings.instance.smoothingSpeed;
        }
    }

    // private void SetSlider(){
    //     slider.value = value;
    // }
    
    public void SetValue(){
        value = slider.value;
        if (sliderValue == SliderValue.Assistance){
            Settings.instance.SetAssistance(value);
        }
        if (sliderValue == SliderValue.AssistanceDecrease){
            Settings.instance.SetAssistanceDecrease(value);
        }
        if (sliderValue == SliderValue.ControlMagnitude){
            Settings.instance.SetMagnitude(value);
        }
        if (sliderValue == SliderValue.MagnitudeX){
            Settings.instance.SetMagnitudeX(value);
        }
        if (sliderValue == SliderValue.MagnitudeY){
            Settings.instance.SetMagnitudeY(value);
        }
        if (sliderValue == SliderValue.MagnitudeZ){
            Settings.instance.SetMagnitudeZ(value);
        }
        if (sliderValue == SliderValue.SmoothingSpeed){
            Settings.instance.SetSmoothingSpeed(value);
        }
    }
}
