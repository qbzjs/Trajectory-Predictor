using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataObject 
{

    public string trialParadigm;
    public string targetCount;
    public int repetitions;
    public string handedness;

    public int runs;
    public int blocksPerRun;
    public int preBlockCountdown;
    public int visibleCountdown;
    public int interRunRestPeriod;
    
    public float preTrialWaitPeriod;
    public float fixationDuration;
    public float arrowDuration;
    public float observationDuration;
    public float targetDuration;
    public float restPeriodMin;
    public float restPeriodMax;
    public float postTrialWaitPeriod;
    public float postBlockWaitPeriod;
    public float postRunWaitPeriod;

    public bool actionObservation;
    public string sampleRate;

    //TODO - POSSIBLY IN ANOTHER CLASS
    //public int[] sequence;
    //public float[] timeSigniture / time sequence (target times
    

    public SettingsDataObject(string trialParadigm, string targets, int repetitions, string handedness, 
        int runs,int blocksPerRun,int preBlockCountdown,int visibleCountdown,int interRunRestPeriod,
        float preTrialWaitPeriod,float fixationDuration,float arrowDuration,float observationDuration,
        float targetDuration,float restPeriodMin,float restPeriodMax,
        float postTrialWaitPeriod,float postBlockWaitPeriod,float postRunWaitPeriod, 
        bool actionObservation, string sampleRate)
    {
        this.trialParadigm = trialParadigm;
        this.targetCount = targets;
        this.repetitions = repetitions;
        this.handedness = handedness;

        this.preTrialWaitPeriod = preTrialWaitPeriod;
        this.fixationDuration = fixationDuration;
        this.arrowDuration = arrowDuration;
        this.observationDuration = observationDuration;
        this.targetDuration = targetDuration;
        this.restPeriodMin = restPeriodMin;
        this.restPeriodMax = restPeriodMax;
        this.postTrialWaitPeriod = postTrialWaitPeriod;
        this.postBlockWaitPeriod = postBlockWaitPeriod;
        this.postRunWaitPeriod = postRunWaitPeriod;
        
        this.actionObservation = actionObservation;
        this.sampleRate = sampleRate;
    }
}
