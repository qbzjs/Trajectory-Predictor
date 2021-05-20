using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using RootMotion.FinalIK;

public class VR_RigTargets : MonoBehaviour
{
    public RigType rigType = RigType.AvatarRig;

    public Transform avatar;
    public VRIK vrIK;
    public float height = 1f;
    public float armLength = 1.2f;

    [Space(10)]
    public Transform VR_Camera;
    public Transform VR_ControllerLeft;
    public Transform VR_ControllerRight;
    public bool useViveTrackers = false;
    public Transform ViveTrackerLeftHand;
    public Transform ViveTrackerRightHand;

    [Space(10)]
    //TODO add feet later 
    public Transform VR_FootTrackerLeft;
    public Transform VR_FootTrackerRight;


    public bool calibrate = false;
    
    [Space(10)]
    public Transform headTarget;
    public Vector3 headTargetOffset;
    public Vector3 headTargetRotation;
    [Space(10)]
    public Transform leftHandTarget;
    public Vector3 leftHandTargetOffset;
    public Vector3 leftHandTargetRotation;
    [Space(10)]
    public Transform rightHandTarget;
    public Vector3 rightHandTargetOffset;
    public Vector3 rightHandTargetRotation;
    [Space(10)]
    //TODO feet added later
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    
    void Awake()
    {
        BuildRig();
    }
    private void BuildRig()
    {
        if (useViveTrackers)
        {
            if (ViveTrackerLeftHand != null)
            {
                VR_ControllerLeft = ViveTrackerLeftHand;
            }
            if (ViveTrackerRightHand != null)
            {
                VR_ControllerRight = ViveTrackerRightHand;
            }
            
        }
        //parent target to rig
        headTarget.transform.parent = VR_Camera;
        leftHandTarget.transform.parent = VR_ControllerLeft;
        rightHandTarget.transform.parent = VR_ControllerRight;

        //apply positions and offsets
        headTarget.transform.position = new Vector3(VR_Camera.transform.position.x + headTargetOffset.x, VR_Camera.transform.position.y + headTargetOffset.y, VR_Camera.transform.position.z + headTargetOffset.z);
        leftHandTarget.transform.position = new Vector3(VR_ControllerLeft.transform.position.x + leftHandTargetOffset.x, VR_ControllerLeft.transform.position.y + leftHandTargetOffset.y, VR_ControllerLeft.transform.position.z + leftHandTargetOffset.z);
        rightHandTarget.transform.position = new Vector3(VR_ControllerRight.transform.position.x + rightHandTargetOffset.x, VR_ControllerRight.transform.position.y + rightHandTargetOffset.y, VR_ControllerRight.transform.position.z + rightHandTargetOffset.z);
        
        //correct rotations
        headTarget.eulerAngles = headTargetRotation;
        leftHandTarget.eulerAngles = leftHandTargetRotation;
        rightHandTarget.eulerAngles = rightHandTargetRotation;
        
        
        

        //        float rigScaler = height
        //        avatar.localScale = new Vector3(avatar.localScale.x + height, avatar.localScale.y + height, avatar.localScale.z + height);

        //set rig targets
        vrIK = avatar.GetComponent<VRIK>();
        vrIK.solver.leftArm.armLengthMlp = armLength;
        vrIK.solver.rightArm.armLengthMlp = armLength;
        vrIK.solver.spine.headTarget = headTarget;
        vrIK.solver.leftArm.target = leftHandTarget;
        vrIK.solver.rightArm.target = rightHandTarget;
    }
    
}
