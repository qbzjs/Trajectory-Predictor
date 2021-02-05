using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetLineVisibility : MonoBehaviour {
    
    public Toggle outerToggle;
    public Toggle innerAToggle;
    public Toggle innerBToggle;
    public Toggle outerRingToggle;
    
    private Color initialColour;
    public Color highlightedColour;

    void Start() {
        initialColour = outerToggle.transform.GetChild(0).GetComponent<Image>().color;
        Initialise();
    }

    private void Initialise() {
        bool b = Settings.instance.linesOuter;
        outerToggle.isOn = b;
        if (outerToggle.isOn) {
            outerToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            outerToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        
        b = Settings.instance.linesInnerA;
        innerAToggle.isOn = b;
        if (innerAToggle.isOn) {
            innerAToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            innerAToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        
        b = Settings.instance.linesInnerB;
        innerBToggle.isOn = b;
        if (innerBToggle.isOn) {
            innerBToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            innerBToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        
        b = Settings.instance.ringOuter;
        outerRingToggle.isOn = b;
        if (outerRingToggle.isOn) {
            outerRingToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            outerRingToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
    }

    public void SetOuter() {
        if (outerToggle.isOn) {
            outerToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            outerToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        Settings.instance.SetLinesOuter(outerToggle.isOn);
    }
    public void SetInnerA() {
        if (innerAToggle.isOn) {
            innerAToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            innerAToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        Settings.instance.SetLinesInnerA(innerAToggle.isOn);
    }
    public void SetInnerB() {
        if (innerBToggle.isOn) {
            innerBToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            innerBToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        Settings.instance.SetLinesInnerB(innerBToggle.isOn);
    }
    public void SetOuterRing() {
        if (outerRingToggle.isOn) {
            outerRingToggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
        }
        else {
            outerRingToggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
        }
        Settings.instance.SetRingOuter(outerRingToggle.isOn);
    }
}