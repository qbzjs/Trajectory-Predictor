using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

public class ArmRig_IK : MonoBehaviour{
    
    public bool applyArmIK = true;
    
    [Header("IK References")]
    public Transform leftArmParentReference;
    public Transform rightArmParentReference;
    
    public Transform leftShoulderReference;
    public Transform leftElbowReference;
    public Transform leftHandReference;
    public Transform rightShoulderReference;
    public Transform rightElbowReference;
    public Transform rightHandReference;
    
    [Header("Arm Rigs")]
    public Transform leftShoulder;
    public Transform leftElbow;
    public Transform leftHand;
    public Transform rightShoulder;
    public Transform rightElbow;
    public Transform rightHand;
     
    void Start()
    {
        //get the reference transforms in the hierarchy
        leftShoulderReference = leftArmParentReference.transform.Find("LeftArm(Clone)/UpperArm").GetComponent<Transform>();
        leftElbowReference = leftArmParentReference.transform.Find("LeftArm(Clone)/UpperArm/LowerArm").GetComponent<Transform>();
        leftHandReference = leftArmParentReference.transform.Find("LeftArm(Clone)/UpperArm/LowerArm/Hand").GetComponent<Transform>();
        rightShoulderReference = rightArmParentReference.transform.Find("RightArm(Clone)/UpperArm").GetComponent<Transform>();
        rightElbowReference = rightArmParentReference.transform.Find("RightArm(Clone)/UpperArm/LowerArm").GetComponent<Transform>();
        rightHandReference = rightArmParentReference.transform.Find("RightArm(Clone)/UpperArm/LowerArm/Hand").GetComponent<Transform>();
    }

    void Update()
    {
        if (applyArmIK){
            ApplyArmReferences();
        }
    }
    //apply the reference transforms to an arm rig
    private void ApplyArmReferences(){
        leftShoulder.position = new Vector3(leftShoulderReference.position.x, leftShoulderReference.position.y, leftShoulderReference.position.z);
        leftShoulder.rotation = new Quaternion(leftShoulderReference.rotation.x, leftShoulderReference.rotation.y, leftShoulderReference.rotation.z,leftShoulderReference.rotation.w);
        
        leftElbow.position = new Vector3(leftElbowReference.position.x, leftElbowReference.position.y, leftElbowReference.position.z);
        leftElbow.rotation = new Quaternion(leftElbowReference.rotation.x, leftElbowReference.rotation.y, leftElbowReference.rotation.z,leftElbowReference.rotation.w);
        
        leftHand.position = new Vector3(leftHandReference.position.x, leftHandReference.position.y, leftHandReference.position.z);
        leftHand.rotation = new Quaternion(leftHandReference.rotation.x, leftHandReference.rotation.y, leftHandReference.rotation.z,leftHandReference.rotation.w);
        
       rightShoulder.position = new Vector3(rightShoulderReference.position.x, rightShoulderReference.position.y, rightShoulderReference.position.z);
        leftShoulder.rotation = new Quaternion(rightShoulderReference.rotation.x, rightShoulderReference.rotation.y, rightShoulderReference.rotation.z,rightShoulderReference.rotation.w);
        
        rightElbow.position = new Vector3(rightElbowReference.position.x, rightElbowReference.position.y, leftElbowReference.position.z);
        rightElbow.rotation = new Quaternion(rightElbowReference.rotation.x, rightElbowReference.rotation.y, rightElbowReference.rotation.z,rightElbowReference.rotation.w);
        
        rightHand.position = new Vector3(rightHandReference.position.x, rightHandReference.position.y, rightHandReference.position.z);
        rightHand.rotation = new Quaternion(rightHandReference.rotation.x, rightHandReference.rotation.y, rightHandReference.rotation.z,rightHandReference.rotation.w);
    }
    

}
