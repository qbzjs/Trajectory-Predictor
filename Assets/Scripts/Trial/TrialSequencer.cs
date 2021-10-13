using System;
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
    }
    void Start(){
        sequenceIndex = 0;
        sequenceComplete = false;
        trialEventType = TrialEventType.Ready;
        InitialiseTrial();
    }
    //call from settings to update every change
    public void InitialiseTrial(){
        GenerateSequence();
        sequenceLength = sequenceOrder.Length;
        UpdateTrialStatus(sequenceLength,0,0, trialEventType, 0);
    }

    #endregion

    #region User Input

    void Update(){
        // if (Input.GetKeyDown(KeyCode.G)){
        //     StartTrialSequence();
        // }
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
        sequenceComplete = false;
        
        for (int i = 0; i < sequenceLength; i++){
            
            restPeriod = GetRestDuration();
            duration = GetTotalDuration() + restPeriod;
            
            sequenceIndex++;
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.PreTrialPhase;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, preTrialWaitPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(preTrialWaitPeriod));

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Fixation;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, fixationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(fixationPeriod));

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Indication;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, indicationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(indicationPeriod));

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            if (actionObservation){
                trialEventType = TrialEventType.Observation;
                UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, observationPeriod);
                yield return new WaitForSeconds(gameManager.SpeedCheck(observationPeriod));
            }
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            //UDP ON
            gameManager.SendUDP_byte(sequenceOrder[i],"Trial Presentation");
            
            //TARGET PRESENTATION
            trialEventType = TrialEventType.Target;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, targetPresentationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(targetPresentationPeriod));
            
            //UDP OFF
            gameManager.SendUDP_byte(sequenceOrder[i]+10, "Trial Presentation Ended");

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Rest;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, restPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(restPeriod));
            
            
            //POST TRIAL SEQUENCE -----
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.PostTrialPhase;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, postTrialWaitPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(postTrialWaitPeriod));

            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], TrialEventType.Debug, 0);
            
            //Debug.Log("____TRIAL END ____________________________________________");
            
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
        if (OnTargetAction != null && eType!= TrialEventType.Debug){
            OnTargetAction(total, index, tNum, eType, dur);
        }

        if (eType != TrialEventType.Debug){
            gameManager.TrialTracker(eType, total, index, tNum, dur);
        }
        
        if (gameManager.debugTimingDetailed){
            Debug.Log("Trial Index: " + index + "Target: " + tNum + " - Event: " + trialEventType.ToString() + " - Timing: " + dur);
        }

        if (gameManager.debugTimingSimple && eType == TrialEventType.Debug){
            Debug.Log("____TRIAL END ____________________________________________");
        }
    }
    #endregion
}