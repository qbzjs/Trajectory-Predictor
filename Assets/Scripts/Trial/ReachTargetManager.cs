using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTargetManager : MonoBehaviour
{

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

    void Awake()
    {
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


    void Update()
    {
        
    }
}
