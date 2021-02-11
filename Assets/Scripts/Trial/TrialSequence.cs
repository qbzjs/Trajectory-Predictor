using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TrialSequence : MonoBehaviour {
    
    public static TrialSequence instance;
    
    //SEQUENCE ACTIVE
    private bool runSequence = false;
    public bool resting = false;
    public bool animateTargets;
    
//    [Header("SEQUENCE SETTINGS")]
//    [HideInInspector] 
    public TrialType trialType = TrialType.CentreOut;
//    [HideInInspector] 
    public int repetitions = 15; // num of sequences to run
//    [HideInInspector] 
    public int startDelay = 5;
//    [HideInInspector] 
    public int targetDuration = 6;
//    [HideInInspector] 
    public int restDuration = 2;
    

    [Header("SEQUENCE OBJECTS (rest last object)")]
    public GameObject[] target = new GameObject[4];
    private GameObject[] t = new GameObject[4];
    [Space(5)]
    public Material defaultMaterial;
    public Material highlightedMaterial;
    
    [Header("DEBUG")]
    public float elapsedTime;
    public int duration;
    public int sequenceIndex = 0;
    public int sequenceCount = 0;

    [Space(10)]
    public int[] sequenceOrder = new int[0];

    public delegate void TargetAction(int targetNumber);
    public static event TargetAction OnTargetAction;
    
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
        
        InitialiseSequence();
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status,"Game Ready");
    }
    public void InitialiseTrial() {
        
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
        UI_DisplayText.instance.SetProgress(sequenceIndex, sequenceOrder.Length);
    }
    
    public void StartTrial() {
        InitialiseSequence();
//        InitialiseTrial(); //initialised from settings
        if (!runSequence) {
            resting = true;
            duration = startDelay;
            sequenceCount = 0;
            sequenceIndex = 0;
            elapsedTime = 0;
            runSequence = true;
            Debug.Log("-----TRIAL STARTED-----");
            Settings.instance.Status = GameStatus.Running;
            UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Game Running");
        }
    }
    public void StopTrial() {
        runSequence = false;
        sequenceCount = 0;
        sequenceIndex = 0;
        elapsedTime = 0;
        RestTarget();
        Debug.Log("-----TRIAL STOPPED-----");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Game Ready");
        UI_DisplayText.instance.SetProgress(sequenceIndex, sequenceOrder.Length);
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
                    duration = restDuration;
                    RestTarget();
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
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }

        int tNumID = sequenceOrder[sequenceIndex];
        int tNum = tNumID+1;

        //SEND VALUE TO UDP
        SendUDP_byte(tNum);
        SendUDP_byte(0);

        //3D Target Event
        if (OnTargetAction != null)
        {
            OnTargetAction(tNum);
        }

        //2D Target Highlight
        target[tNumID].GetComponent<Renderer>().material = highlightedMaterial;
        if (animateTargets) {
            target[tNumID].GetComponent<TargetAnimator>().ScaleTarget();
        }
        sequenceIndex++;
        
        //update display
        UI_DisplayText.instance.SetProgress(sequenceIndex, sequenceOrder.Length);

        if (sequenceIndex >= sequenceOrder.Length)
        {
            runSequence = false;
            sequenceIndex = 0;
            elapsedTime = 0;
            StartCoroutine(CompleteSequence());
        }
    }


    private void SendUDP_byte(int t)
    {
        UDPClient.instance.UdpSend_byte((byte)t);
        //        Debug.Log("Value to send : " + value);
    }

    private void RestTarget() {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }
    }
    private IEnumerator CompleteSequence()
    {
        yield return new WaitForSeconds(targetDuration);
        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<Renderer>().material = defaultMaterial;
        }
        yield return new WaitForSeconds(restDuration);
        
        Debug.Log("-----SEQUENCE COMPLETED-----");
        Settings.instance.Status = GameStatus.Ready;
        UI_DisplayText.instance.SetStatus(Settings.instance.Status, "Game Complete");

//        yield return new WaitForSeconds(targetDuration);
//        Debug.Log("-----SEQUENCE READY-----");
//        Settings.instance.Status = GameStatus.Ready;
//        UI_DisplayText.instance.SetStatus(Settings.instance.Status);
//        UI_DisplayText.instance.SetComplete("");
    }
}
