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
    public bool actionObservation;

    [Space(5)]
    public float duration;
    
    public TrialEventType trialEventType;
    public float preTrialWaitPeriod = 1f;
    public float fixationPeriod = 2f;
    public float indicationPeriod = 2f;
    public float observationPeriod = 2f;
    public float targetPresentationPeriod = 2f;
    public float restPeriod;
    public float restPeriodMin = 2;
    public float restPeriodMax;
    public float postTrialWaitPeriod = 1f;
    
    public bool sequenceComplete = false;

    public int[] sequenceOrder = new int[0];

    public delegate void TargetAction(int trialTotal,int trialIndex, int targetNumber, TrialEventType eType, float dur);
    public static event TargetAction OnTargetAction;

    #region Initialise
    void Awake(){
        gameManager = GetComponent<GameManager>();
        sequenceLength = gameManager.trialTotal;
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
            
            restPeriod = GetRestDuration();
            duration = GetTotalDuration() + restPeriod;
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.PreTrialPhase;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, preTrialWaitPeriod);
            yield return new WaitForSeconds(preTrialWaitPeriod);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Fixation;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, fixationPeriod);
            yield return new WaitForSeconds(fixationPeriod);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Arrow;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, indicationPeriod);
            yield return new WaitForSeconds(indicationPeriod);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            if (actionObservation){
                trialEventType = TrialEventType.Observation;
                UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, observationPeriod);
                yield return new WaitForSeconds(observationPeriod);
            }
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.Target;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, targetPresentationPeriod);
            yield return new WaitForSeconds(targetPresentationPeriod);

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Rest;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, restPeriod);
            yield return new WaitForSeconds(restPeriod);

            sequenceIndex++;
            
            //POST TRIAL SEQUENCE -----
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.PostTrialPhase;
            UpdateTrialStatus(sequenceLength,i,sequenceOrder[i], trialEventType, postTrialWaitPeriod);
            yield return new WaitForSeconds(postTrialWaitPeriod);

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
        float d = Random.Range(restPeriodMin, restPeriodMax);
        if (restPeriodMax <= restPeriodMin){
            d = restPeriodMin;
        }

        return d;
    }

    private float GetTotalDuration(){
        float d = fixationPeriod + indicationPeriod + observationPeriod + targetPresentationPeriod;
        if (!Settings.instance.actionObservation){
            d = d - observationPeriod;
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