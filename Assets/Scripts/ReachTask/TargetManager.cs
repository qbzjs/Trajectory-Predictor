using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    
    
    public Transform originPoint;
    [Range(0.05f,0.25f)]
    public float targetDistance;

    private TargetController controller;
    
    void Start()
    {
        controller = gameObject.GetComponent<TargetController>();
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("target centre");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("target left");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("target top");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("target right");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("target bottom");
        }
    }

    public void SetTarget(int tNum)
    {
        
    }
}
