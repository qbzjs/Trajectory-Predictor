using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Random = UnityEngine.Random;

public class TrialSequencer : MonoBehaviour
{
    private GameManager gameManager;
    
    public int targetCount = 4;
    public int repetitions = 4;
    public int sequenceLength = 0;
    public int sequenceIndex = 0;

    [Header("---------")]
    //public float timer;
    public float duration;
    
    public TrialEventType trialEventType;
    public float preTrialWaitDuration = 1f;
    public float fixationDuration = 2f;
    public float arrowDuration = 2f;
    public float observationDuration = 2f;
    public float targetDuration = 2f;
    public float restDuration;
    public float restDurationMin = 2;
    public float restDurationMax;
    public float postTrialWaitDuration = 1f;
    
    public bool sequenceComplete = false;

    public int[] sequenceOrder = new int[0];

    public delegate void TargetAction(int trialTotal,int trialIndex, int targetNumber, TrialEventType eType, float dur);
    public static event TargetAction OnTargetAction;

    #region Initialise
    void Awake(){
        gameManager = GetComponent<GameManager>();
    }
    void Start(){
        sequenceIndex = 0;
        InitialiseTrial();
    }
    //call from settings to update every change
    public void InitialiseTrial(){
        GenerateSequence();
        sequenceComplete = false;
        sequenceLength = sequenceOrder.Length;
        trialEventType = TrialEventType.Ready;
        UpdateTrialStatus(sequenceLength,0,0, trialEventType, 0);
    }

    #endregion

    #region User Input

    void Update(){
        if (Input.GetKeyDown(KeyCode.G)){
            StartTrialSequence();
        }
    }

    //run from blockmanager
    public void StartTrialSequence(){
        InitialiseTrial();
        StartCoroutine(RunTrialSequence());
    }

    #endregion
    
    #region Trial Logic

    public void GenerateSequence(){
        sequenceOrder = CSC_Math.IntArray_from_ElementRepeatNumbers(targetCount, repetitions);
        sequenceOrder = CSC_Math.RandPerm_intArray(sequenceOrder);
    }

    private IEnumerator RunTrialSequence(){
        sequenceIndex = 0;
        
        for (int i = 0; i < sequenceLength; i++){
            
            restDuration = GetRestDuration();
            duration = GetTotalDuration() + restDuration;
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.PreTrialPhase;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, preTrialWaitDuration);
            yield return new WaitForSeconds(preTrialWaitDuration);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Fixation;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, fixationDuration);
            yield return new WaitForSeconds(fixationDuration);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Arrow;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, arrowDuration);
            yield return new WaitForSeconds(arrowDuration);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            if (Settings.instance.actionObservation){
                trialEventType = TrialEventType.Observation;
                UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, observationDuration);
                yield return new WaitForSeconds(observationDuration);
            }
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.Target;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, targetDuration);
            yield return new WaitForSeconds(targetDuration);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Rest;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, restDuration);
            yield return new WaitForSeconds(restDuration);

            sequenceIndex++;
            
            //POST TRIAL SEQUENCE -----
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.PostTrialPhase;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, postTrialWaitDuration);
            yield return new WaitForSeconds(postTrialWaitDuration);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            Debug.Log("____TRIAL END ____________________________________________");
            
            if (sequenceIndex >= sequenceLength){
                sequenceComplete = true;
                trialEventType = TrialEventType.Complete;
                sequenceIndex = 0;
                UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, 0);
            }
        }
    }

    private float GetRestDuration(){
        float d = Random.Range(restDurationMin, restDurationMax);
        if (restDurationMax <= restDurationMin){
            d = restDurationMin;
        }

        return d;
    }

    private float GetTotalDuration(){
        float d = fixationDuration + arrowDuration + observationDuration + targetDuration;
        if (!Settings.instance.actionObservation){
            d = d - observationDuration;
        }

        return d;
    }
    #endregion

    #region Status & Event Updates

    private void UpdateTrialStatus(int total, int index, int tNum, TrialEventType eType, float dur){
        if (OnTargetAction != null){
            OnTargetAction(total, index, tNum, eType, dur);
        }
        gameManager.TrialTracker(total, index, tNum, eType, dur);

        if (gameManager.debug){
            Debug.Log("Trial Index: " + index + "Target: " + tNum + " - Event: " + trialEventType.ToString() + " - Timing: " + dur);
        }
    }

    #endregion
}