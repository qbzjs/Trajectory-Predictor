using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
    
public class ReachTargetManager : MonoBehaviour{
    
    private GameObject reachObject;
    [Range(0, 1.5f)]
    public float rightLeftOffset = 0.5f;
    private Vector3 positionLeft;
    private Vector3 positionRight;
        
    public GameObject[] reachTarget = new GameObject[5];

    public Material targetDefault;
    public Material targetHighlight;

    public bool animateTargets;

    private TaskSide taskSide = TaskSide.Left;

    //send general target action
    public delegate void TargetAction(Transform target, TaskSide side);
    public static event TargetAction OnTargetAction;
    //send to ghost target controller - move arm to target - target null move arm back
    public delegate void TargetRestAction(Transform target, TaskSide side);
    public static event TargetRestAction OnTargetRestAction;


    private void OnEnable()
    {
        TrialSequence.OnTargetAction += TrialSequence_OnTargetAction;
        TrialSequence.OnTargetRestAction += TrialSequence_OnTargetRestAction;
    }
    private void OnDisable()
    {
        TrialSequence.OnTargetAction -= TrialSequence_OnTargetAction;
        TrialSequence.OnTargetRestAction -= TrialSequence_OnTargetRestAction;
    }

    void Awake(){
        reachObject = gameObject;
        positionLeft = reachObject.transform.position;
        //float x = Mathf.Abs(reachObject.transform.position.x);
        rightLeftOffset = -rightLeftOffset;
        float x = reachObject.transform.position.x + rightLeftOffset;
        positionRight = new Vector3(x, reachObject.transform.position.y, reachObject.transform.position.z);
        
        for (int i = 0; i < reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }
    }

    private void TrialSequence_OnTargetAction(int targetNumber)
    {
        Debug.Log(targetNumber +" : " + reachTarget[targetNumber].name.ToString());

        for(int i=0; i<reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }

        reachTarget[targetNumber].GetComponent<Renderer>().material = targetHighlight;

        if (animateTargets)
        {
            reachTarget[targetNumber].GetComponent<TargetAnimator>().ScaleTarget();
        }

        if (OnTargetAction != null)
        {
            OnTargetAction(reachTarget[targetNumber].transform, taskSide);
        }
    }
    private void TrialSequence_OnTargetRestAction(int targetNumber)
    {
        Debug.Log(targetNumber + " : " + reachTarget[targetNumber].name.ToString());

        for (int i = 0; i < reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }

        if (OnTargetRestAction != null)
        {
            OnTargetRestAction(reachTarget[targetNumber].transform, taskSide);
        }
    }

    public void SetReachSide(TaskSide side)
    {
        taskSide = side;
        if (side == TaskSide.Left){
            reachObject.transform.position = positionLeft;
        }

        if (side == TaskSide.Right){
            reachObject.transform.position = positionRight;
        }
    }
}
