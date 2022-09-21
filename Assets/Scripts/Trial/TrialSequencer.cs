using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Random = UnityEngine.Random;

public class TrialSequencer : MonoBehaviour
{
    private GameManager gameManager;

    //TODO WAIT FOR USER INPUT
    public bool pauseForInput;
        
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
        
        //pause function - NEWLY ADDED
        yield return new WaitUntil(() => !gameManager.paused);

        //trial event
        gameManager.TrialEvent(TrialEventType.TrialSequenceStarted, -1, 0,sequenceIndex,sequenceLength);
        
        for (int i = 0; i < sequenceLength; i++){
            
            restPeriod = GetRestDuration();
            duration = GetTotalDuration() + restPeriod;
            
            sequenceIndex++;

            // trialEventType = TrialEventType.TrialSequenceStarted;
            // UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, 0);
            
            //pause function - NEWLY ADDED
            yield return new WaitUntil(() => !gameManager.paused);

            //trial event
            gameManager.TrialEvent(TrialEventType.PreTrialPhase, sequenceOrder[i],gameManager.SpeedCheck(preTrialWaitPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            //UDP pre-trial trigger - for ERSP (NEWLY ADDED)
            //gameManager.SendUDP_byte(99,0, TrialEventType.PreTrialPhase); //modifier adds 1 to the trigger number
            
            trialEventType = TrialEventType.PreTrialPhase;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, preTrialWaitPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(preTrialWaitPeriod));

            //trial event
            gameManager.TrialEvent(TrialEventType.Fixation, sequenceOrder[i],gameManager.SpeedCheck(fixationPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Fixation;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, fixationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(fixationPeriod));

            //trial event
            gameManager.TrialEvent(TrialEventType.Indication, sequenceOrder[i],gameManager.SpeedCheck(indicationPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            gameManager.SendUDP_byte(99,0, TrialEventType.Indication); //modifier adds 1 to the trigger number
            trialEventType = TrialEventType.Indication;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, indicationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(indicationPeriod));

            //trial event
            gameManager.TrialEvent(TrialEventType.Observation, sequenceOrder[i],gameManager.SpeedCheck(observationPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            if (actionObservation){
                trialEventType = TrialEventType.Observation;
                UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, observationPeriod);
                yield return new WaitForSeconds(gameManager.SpeedCheck(observationPeriod));
            }
            
            //trial event
            gameManager.TrialEvent(TrialEventType.TargetPresentation, sequenceOrder[i],gameManager.SpeedCheck(targetPresentationPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            //UDP ON
            gameManager.SendUDP_byte(sequenceOrder[i],1, TrialEventType.TargetPresentation); //modifier adds 1 to the trigger number
            
            //TARGET PRESENTATION
            trialEventType = TrialEventType.TargetPresentation;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, targetPresentationPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(targetPresentationPeriod));
            
            //UDP OFF
            gameManager.SendUDP_byte(sequenceOrder[i]+10, 1, TrialEventType.Rest); //modifier adds 1 to the trigger number

            //trial event
            gameManager.TrialEvent(TrialEventType.Rest, sequenceOrder[i],gameManager.SpeedCheck(restPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            trialEventType = TrialEventType.Rest;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, restPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(restPeriod));
            
            
            //POST TRIAL SEQUENCE -----
            
            //trial event
            gameManager.TrialEvent(TrialEventType.PostTrialPhase, sequenceOrder[i],gameManager.SpeedCheck(postTrialWaitPeriod),sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);

            trialEventType = TrialEventType.PostTrialPhase;
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, postTrialWaitPeriod);
            yield return new WaitForSeconds(gameManager.SpeedCheck(postTrialWaitPeriod));

            //trial event
            gameManager.TrialEvent(TrialEventType.TrialComplete, sequenceOrder[i],0,sequenceIndex,sequenceLength);
            
            //pause function
            yield return new WaitUntil(() => !gameManager.paused);
            
            UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], TrialEventType.Debug, 0);
            
            //Debug.Log("____TRIAL END ____________________________________________");
            
            if (sequenceIndex >= sequenceLength){
                sequenceComplete = true;
                trialEventType = TrialEventType.Complete;
                sequenceIndex = 0;
                UpdateTrialStatus(sequenceLength,sequenceIndex,sequenceOrder[i], trialEventType, 0);
                //trial event
                gameManager.TrialEvent(TrialEventType.TrialSequenceComplete, sequenceOrder[i],0,sequenceIndex,sequenceLength);
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