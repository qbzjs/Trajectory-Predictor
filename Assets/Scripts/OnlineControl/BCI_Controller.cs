using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class BCI_Controller : MonoBehaviour
{

    public bool controlActive;
    
    private GameObject controlObject;
    private Rigidbody rb; //object moved by the BCI - arm will take position from this object
    
    public BCI_ControlType controlType;
    
    public bool useGravity;
    public bool freeze;
    [Space(4)] [Range(0, 10f)] public float mass = 1;
    [Range(0, 10f)] public float drag = 0;
    
    [Range(0, 0.5f)] public float motionThreshold = 0.1f;
    
    void Start()
    {
        controlObject = BCI_ControlManager.instance.controlObject;
        
        if (controlObject.GetComponent<Rigidbody>()){
            rb = controlObject.GetComponent<Rigidbody>();
        }
        else{
            Debug.Log("CONTROL OBJECT MISSING RIGIDBODY");
        }
    }
    void Update()
    {
        //rigidbody object settings
        rb.useGravity = useGravity;
        rb.isKinematic = freeze;
        rb.mass = mass;
        rb.drag = drag;
    }
}
