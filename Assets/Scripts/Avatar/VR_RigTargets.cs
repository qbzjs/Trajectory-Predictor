using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_RigTargets : MonoBehaviour
{
    public Transform VR_Camera;
    public Transform VR_ControllerLeft;
    public Transform VR_ControllerRight;

    //TODO add feet later 
    public Transform VR_FootTrackerLeft;
    public Transform VR_FootTrackerRight;

    public Transform headTarget;
    public Vector3 headTargetOffset;
    public Transform leftHandTarget;
    public Vector3 leftHandTargetOffset;
    public Transform rightHandTarget;
    public Vector3 rightHandTargetOffset;

    //TODO feet added later
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        headTarget.transform.position = new Vector3(VR_Camera.transform.position.x + headTargetOffset.x, VR_Camera.transform.position.y + headTargetOffset.y, VR_Camera.transform.position.z + headTargetOffset.z);
        headTarget.transform.rotation = VR_Camera.rotation;

 //       leftHandTarget.transform.position = new Vector3(VR_ControllerLeft.transform.position.x + leftHandTargetOffset.x, VR_ControllerLeft.transform.position.y + leftHandTargetOffset.y, VR_ControllerLeft.transform.position.z + leftHandTargetOffset.z);

 //       rightHandTarget.transform.position = new Vector3(VR_ControllerLeft.transform.position.x + rightHandTargetOffset.x, VR_ControllerLeft.transform.position.y + rightHandTargetOffset.y, VR_ControllerLeft.transform.position.z + rightHandTargetOffset.z);
    }
}
