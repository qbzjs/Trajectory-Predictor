using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TrialSequence : MonoBehaviour {
    
    public static TrialSequence instance;

    //TODO - add linear sequence
    public SequenceType sequenceType = SequenceType.Permutation;
    
    //SEQUENCE ACTIVE
    private bool runSequence = false;
    public bool resting = false;
    public bool animateTargets;

    public bool sequenceComplete;
    
//    [Header("SEQUENCE SETTINGS")]
//    [HideInInspector] 
    public TrialType trialType = TrialType.CentreOut;
//    [HideInInspector] 
    public int repetitions = 25; // num of sequences to run
//    [HideInInspector] 
    public int startDelay = 60;
//    [HideInInspector] 
    public float targetDuration = 2;
//    [HideInInspector] 
    [FormerlySerializedAs("restDuration")] 
    public int restDurationMin = 2;

    public float restDurationMax;

    private bool triggerSent = false;

    [Header("SEQUENCE OBJECTS (rest last object)")]
    public GameObject[] target = new GameObject[5];
    private GameObject[] t = new GameObject[5];
    [Space(5)]
    public Material defaultMaterial;
    public Material highlightedMaterial;
    
    [Header("DEBUG")]
    public float elapsedTime;
    public float duration;
    public int sequenceIndex = 0;
    public int sequenceCount = 0;

    [Space(10)]
    public int[] sequenceOrder = new int[0];

    public delegate void TargetAction(int targetNumber);
    public static event TargetAction OnTargetAction;
    public delegate void TargetRestAction(int targetNumber);
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
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }
        
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
        UI_DisplayText.instance.SetProgressMovement(sequenceIndex, sequenceOrder.Length);
        
    }

    public void Reset()
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }

        //InitialiseSequence();
        resting = true;
        sequenceCount = 0;
        sequenceIndex = 0;
        elapsedTime = 0;
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "System Ready");
        Debug.Log("-----TRIAL INITIALISED-----");
    }
    public void Initialise()
    {
        InitialiseSequence();
        resting = true;
        sequenceCount = 0;
        sequenceIndex = 0;
        elapsedTime = 0;
        Debug.Log("-----TRIAL INITIALISED-----");
        Settings.instance.Status = GameStatus.Running;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");
    }
//     public void StartTrial() {
//         InitialiseSequence();
// //        InitialiseTrial(); //initialised from settings
//         if (!runSequence) {
//             resting = true;
//             duration = startDelay;           
//             sequenceCount = 0;
//             sequenceIndex = 0;
//             elapsedTime = 0;
//             runSequence = true;
//             Debug.Log("-----TRIAL STARTED-----");
//             RestTarget();
//             Settings.instance.Status = GameStatus.Running;
//             UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");
//         }
//     }
    public void StartTrial() {
        //InitialiseSequence();
        if (!runSequence) {
            //resting = true;
            //sequenceCount = 0;
            //sequenceIndex = 0;
            //elapsedTime = 0;
            Debug.Log("-----TRIAL STARTED-----");
            //Settings.instance.Status = GameStatus.Running;
            //UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");

            //Invoke("RunTrial", startDelay);  //startdelay now the waiting period
            RunTrial();
        }
    }
    public void RunTrial() {
        //InitialiseSequence();
//        InitialiseTrial(); //initialised from settings
        if (!runSequence) {
            resting = false;
            duration = targetDuration;           
            sequenceCount = 0;
            sequenceIndex = 0;
            elapsedTime = 0;
            sequenceComplete = false;
            runSequence = true;
            Debug.Log("-----TRIAL STARTED-----");
            SetTarget();
            Settings.instance.Status = GameStatus.Running;
            UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Running");
        }
    }
    public void StopTrial() {
        CancelInvoke();
        runSequence = false;
        sequenceCount = 0;
        sequenceIndex = 0;
        elapsedTime = 0;
        //stops sending an extra rest event at end of sequence
        if (!sequenceComplete)
        {
            RestTarget();
        }
        
        Debug.Log("-----TRIAL STOPPED----- ");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "System Ready");
        UI_DisplayText.instance.SetProgressMovement(sequenceIndex, sequenceOrder.Length);
    }
    void Update()
    {
        if (runSequence && sequenceCount < repetitions)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
            {
                elapsedTime = 0; 
                
                if (!resting) {
                    resting = true;
                    if (restDurationMax <= restDurationMin)
                    {
                        duration = restDurationMin;
                    }
                    else
                    {
                        duration =  Random.Range(restDurationMin,restDurationMax);
                    }
                    RestTarget();
                    if (sequenceComplete)
                    {
                        runSequence = false;
                        sequenceIndex = 0;
                        elapsedTime = 0;
                        StartCoroutine(CompleteSequence());
                    }
                }
                else {
                    resting = false;
                    duration = targetDuration;
                    SetTarget();
                }
            }
        }

    }

    //------PERMUTATION GENERATION
    private void SetTarget() 
    {
//        Debug.Log("Set Target");

        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }

        int tNumID = sequenceOrder[sequenceIndex];
        int tNum = tNumID+1;

       // // if (!triggerSent){
       //      //SEND VALUE TO UDP
       //      SendUDP_byte(tNum);
       //      SendUDP_byte(0);
       //      Debug.Log("trigger sent");
       //  //    triggerSent = true;
       //  //}

        //3D Target Event
        if (OnTargetAction != null)
        {
            OnTargetAction(tNumID);
        }

        //2D Target Highlight
        target[tNumID].GetComponent<Renderer>().material = highlightedMaterial;
        if (animateTargets) {
            target[tNumID].GetComponent<TargetAnimator>().ScaleTarget();
        }
        sequenceIndex++;
        
        //update display
        UI_DisplayText.instance.SetProgressMovement(sequenceIndex, sequenceOrder.Length);

        //TODO NEEDS TO EXECUTE AFTER REST (THIS IS STOPPING THE UPDATEON LAST TARGET
        if (sequenceIndex >= sequenceOrder.Length)
        {
            sequenceComplete = true;
            // runSequence = false;
             sequenceIndex = 0;
            // elapsedTime = 0;
            // StartCoroutine(CompleteSequence());
        }
    }


    private void SendUDP_byte(int t)
    {
        UDPClient.instance.SendData((byte)t);
        //        Debug.Log("Value to send : " + value);
    }

    private void RestTarget() {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }

        int tNumID = sequenceOrder[sequenceIndex];

        if (OnTargetRestAction != null)
        {
            OnTargetRestAction(tNumID);
        }

        Debug.Log("Rest : " + tNumID);
    }
    private IEnumerator CompleteSequence()
    {
        yield return new WaitForSeconds(targetDuration);
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }
        
        TrialControls.instance.SetStop();
        
        yield return new WaitForSeconds(restDurationMin);
        
        Debug.Log("-----SEQUENCE COMPLETED-----");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Trial Complete");

        TrialManager.instance.BlockComplete();

//        yield return new WaitForSeconds(targetDuration);
//        Debug.Log("-----SEQUENCE READY-----");
//        Settings.instance.Status = GameStatus.Ready;
//        UI_DisplayText.instance.SetStatus(Settings.instance.Status);
//        UI_DisplayText.instance.SetComplete("");
    }
}
