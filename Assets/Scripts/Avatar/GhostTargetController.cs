using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
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

    public Vector3 offset;

    public Vector3 currentTarget;
    public TaskSide side = TaskSide.Left;

    public float AO_Period;

    public float speed = 1;

    public float timer;
    public bool active;

    public Material ghostMaterial;
    public Material ghostMaterialOn;
    public Material ghostMaterialOff;
    private Material targetMat;
    public Color32 ghostDefaultColour;
    public Color32 ghostOffColour;

    void Start()
    {
        rigTargets = gameObject.GetComponent<VR_RigTargets>();
        //get hand targets
        ghostTargetLeft = rigTargets.leftHandTarget;
        ghostTargetRight = rigTargets.rightHandTarget;


        leftHandParent = rigTargets.VR_ControllerLeft.transform.Find("LeftHandTarget");
        rightHandParent = rigTargets.VR_ControllerRight.transform.Find("RightHandTarget"); 
        lOffset = rigTargets.leftHandTargetOffset;
        rOffset = rigTargets.rightHandTargetOffset;
    }
    private void OnEnable()
    {
        ReachTargetManager.OnTargetRestAction += ReachTargetManager_OnTargetRestAction;
        ReachTargetManager.OnTargetAction += ReachTargetManager_OnTargetAction;
    }
    private void OnDisable()
    {
        ReachTargetManager.OnTargetRestAction -= ReachTargetManager_OnTargetRestAction;
        ReachTargetManager.OnTargetAction -= ReachTargetManager_OnTargetAction;

        ghostMaterial = ghostMaterialOn;
    }
    private void ReachTargetManager_OnTargetRestAction(Transform tar, TaskSide s)
    {
        active = false;
        Vector3 t = new Vector3(tar.position.x+offset.x, tar.position.y + offset.y, tar.position.z + offset.z);
        currentTarget = t;
        side = s;
        timer = 0;
        active = true ;
    }
    private void ReachTargetManager_OnTargetAction(Transform tar, TaskSide s)
    {
        //currentTarget = tar;
        side = s;
        active = false ;
    }
    void Update()
    {
        AO_Period = Settings.instance.restDuration;

        lp = leftHandParent.transform.position;
        rp = rightHandParent.transform.position;

        //positions to reset the ghost limb
//        leftHandPosition = new Vector3(lp.x + lOffset.x, lp.y + lOffset.y, lp.z + lOffset.z);
//        rightHandPosition = new Vector3(rp.x + rOffset.x, rp.y + rOffset.y, rp.z + rOffset.z);

        leftHandPosition = new Vector3(lp.x, lp.y, lp.z);
        rightHandPosition = new Vector3(rp.x, rp.y, rp.z);


        timer += Time.deltaTime;
        if (timer >= AO_Period / 2 && active)
        {
//            if (side == TaskSide.Left)
//            {
//                currentTarget = leftHandPosition;
//            }
//            else
//            {
//                currentTarget = rightHandPosition;
//            }
            timer = 0;
            active = false;
        }

        if (active)
        {
            if (side == TaskSide.Left)
            {
                ghostTargetLeft.position = Vector3.Lerp(ghostTargetLeft.position, currentTarget, Time.deltaTime * speed);
            }
            else
            {
                ghostTargetRight.position = Vector3.Lerp(ghostTargetRight.position, currentTarget, Time.deltaTime * speed);
            }
            //ghostMaterial.SetColor("_Color", ghostDefaultColour);
            targetMat = ghostMaterialOn;

        }
        else
        {
            ghostTargetLeft.position = Vector3.Lerp(ghostTargetLeft.position, leftHandPosition, Time.deltaTime * speed);
            ghostTargetRight.position = Vector3.Lerp(ghostTargetRight.position, rightHandPosition, Time.deltaTime * speed);
            //ghostMaterial.SetColor("_Color", ghostOffColour);
            targetMat = ghostMaterialOff;
        }

        ghostMaterial.Lerp(ghostMaterial, targetMat, Time.deltaTime * (speed*2));
    }
    void FixedUpdate()
    {

    }
}