using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

/// <summary>
/// moves the ghost rig hands to the highlighted target
/// </summary>

public class GhostTargetController : MonoBehaviour
{
    public VR_RigTargets rigTargets;
    [Space(10)]
    public Transform ghostTargetLeft;
    public Transform ghostTargetRight;

    public Transform leftHandParent;
    public Vector3 lp;
    public Transform rightHandParent;
    public Vector3 rp;

    public Vector3 leftHandPosition;
    public Vector3 rightHandPosition;
    public Vector3 lOffset;
    public Vector3 rOffset;

    public Transform currentTarget;

    void Start()
    {
        rigTargets = gameObject.GetComponent<VR_RigTargets>();
        //get hand targets
        ghostTargetLeft = rigTargets.leftHandTarget;
        ghostTargetRight = rigTargets.rightHandTarget;


        leftHandParent = rigTargets.VR_ControllerLeft;
        rightHandParent = rigTargets.VR_ControllerRight;
        lOffset = rigTargets.leftHandTargetOffset;
        rOffset = rigTargets.rightHandTargetOffset;
    }
    private void OnEnable()
    {
       // ReachTargetManager.OnTargetRestAction += 
    }
    void Update()
    {
        lp = leftHandParent.transform.position;
        rp = rightHandParent.transform.position;

        //positions to reset the ghost limb
        leftHandPosition = new Vector3(lp.x + lOffset.x, lp.y + lOffset.y, lp.z + lOffset.z);
        rightHandPosition = new Vector3(rp.x + rOffset.x, rp.y + rOffset.y, rp.z + rOffset.z);

    }
    void FixedUpdate()
    {

    }
}