using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCI_ControlMixer {
    
    private float p; //predicted
    private float t; //target
    private float cx; //control
    private float cy; //control
    private float cz; //control
    private float a; //assistance
    
    private float controlAxis;
    private Vector3 controlVector;

    public float AssistControl(float predicted, float target, float assistance){
	    
	    controlAxis = predicted + (target - predicted) * assistance;
	    return controlAxis;
    }

    public Vector3 AssistControl(Vector3 predicted, Vector3 target, float assistance){
	    a = assistance;
	    
	    p = predicted.x;
	    t = target.x;
	    cx = p + (t - p) * a;
	    
	    p = predicted.y;
	    t = target.y;
	    cy = p + (t - p) * a;
	    
	    p = predicted.z;
	    t = target.z;
	    cz = p + (t - p) * a;
	    
	    controlVector = new Vector3(cx, cy, cz);
	    return controlVector;
    }
}
