using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCI_ControlMixer {
    
    private float p; //predicted
    private float t; //target
    private float a; //assistance
    
    private float ax; //control
    private float ay; //control
    private float az; //control
    
    private float assistedAxis;
    private Vector3 assistedVector;

    public float AssistControl(float predicted, float target, float assistance){
	    assistedAxis = predicted + (target - predicted) * assistance;
	    return assistedAxis;
    }

    public Vector3 AssistControl(Vector3 predicted, Vector3 target, float assistance){
	    a = assistance;
	    
	    p = predicted.x;
	    t = target.x;
	    ax = p + (t - p) * a;
	    
	    p = predicted.y;
	    t = target.y;
	    ay = p + (t - p) * a;
	    
	    p = predicted.z;
	    t = target.z;
	    az = p + (t - p) * a;
	    
	    assistedVector = new Vector3(ax, ay, az);
	    return assistedVector;
    }
}
