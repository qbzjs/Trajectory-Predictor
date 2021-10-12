using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSpeedSettings : MonoBehaviour
{
    public Slider speedSlider;
    public float speedValue;
    
    void Start()
    {
        
    }
    
    public void SetSpeed(){
        speedValue = speedSlider.value;
        Settings.instance.SetSpeedModifier(speedValue);
        SetSpeedEnd();
    }

    public void SetSpeedEnd(){
        speedValue = 0;
        Settings.instance.SetSpeedModifier(speedValue);
    }
}
