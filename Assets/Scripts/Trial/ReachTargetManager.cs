using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTargetManager : MonoBehaviour
{

    private void OnEnable()
    {
        TrialSequence.OnTargetAction += TrialSequence_OnTargetAction;
    }
    private void OnDisable()
    {
        TrialSequence.OnTargetAction -= TrialSequence_OnTargetAction;
    }

    private void TrialSequence_OnTargetAction(int targetNumber)
    {
        Debug.Log(targetNumber);
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
