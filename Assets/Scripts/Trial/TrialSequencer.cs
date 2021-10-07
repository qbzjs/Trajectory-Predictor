using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Random = UnityEngine.Random;

public class TrialSequencer : MonoBehaviour{
    
    public int targetCount = 4;
    public int repetitions = 4;
    public int sequenceLength = 0;
    public int sequenceIndex = 0;

    [Header("---------")]
    //public float timer;
    public float duration;
    
    [Header("---------")]
    public TrialEventType trialEventType;
    public float fixationDuration = 2f;
    public float arrowDuration = 2f;
    public float observationDuration = 2f;
    public float targetDuration = 2f;
    public float restDuration;
    public float restDurationMin = 2;
    public float restDurationMax;
    
    [Header("---------")]
    public bool sequenceComplete = false;
    
    public int[] sequenceOrder = new int[0];
    
    public delegate void TargetAction(int targetNumber, TrialEventType eType, float dur);
    public static event TargetAction OnTargetAction;
    
    void Start()
    {
        // InitialiseTrial();
    }


    void Update()
    {
        
    }
    public void StartTrialSequence(){
        InitialiseTrial();
        StartCoroutine(RunTrialSequence());
    }
    public void InitialiseTrial() {
        GenerateSequence();
        sequenceLength = sequenceOrder.Length;
    }
    public void GenerateSequence() {
        sequenceOrder = CSC_Math.IntArray_from_ElementRepeatNumbers(targetCount, repetitions);
        sequenceOrder = CSC_Math.RandPerm_intArray(sequenceOrder);
    }
    private IEnumerator RunTrialSequence(){

        for (int i = 0; i < sequenceLength; i++)
        {
            restDuration = GetRestDuration();
            duration = GetTotalDuration() + restDuration;
            
            trialEventType = TrialEventType.Fixation;
            Debug.Log("TARGET: " + sequenceOrder[i] + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + fixationDuration.ToString()); //send fixation event
            SendEvent(sequenceOrder[i], trialEventType, fixationDuration);
            yield return new WaitForSeconds(fixationDuration);
        
            trialEventType = TrialEventType.Arrow;
            Debug.Log("TARGET: " + sequenceOrder[i] + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + arrowDuration.ToString()); //send arrow event
            SendEvent(sequenceOrder[i], trialEventType, fixationDuration);
            yield return new WaitForSeconds(arrowDuration);

            if (Settings.instance.actionObservation)
            {
                trialEventType = TrialEventType.Observation;
                Debug.Log("TARGET: " + sequenceOrder[i] + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + observationDuration.ToString()); //send observation event
                SendEvent(sequenceOrder[i], trialEventType, fixationDuration);
                yield return new WaitForSeconds(observationDuration);
            }

            trialEventType = TrialEventType.Target;
            Debug.Log("TARGET: " + sequenceOrder[i] + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + targetDuration.ToString()); //send target event
            SendEvent(sequenceOrder[i], trialEventType, fixationDuration);
            yield return new WaitForSeconds(targetDuration);

            
            trialEventType = TrialEventType.Rest;
            Debug.Log("TARGET: " + sequenceOrder[i] + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + restDuration.ToString()); //send target event
            SendEvent(sequenceOrder[i], trialEventType, fixationDuration);
            yield return new WaitForSeconds(restDuration);
            
            sequenceIndex++;
            
            Debug.Log("-------- Trial: '" + sequenceIndex.ToString() + "' complete ------------------------");

            if (sequenceIndex >= sequenceLength){
                sequenceComplete = true;
                sequenceIndex = 0;
            }
        }
        
        //yield return new WaitUntil(() => sequenceComplete);
        Debug.Log("TRIAL SEQUENCE COMPLETE");
    }
    
    private float GetRestDuration()
    {
        float d = Random.Range(restDurationMin,restDurationMax);
        if (restDurationMax <= restDurationMin)
        {
            d = restDurationMin;
        }
        return d;
    }
    private float GetTotalDuration() {
        float d = fixationDuration + arrowDuration + observationDuration + targetDuration;
        if (!Settings.instance.actionObservation) {
            d = d - observationDuration;
        }
        return d;
    }
    
    private void SendEvent(int tNum, TrialEventType eType, float dur){
        if (OnTargetAction != null) {
            OnTargetAction(tNum,eType,dur);
        }
    }
}
