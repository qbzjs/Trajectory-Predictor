using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationColours : MonoBehaviour
{
    public static ApplicationColours instance;

    public Color coreColour; //UI - orange/blue
    public Color noColor = Color.white;
    public Color disabledColour; //light grey
    public Color highlightUI; // core but brighter
    public Color actualColour; // actual movement colour
    public Color imaginedColor; // imagined movement colour
    public Color targetColour; // target default colour
    public Color targetHighlightColor; // target default colour to touch
    public Color feedbackColour; // pink - feedback
    
    void Awake(){
        instance = this;
    }
}
