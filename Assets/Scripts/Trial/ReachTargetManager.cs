using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
    
public class ReachTargetManager : MonoBehaviour{
    
    private GameObject reachObject;
    private Vector3 positionLeft;
    private Vector3 positionRight;
        
    public GameObject[] reachTarget = new GameObject[5];

    public Material targetDefault;
    public Material targetHighlight;

    public bool animateTargets;

    private void OnEnable()
    {
        TrialSequence.OnTargetAction += TrialSequence_OnTargetAction;
    }
    private void OnDisable()
    {
        TrialSequence.OnTargetAction -= TrialSequence_OnTargetAction;
    }

    void Awake(){
        reachObject = gameObject;
        positionLeft = reachObject.transform.position;
        float x = Mathf.Abs(reachObject.transform.position.x);
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
    }


    public void SetReachSide(TaskSide side)
    {
        if (side == TaskSide.Left){
            reachObject.transform.position = positionLeft;
        }

        if (side == TaskSide.Right){
            reachObject.transform.position = positionRight;
        }
    }
}
