using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistTest : MonoBehaviour
{
    public float predicted = 1.0f; //predicted
    public float target = 0.8f; //target
    public float assistance = 1.0f; //assistance

    public float assistValue;


    void Update(){
        AssistValue();
    }
    void AssistValue(){
        assistValue = predicted + (target - predicted) * assistance;
    }
}
