using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCI_ControlMixer {
    
    private float p; //predicted
    private float t; //target
    private float d; //displayed
    private float a; //assistance
    
    private float displayed;

    public float DiscriminateAxis(float predicted, float target, float assistance){
	    d = p + (t - p) * a;
	    displayed = d;
	    return displayed;
    }
}
