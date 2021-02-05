using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

[System.Serializable]
public class VR_Map
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    } 
}

public class VR_Rig : MonoBehaviour
{
    public bool active = true;
    [Range(1f,2f)]
    public float rigHeight = 1.8f;
    private float defaultRigHeight = 1.6f;
    public bool turnSmoothing = true;
    [Range(0, 10)]
    public float turnSmoothingSpeed = 5f;

    public VR_Map head;
    public VR_Map leftHand;
    public VR_Map rightHand;

    public Transform headConstraint;
    public Vector3 headBodyOffset;
    
    void Awake()
    {
        ApplyRigHeight();
    }
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    void FixedUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;

        if (turnSmoothing)
        {
            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothingSpeed);
        }
        else
        {
            transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;
        }     

        if (active)
        {
            head.Map();
            leftHand.Map();
            rightHand.Map();
        }
    }
    void ApplyRigHeight()
    {
        float offSet = rigHeight - defaultRigHeight;
        transform.localScale = new Vector3(transform.localScale.x + offSet, transform.localScale.x + offSet, transform.localScale.x + offSet);
    }
}
