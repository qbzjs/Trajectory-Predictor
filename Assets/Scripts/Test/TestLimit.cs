using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLimit : MonoBehaviour{
    
    
    public float input=0;
    public float limit = 100;
    public float upper = 200;
    public float lower = 0;
    public float output = 0;
    
    void Start()
    {
        
    }

    
    void Update(){
        // output = Utilities.SetUpperLimit(input, limit);
        // output = Utilities.SetLowerLimit(input, -limit);
        
        // output = Utilities.SetLimitsPositiveNegative(input, limit);

        output = Utilities.SetUpperLowerLimit(input, upper, lower);
    }
}
