using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 targetRotation;
    public Vector3 rotationOffset;
    void Awake(){
        //rotationTarget = this.transform;
    }
    
    void Update(){
        //this.transform.rotation = rotationTarget.rotation;

        targetRotation = followTarget.eulerAngles;
        Vector3 tr = new Vector3(targetRotation.x + rotationOffset.x, targetRotation.y + rotationOffset.y, targetRotation.z + rotationOffset.z);
        this.transform.eulerAngles = tr;
    }
}
