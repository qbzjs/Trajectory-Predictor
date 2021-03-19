using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerLimitExtension 
{
    
    public Vector3 NormaliseAngle(Vector3 r) {
        return new Vector3(CheckAngle(r.x),CheckAngle(r.y),CheckAngle(r.z)); 
    }
    private float CheckAngle(float value) {
        float angle = value - 180;
        if (angle > 0) {
            return angle - 180;
        }
        return angle + 180;
    }
}
