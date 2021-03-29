using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
    
public class ReachTargetManager : MonoBehaviour{
    
    private GameObject reachObject;
    [Range(0, 1.5f)]
    public float rightLeftOffset = 0.5f;
    private Vector3 positionLeft;
    private Vector3 positionRight;
        
    public GameObject[] reachTarget = new GameObject[5];

    public Material targetDefault;
    public Material targetHighlight;

    public bool animateTargets;

    private int lastTrigger;
    private int currentTrigger;

    private TaskSide taskSide = TaskSide.Left;

    public bool oneShotTriggerUDP = false;

    //send general target action
    public delegate void TargetAction(Transform target, TaskSide side);
    public static event TargetAction OnTargetAction;
    //send to ghost target controller - move arm to target - target null move arm back
    public delegate void TargetRestAction(Transform target, TaskSide side);
    public static event TargetRestAction OnTargetRestAction;


    private void OnEnable()
    {
        TrialSequence.OnTargetAction += TrialSequence_OnTargetAction;
        TrialSequence.OnTargetRestAction += TrialSequence_OnTargetRestAction;
    }
    private void OnDisable()
    {
        TrialSequence.OnTargetAction -= TrialSequence_OnTargetAction;
        TrialSequence.OnTargetRestAction -= TrialSequence_OnTargetRestAction;
    }

    void Awake(){
        reachObject = gameObject;
        positionLeft = reachObject.transform.position;
        //float x = Mathf.Abs(reachObject.transform.position.x);
        rightLeftOffset = -rightLeftOffset;
        float x = reachObject.transform.position.x + rightLeftOffset;
        positionRight = new Vector3(x, reachObject.transform.position.y, reachObject.transform.position.z);
        
        for (int i = 0; i < reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }
    }

    private void TrialSequence_OnTargetAction(int targetNumber)
    {
        
        Debug.Log(targetNumber + 1 + " : " + reachTarget[targetNumber].name.ToString() + " (EEG)");

        for(int i=0; i<reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }

        reachTarget[targetNumber].GetComponent<Renderer>().material = targetHighlight;

        if (animateTargets)
        {
            reachTarget[targetNumber].GetComponent<TargetAnimator>().ScaleTarget();
        }

        if (OnTargetAction != null)
        {
            OnTargetAction(reachTarget[targetNumber].transform, taskSide);
        }

        currentTrigger = targetNumber;

        DAO.instance.ReachTarget = targetNumber + 1;
        
        SendUDP_byte(targetNumber + 1);

        PlayBeep(1f);

        if (oneShotTriggerUDP)
        {
            StartCoroutine(TriggerReset());
        }
        
    }
    private void TrialSequence_OnTargetRestAction(int targetNumber)
    {
        Debug.Log(targetNumber+1 + " : " + reachTarget[targetNumber].name.ToString() + " (EEG)");

        for (int i = 0; i < reachTarget.Length; i++)
        {
            reachTarget[i].GetComponent<Renderer>().material = targetDefault;
        }

        if (OnTargetRestAction != null)
        {
            OnTargetRestAction(reachTarget[targetNumber].transform, taskSide);
        }

        // SendUDP_byte(targetNumber+10);

        lastTrigger = currentTrigger;
        lastTrigger++;
        DAO.instance.ReachTarget = lastTrigger + 10;
        SendUDP_byte(lastTrigger+10);

        PlayBeep(0.8f);

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

    public void SetReachSide(TaskSide side)
    {
        taskSide = side;
        if (side == TaskSide.Left){
            reachObject.transform.position = positionLeft;
        }

        if (side == TaskSide.Right){
            reachObject.transform.position = positionRight;
        }
    }
    
    private void PlayBeep(float p){
        AudioSource a = reachTarget[currentTrigger].GetComponent<AudioSource>();
        a.pitch = p;
//        print("AUDIO - Beep");

        a.Play();
    }
    
    private void SendUDP_byte(int t)
    {
        Debug.Log("Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);
    }
}
