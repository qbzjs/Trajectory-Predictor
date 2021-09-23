using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataObject 
{

    public string trialType;
    public string paradigm;
    public string handedness;
    public bool actionObservation;

    public string sampleRate;
    
    public int trialBlocks;
    public int repetitions;
    public int startDelay;
    public int interBlockRestPeriod;
    public int restDurationMin;
    public int restDurationMax;
    public int targetDuration;


    //TODO - POSSIBLY IN ANOTHER CLASS
    //public int[] sequence;
    //public float[] timeSigniture / time sequence (target times



    public SettingsDataObject(string trialType, string paradigm, string handedness, bool actionObservation,
        string sampleRate, int trialBlocks, int repetitions, int startDelay, int interBlockRestPeriod, int restDurationMin, int restDurationMax, int targetDuration)
    {
        this.trialType = trialType;
        this.paradigm = paradigm;
        this.handedness = handedness;
        this.actionObservation = actionObservation;
        this.sampleRate = sampleRate;
        this.trialBlocks = trialBlocks;
        this.repetitions = repetitions;
        this.startDelay = startDelay;
        this.interBlockRestPeriod = interBlockRestPeriod;
        this.restDurationMin = restDurationMin;
        this.restDurationMax = restDurationMax;
        this.targetDuration = targetDuration;
    }
}
