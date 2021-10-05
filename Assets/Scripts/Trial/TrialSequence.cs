using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class TrialSequence : MonoBehaviour {
    
    public static TrialSequence instance;

    public TrialEventType trialEventType;
    public TextMeshProUGUI sequenceDebugDisplay;
    
    //TODO - add linear sequence
    public SequenceType sequenceType = SequenceType.Permutation;

    private int tNumID;
    
    //SEQUENCE ACTIVE
    private bool runSequence = false;

    public bool sequenceComplete;
    
//    [Header("SEQUENCE SETTINGS")]
//    [HideInInspector] 
    public TrialType trialType = TrialType.CentreOut;
//    [HideInInspector] 
    public int repetitions = 25; // num of sequences to run
//    [HideInInspector] 
    public int startDelay = 60;
    
    
//    [HideInInspector] 
    public float fixationDuration = 2f;
    //    [HideInInspector] 
    public float arrowDuration = 2f;
    //    [HideInInspector] 
    public float observationDuration = 2f;
    //    [HideInInspector] 
    public float targetDuration = 2f;

//    [HideInInspector] 
    public float restDuration;
    public int restDurationMin = 2;

    public float restDurationMax;

    private bool triggerSent = false;

    [Header("SEQUENCE OBJECTS 2D (rest last object)")]
    public GameObject[] target = new GameObject[5];
    private GameObject[] t = new GameObject[5];
    [Space(5)]
    public Material defaultMaterial;
    public Material highlightedMaterial;
    
    [Header("DEBUG")]
    public float elapsedTime;
    public float duration;
    public int sequenceIndex = 0;

    private bool fixationSeq = false;
    private bool arrowSeq = false;
    private bool targetSeq = false;

    [Space(10)]
    public int[] sequenceOrder = new int[0];

    public delegate void TargetAction(int targetNumber, TrialEventType eType, float dur);
    public static event TargetAction OnTargetAction;
    public delegate void TargetRestAction(int targetNumber, TrialEventType eType, float dur);
    public static event TargetRestAction OnTargetRestAction;

    private void OnEnable(){

    }
    private void OnDisable(){

    }
    private void Awake() {
        instance = this;
        t = target;
    }

    void Start() {
        //InitialiseSequence();
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status,"System Ready");
    }
    public void InitialiseTrial() {
        
        //TODO fix trial initilisation - centre out or 4 targets has error 
        if (trialType == TrialType.Three_Targets) {
            //take the last object away from the array
            GameObject[] g = new GameObject[3];
            for (int i = 0; i < target.Length-1; i++) {
                g[i] = target[i];
            }
            target[target.Length-2].SetActive(false);
            target = new GameObject[3];
            target = g;
        }
        if (trialType == TrialType.Four_Targets) {
            GameObject[] g = new GameObject[4];
            for (int i = 0; i < target.Length-1; i++) {
                g[i] = target[i];
            }
            target[target.Length-1].SetActive(false);
            target = new GameObject[4];
            target = g;
        }
        if (trialType == TrialType.CentreOut) {
            target = new GameObject[t.Length];
            target = t;
        }
    }
    public void InitialiseSequence() {
        int targetCount = target.Length;
        sequenceOrder = CSC_Math.IntArray_from_ElementRepeatNumbers(targetCount, repetitions);
        sequenceOrder = CSC_Math.RandPerm_intArray(sequenceOrder);
        UI_DisplayText.instance.SetTrialProgress(sequenceIndex, sequenceOrder.Length);
        
    }

    public void Reset()
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }

        //InitialiseSequence();
        sequenceIndex = 0;
        elapsedTime = 0;
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "System Ready");
        Debug.Log("-----TRIAL INITIALISED-----");
    }
    public void Initialise()
    {
        InitialiseSequence();
        sequenceIndex = 0;
        elapsedTime = 0;
        Debug.Log("-----TRIAL INITIALISED-----");
        Settings.instance.Status = GameStatus.Running;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");
    }

    public void StartTrial() {
        //InitialiseSequence();
        if (!runSequence) {
            Debug.Log("-----TRIAL STARTED-----");
            RunTrial();
        }
    }
    public void RunTrial() {

        if (!runSequence) {
            // duration = targetDuration;    
            restDuration = GetRestDuration();
            duration = GetTotalDuration() + restDuration;
            sequenceIndex = 0;
            elapsedTime = 0;
            sequenceComplete = false;
            elapsedTime = duration;
            runSequence = true;

            Settings.instance.Status = GameStatus.Running;
            UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");
        }
    }
    public void StopTrial() {
        CancelInvoke();
        runSequence = false;
        sequenceIndex = 0;
        elapsedTime = 0;
        //stops sending an extra rest event at end of sequence
        if (!sequenceComplete)
        {
            StartCoroutine(RestTarget());
        }
        
        Debug.Log("-----TRIAL STOPPED----- ");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "System Ready");
        UI_DisplayText.instance.SetTrialProgress(sequenceIndex, sequenceOrder.Length);
    }

    // void Update()
    // {
    //     if (runSequence && sequenceCount < repetitions)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         if (elapsedTime >= duration)
    //         {
    //             elapsedTime = 0; 
    //             
    //             if (!resting) {
    //                 resting = true;
    //                 if (restDurationMax <= restDurationMin)
    //                 {
    //                     duration = restDurationMin;
    //                 }
    //                 else
    //                 {
    //                     restDuration=  Random.Range(restDurationMin,restDurationMax);
    //                     duration = restDuration;
    //                 }
    //                 StartCoroutine(RestTarget());
    //                 if (sequenceComplete)
    //                 {
    //                     runSequence = false;
    //                     sequenceIndex = 0;
    //                     elapsedTime = 0;
    //                     StartCoroutine(CompleteSequence());
    //                 }
    //             }
    //             else {
    //                 resting = false;
    //                 // duration = targetDuration;
    //                 duration = GetTotalDuration();
    //                 //SetTarget(); //new trial target started
    //                 StartCoroutine(SetTargetSequence()); //TRIAL EVENT SEQUENCE
    //             }
    //         }
    //     }
    // }

    
    private void Update()
    {
        if (runSequence && sequenceIndex < sequenceOrder.Length)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration ){
                elapsedTime = 0; 
                Debug.Log(sequenceIndex + " ---------------------------------------------");
                StartCoroutine(SetTargetSequence()); //TRIAL EVENT SEQUENCE
            }
        }

    }
    
    private IEnumerator SetTargetSequence()
    {
        restDuration = GetRestDuration();
        duration = GetTotalDuration() + restDuration;
        
        tNumID = sequenceOrder[sequenceIndex];
        
        //update display
        UI_DisplayText.instance.SetTrialProgress(sequenceIndex+1, sequenceOrder.Length);

        trialEventType = TrialEventType.Fixation;
        //Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + fixationDuration.ToString()); //send fixation event
        sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + fixationDuration.ToString() +"s";
        SendEvent(tNumID, trialEventType, fixationDuration);
        yield return new WaitForSeconds(fixationDuration);
        
        trialEventType = TrialEventType.Arrow;
        //Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + arrowDuration.ToString()); //send arrow event
        sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + arrowDuration.ToString() +"s";
        SendEvent(tNumID, trialEventType, arrowDuration);
        yield return new WaitForSeconds(arrowDuration);

        if (Settings.instance.actionObservation)
        {
            trialEventType = TrialEventType.Observation;
            //Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + observationDuration.ToString()); //send observation event
            sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + observationDuration.ToString()+"s";
            SendEvent(tNumID, trialEventType, observationDuration);
            yield return new WaitForSeconds(observationDuration);
        }

        trialEventType = TrialEventType.Target;
        //Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + targetDuration.ToString()); //send target event
        sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + targetDuration.ToString()+"s";
        SendEvent(tNumID, trialEventType, targetDuration);
        yield return new WaitForSeconds(targetDuration);

        
        trialEventType = TrialEventType.Rest;
        
        //Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + restDuration.ToString()); //send target event
        SendEvent(tNumID, trialEventType, restDuration);
        sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + restDuration.ToString()+"s";
        yield return new WaitForSeconds(restDuration);
        
        sequenceIndex++;

        if (sequenceIndex >= sequenceOrder.Length){
            sequenceComplete = true;
            runSequence = false;
            StartCoroutine(CompleteSequence());
            sequenceIndex = 0;
            elapsedTime = 0;
        }
        
    }
    private float GetTotalDuration() {
        float d = fixationDuration + arrowDuration + observationDuration + targetDuration;
        if (!Settings.instance.actionObservation) {
            d = d - observationDuration;
        }
        return d;
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
    
    private void SetTarget() 
    {
        
        // //For 2D
        // for (int i = 0; i < target.Length; i++)
        // {
        //     target[i].GetComponent<Renderer>().material = defaultMaterial;
        // }

        //MOVE TO UPDATE - tNum should be available before event sequence
        
        tNumID = sequenceOrder[sequenceIndex];

        // //3D Target Event OLD VERSION **** MOVED TO TARGET SEQUENCE
        // if (OnTargetAction != null)
        // {
        //     //OnTargetAction(tNumID);
        // }

        //For 2D
        //TODO - redo 2D once 3D system is working (no priority - additional work)
        //2D Target Highlight 
        // target[tNumID].GetComponent<Renderer>().material = highlightedMaterial;
        // if (animateTargets) {
        //     target[tNumID].GetComponent<TargetAnimator>().ScaleTarget();
        // }
        
        //next sequence index - this is for next target
        
        sequenceIndex++;
        
        //update display
        UI_DisplayText.instance.SetTrialProgress(sequenceIndex, sequenceOrder.Length);

        //TODO NEEDS TO EXECUTE AFTER REST (THIS IS STOPPING THE UPDATE ON LAST TARGET - Not sure what this is??? (old note) (possibly fixed by 'CompleteSequence()'
        if (sequenceIndex >= sequenceOrder.Length)
        {
            sequenceComplete = true;
            // runSequence = false;
             sequenceIndex = 0;
            // elapsedTime = 0;
            // StartCoroutine(CompleteSequence());
        }
    }

    private void SendEvent(int tNum, TrialEventType eType, float dur)
    {
        if (eType != TrialEventType.Rest) {
            if (OnTargetAction != null) {
                OnTargetAction(tNum,eType,dur);
            }
        }
        else {
            if (OnTargetRestAction != null) {
                OnTargetRestAction(tNum,eType,dur);
            }
        }
    }
    

    // private void SendUDP_byte(int t)
    // {
    //     UDPClient.instance.SendData((byte)t);
    //     //        Debug.Log("Value to send : " + value);
    // }

    private IEnumerator RestTarget() {
        
        //frame delay for tNum to generate
        yield return new WaitForEndOfFrame();


        //FOR 2D
        // for (int i = 0; i < target.Length; i++)
        // {
        //     target[i].GetComponent<Renderer>().material = defaultMaterial;
        // }

        tNumID = sequenceOrder[sequenceIndex];

        //**** MOVED TO TARGET SEQUENCE
        if (OnTargetRestAction != null)
        {
            //OnTargetRestAction(tNumID);
        }
        
        trialEventType = TrialEventType.Rest;
        SendEvent(tNumID, trialEventType, restDuration);

        Debug.Log("TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + restDuration.ToString()); //send target event
        sequenceDebugDisplay.text = "TARGET: " + tNumID + " - EVENT: " + trialEventType.ToString() + " - TIMING: " + restDuration.ToString()+"s";
        
    }
    private IEnumerator CompleteSequence()
    {
        yield return new WaitForSeconds(targetDuration);

        TrialControls.instance.SetStop();
        
        yield return new WaitForSeconds(restDuration);
        
        Debug.Log("-----SEQUENCE COMPLETED-----");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Complete");

        TrialManager.instance.BlockComplete();

    }
}
