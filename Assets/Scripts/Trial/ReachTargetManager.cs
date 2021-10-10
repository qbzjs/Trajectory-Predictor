using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Leap.Unity;

public class ReachTargetManager : MonoBehaviour{
    
    private GameObject reachObject;
    [Range(-4f, 4f)]
    public float rightLeftOffset = 0.5f;
    private Vector3 positionLeft;
    private Vector3 positionRight;
        
    public GameObject[] reachTarget = new GameObject[5];

    public Material targetDefault;
    public Material targetHighlight;

    public bool animateTargets;

    private int lastTrigger;
    private int currentTrigger;

    private Handedness _handedness = Handedness.Left;

    public bool oneShotTriggerUDP = false;

    //send general target action
    public delegate void TargetAction(Transform target, Handedness side);
    public static event TargetAction OnTargetAction;
    //send to ghost target controller - move arm to target - target null move arm back
    public delegate void TargetRestAction(Transform target, Handedness side);
    public static event TargetRestAction OnTargetRestAction;


    //TODO----------------NEW EVENTS
    private void OnEnable()
    {
        // TrialSequence.OnTargetAction += TrialSequence_OnTargetAction;
        // TrialSequence.OnTargetRestAction += TrialSequence_OnTargetRestAction;
    }
    private void OnDisable()
    {
        // TrialSequence.OnTargetAction -= TrialSequence_OnTargetAction;
        // TrialSequence.OnTargetRestAction -= TrialSequence_OnTargetRestAction;
    }

    void Awake(){
        reachObject = gameObject;
        positionRight = reachObject.transform.position;
        //float x = Mathf.Abs(reachObject.transform.position.x);
        //rightLeftOffset = -rightLeftOffset;
        float x = reachObject.transform.position.x + rightLeftOffset;
        positionLeft = new Vector3(x, reachObject.transform.position.y, reachObject.transform.position.z);
        
        for (int i = 0; i < reachTarget.Length; i++)
        {
            //reachTarget[i].GetComponent<Renderer>().material = targetDefault;
            reachTarget[i].transform.Find("Target").GetComponent<Renderer>().material = targetDefault;
        }
    }

    private void TrialSequence_OnTargetAction(int targetNumber, TrialEventType eType, float dur)
    {
        
        //Debug.Log(targetNumber + " : " + reachTarget[targetNumber].name.ToString() + " (EEG)");

        for(int i=0; i<reachTarget.Length; i++)
        {
            //reachTarget[i].GetComponent<Renderer>().material = targetDefault;
            reachTarget[i].transform.Find("Target").GetComponent<Renderer>().material = targetDefault;
        }

        //reachTarget[targetNumber].GetComponent<Renderer>().material = targetHighlight;
        reachTarget[targetNumber].transform.Find("Target").GetComponent<Renderer>().material = targetHighlight;

        if (animateTargets)
        {
            reachTarget[targetNumber].GetComponent<TargetAnimator>().ScaleTarget();
        }

        if (OnTargetAction != null)
        {
            OnTargetAction(reachTarget[targetNumber].transform, _handedness);
        }

        currentTrigger = targetNumber;

        DAO.instance.ReachTarget = targetNumber + 1;
        
        PlayBeep(1f);

        if (eType == TrialEventType.Target)
        {
            SendUDP_byte(targetNumber + 1);

            if (oneShotTriggerUDP) {
                StartCoroutine(TriggerReset());
            }
        }
    }
    private void TrialSequence_OnTargetRestAction(int targetNumber, TrialEventType eType, float dur)
    {
        //Debug.Log(targetNumber + " : " + reachTarget[targetNumber].name.ToString() + " (EEG)");

        for (int i = 0; i < reachTarget.Length; i++)
        {
            //reachTarget[i].GetComponent<Renderer>().material = targetDefault;
            reachTarget[i].transform.Find("Target").GetComponent<Renderer>().material = targetDefault;
        }

        if (OnTargetRestAction != null)
        {
            OnTargetRestAction(reachTarget[targetNumber].transform, _handedness);
        }

        // SendUDP_byte(targetNumber+10);

        lastTrigger = currentTrigger;
        lastTrigger++;
        DAO.instance.ReachTarget = lastTrigger + 10;
        SendUDP_byte(lastTrigger+10);

        //PlayBeep(0.8f);

        if (oneShotTriggerUDP)
        {
            StartCoroutine(TriggerReset());
        }
    }

    private IEnumerator TriggerReset()
    {
        yield return new WaitForEndOfFrame();
        SendUDP_byte(0);
    }

    public void SetReachSide(Handedness side)
    {
        _handedness = side;
        if (side == Handedness.Left){
            reachObject.transform.position = positionLeft;
        }

        if (side == Handedness.Right){
            reachObject.transform.position = positionRight;
        }
    }
    
    private void PlayBeep(float p){
        if (reachTarget[currentTrigger].GetComponent<AudioSource>())
        {
            AudioSource a = reachTarget[currentTrigger].GetComponent<AudioSource>();
            a.pitch = p;
            a.Play();
        }
//        print("AUDIO - Beep");

    }
    
    private void SendUDP_byte(int t)
    {
        Debug.Log("UDP Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }
    
    //******temp
    
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha0))
    //     {
    //         Debug.Log("target centre");
    //         SetTarget(0);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         Debug.Log("target left");
    //         SetTarget(1);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         Debug.Log("target top");
    //         SetTarget(2);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         Debug.Log("target right");
    //         SetTarget(3);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha4))
    //     {
    //         Debug.Log("target bottom");
    //         SetTarget(4);
    //     }
    // }
    //
    // public void SetTarget(int tNum)
    // {
    //     TargetController controller;
    //     controller = gameObject.GetComponent<TargetController>();
    //     controller.Target(tNum);
    // }
    
}
