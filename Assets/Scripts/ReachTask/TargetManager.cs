using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Enums;
using UnityEngine;
using PathologicalGames;
using DG.Tweening;
using Leap.Unity;
using Unity.MLAgents;

public class TargetManager : MonoBehaviour
{
    private TargetController controller;
    
    public GameObject targetGhostPrefab;
    public GameObject[] targetGhosts = new GameObject[0];
    public GameObject targetPrefab;
    private GameObject target; //move this
    private GameObject targetMesh; //scale and colour this
    public GameObject fixationCrossPrefab;
    private GameObject fixationCross;
    public GameObject indicatorPrefab;
    private GameObject indicatior;

    public GameObject targetHomeObject;

    public GameObject ghostHandRight;
    public Renderer ghostHandRightMesh;

    
    
    private Renderer targetRenderer;
    private Renderer homeTargetRenderer;
    private Renderer fixationRenderer;
    private Renderer indicatorHeadRenderer;
    private Renderer indicatorBodyRenderer;
    private Color defaultColour;
    private Color highlightColour;
    private Color defaultFixationColour;
    private Color highlightFixationColour;
    private Color defaultIndicatorColour;
    private Color highlightIndicatorColour;

    public Transform originPoint;
    [Range(0.05f,0.25f)]
    public float targetDistance = 0.125f;
    
    public Vector3 offsetFromOrigin;
    public Vector3 targetDestination;
    public Transform destinationTransform;
    public Transform lineDestinationTransform;
    public Transform fallbackTransform;
    public Vector3 handPosition;

    [Range(0, 1)] public float animationDuration = 0.1f;

    public bool useLine = true;
    public bool lineToTarget = false;
    private LineRenderer lineRenderer;

    public int targetIndex;
    public float lifeTime;
    
    //ML
    public bool trainAgent;
    public GameObject agent;
    public GameObject agentGoal;
    
    //debugging
    private bool targetsActive;
    
    public delegate void TargetActions(bool targetPresent, bool restPresent, Vector3 position);
    public static event TargetActions OnTargetAction;


    #region Event Subscriptions

    private void OnEnable(){
        GameManager.OnRunAction += GameManagerOnRunAction;
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
        GameManager.OnTrialAction += GameManagerOnTrialAction;
    }

    private void OnDisable(){
        GameManager.OnRunAction -= GameManagerOnRunAction;
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
        GameManager.OnTrialAction -= GameManagerOnTrialAction;
    }

    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Reset){

        }

        if (eventType == GameStatus.Paused){

        }

        if (eventType == GameStatus.Unpaused){

        }
    }

    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal){
        if (eventType == GameStatus.RunStarted){

        }

        if (eventType == GameStatus.RunComplete){

        }

        if (eventType == GameStatus.AllRunsComplete){

        }
    }

    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.Countdown){
            InitialiseFixation();
            InitialiseGhostTargetArray();
        }

        if (eventType == GameStatus.VisibleCountdown){
            InitialiseHomeTarget();
        }

        if (eventType == GameStatus.CountdownComplete){
            if (trainAgent){
                agent.SetActive(true);
            }
            
        }

        if (eventType == GameStatus.BlockStarted){

        }

        if (eventType == GameStatus.BlockComplete){
            RemoveFixation();
            RemoveGhostTargetArray();
            agent.SetActive(false);
        }

        if (eventType == GameStatus.AllBlocksComplete){

        }

        if (eventType == GameStatus.WaitingForInput){

        }
    }

    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifeTime, int index, int total){
        targetIndex = targetNum;
        this.lifeTime = lifeTime;
        if (eventType == TrialEventType.TrialSequenceStarted){
            //no tNum yet
        }

        if (eventType == TrialEventType.PreTrialPhase){
            Debug.Log("Instantiate Target Objects");
            InitialiseObjects();
        }

        if (eventType == TrialEventType.Fixation){
            Debug.Log("Display Fixation");
            DisplayFixation();
        }

        if (eventType == TrialEventType.Indication){
            Debug.Log("Display Indication");
            // DisplayIndication();
        }

        if (eventType == TrialEventType.Observation){
            Debug.Log("Display Observation");
            DisplayIndication();
            DisplayObservation();
        }

        if (eventType == TrialEventType.TargetPresentation){
            Debug.Log("Present Target");
            DisplayTarget();
        }

        if (eventType == TrialEventType.Rest){
            Debug.Log("Start Target Removal");
            RemoveTarget();
        }

        if (eventType == TrialEventType.PostTrialPhase){
            Debug.Log("Destroy Target Objects");
            DestroyObjects();
        }

        if (eventType == TrialEventType.TrialComplete){

        }

        if (eventType == TrialEventType.TrialSequenceComplete){

        }
    }

    #endregion

    void Awake(){
        //ml
        //agent.SetActive(false);
        
        originPoint = this.transform;
        controller = gameObject.GetComponent<TargetController>();

        ghostHandRightMesh.material.DOFade(0, 0);
        
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineToTarget = false;

        targetHomeObject.transform.DOScale(0, 0);
        
        defaultColour = Settings.instance.defaultColour;
        highlightColour = Settings.instance.highlightColour;
        defaultFixationColour = Settings.instance.defaultFixationColour;
        highlightFixationColour = Settings.instance.highlightFixationColour;
    }

    private void Start(){
        if (DAO.instance != null){
            handPosition = DAO.instance.motionDataRightWrist.position;
        }
        InitialisePositions();
        
        //observation ml events
        if (OnTargetAction!=null){
            OnTargetAction(false, false, targetHomeObject.transform.position);
        }
    }

    private IEnumerator InitialisePositions(){
        yield return new WaitForEndOfFrame();

        ghostHandRight.transform.DOMove(handPosition, 0);
        lineDestinationTransform.position = handPosition;
    }

    private void Update(){
        if (DAO.instance != null){
            handPosition = DAO.instance.motionDataRightWrist.position;
        }
        // lineRenderer.SetPosition(0,originPoint.position);

        lineRenderer.SetPosition(0,handPosition);
        lineRenderer.SetPosition(1,lineDestinationTransform.position);
        
        if (lineToTarget){
            lineRenderer.SetPosition(0,handPosition);
            lineRenderer.SetPosition(1,lineDestinationTransform.position);
        }
        else{
            lineRenderer.SetPosition(0,handPosition);
            lineRenderer.SetPosition(1,handPosition);
        }

        #region Debugging controls

        // //debugging controls
        // if (Input.GetKeyDown(KeyCode.UpArrow)){
        //     InitialiseGhostTargetArray();
        //     InitialiseFixation();
        //     InitialiseObjects();
        //     lifeTime = 1f;
        //     targetsActive = true;
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow)){
        //     RemoveGhostTargetArray();
        //     targetsActive = false;
        // }
        //
        // if (targetsActive){
        //     if (Input.GetKeyDown(KeyCode.Alpha0)){
        //         RemoveTarget();
        //         DestroyObjects();
        //     }
        //     if (Input.GetKeyDown(KeyCode.Alpha1)){
        //         RemoveTarget();
        //         DestroyObjects();
        //         targetIndex = 0;
        //         InitialiseObjects();
        //         DisplayIndication();
        //         DisplayTarget();
        //     }
        //     if (Input.GetKeyDown(KeyCode.Alpha2)){
        //         RemoveTarget();
        //         DestroyObjects();
        //         targetIndex = 1;
        //         InitialiseObjects();
        //         DisplayIndication();
        //         DisplayTarget();
        //     }
        //     if (Input.GetKeyDown(KeyCode.Alpha3)){
        //         RemoveTarget();
        //         DestroyObjects();
        //         targetIndex = 2;
        //         InitialiseObjects();
        //         DisplayIndication();
        //         DisplayTarget();
        //     }
        //     if (Input.GetKeyDown(KeyCode.Alpha4)){
        //         RemoveTarget();
        //         DestroyObjects();
        //         targetIndex = 3;
        //         InitialiseObjects();
        //         DisplayIndication();
        //         DisplayTarget();
        //     }
        // }

        #endregion


    }

    private void InitialiseHomeTarget(){
        homeTargetRenderer = targetHomeObject.GetComponent<Renderer>();
        homeTargetRenderer.material.DOColor(highlightColour, 1f);
    }
    private void InitialiseFixation(){
        if (fixationCross == null){
            fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
            fixationCross.transform.position = originPoint.position;
            fixationRenderer = fixationCross.transform.Find("Cross").GetComponent<Renderer>();
            fixationCross.transform.DOScale(0, 0);
            fixationRenderer.material.DOFade(0, 0);
            fixationCross.SetActive(true);
        }
        
        fixationCross.transform.DOScale(0.5f, GameManager.instance.SpeedCheck(animationDuration));
        fixationRenderer.material.DOFade(1, GameManager.instance.SpeedCheck(animationDuration));
    }

    private void RemoveFixation(){
        fixationCross.transform.DOScale(0, GameManager.instance.SpeedCheck(animationDuration));
        fixationRenderer.material.DOFade(0, GameManager.instance.SpeedCheck(animationDuration));
    }
    private void InitialiseObjects(){
        targetDestination = GetDestination(targetIndex);
        Debug.Log(targetDestination);
        destinationTransform.position = targetDestination;
        
        target = Instantiate(targetPrefab, originPoint.position, quaternion.identity);
        target.transform.position = targetDestination;
        targetMesh = target.transform.Find("Target").gameObject;
        targetMesh.transform.DOScale(0, 0);
        
        targetRenderer = targetMesh.GetComponent<Renderer>();

        defaultColour = Settings.instance.defaultColour;
        highlightColour = Settings.instance.highlightColour;
        defaultFixationColour = Settings.instance.defaultFixationColour;
        highlightFixationColour = Settings.instance.highlightFixationColour;
        
        
        target.SetActive(false);
    }

    private void DisplayFixation(){

    }
    private void DisplayIndication()
    {
        //observation ml events
        if (OnTargetAction!=null){
            OnTargetAction(false, false, targetHomeObject.transform.position);
        }
        
        homeTargetRenderer.material.DOColor(defaultColour, lifeTime / 4);
        
        targetGhosts[targetIndex].transform.DOScale(0f, lifeTime/4);
        target.SetActive(true);
        targetMesh.transform.DOScale(0.75f, lifeTime/4);
        
        // ghostHandRightMesh.material.DOFade(0.05f, lifeTime);
        ghostHandRightMesh.material.DOFade(0, lifeTime);
        ghostHandRight.transform.DOMove(handPosition, lifeTime/2);
    }
    private void DisplayObservation()
    {
        // lineDestinationTransform.DOMove(destinationTransform.position, lifeTime / 2);
        // ghostHandRightMesh.material.DOFade(0.25f, lifeTime);
        // ghostHandRight.transform.DOMove(destinationTransform.position, lifeTime);

    }
    private void DisplayTarget()
    {
        //observation ml events
        if (OnTargetAction!=null){
            OnTargetAction(true, false, targetDestination);
        }
        SetMLAgentGoal(targetDestination, false);
        
        //everything in target period
        gameObject.GetComponent<AudioSource>().Play();
        if (useLine){
            lineToTarget = true;
        }
        lineDestinationTransform.position = handPosition;
        lineDestinationTransform.DOMove(destinationTransform.position, lifeTime / 4);
        ghostHandRightMesh.material.DOFade(0.2f, lifeTime);
        ghostHandRight.transform.DOMove(destinationTransform.position, lifeTime);

        //homeTargetRenderer.material.DOColor(defaultColour, lifeTime / 4);
        
        targetMesh.transform.DOScale(1, lifeTime/4);
        targetRenderer.material.DOColor(highlightColour, lifeTime / 4);

    }
    private void RemoveTarget()
    {
        //observation ml events
        if (OnTargetAction!=null){
            OnTargetAction(false, true, targetHomeObject.transform.position);
        }
        SetMLAgentGoal(targetHomeObject.transform.position, true);
        //SetMLAgentGoal(fixationCross.transform.position);
        
        // ghostHandRightMesh.material.DOFade(0.05f, lifeTime/4);
        
        homeTargetRenderer.material.DOColor(highlightColour, lifeTime / 4);
        
        targetMesh.transform.DOScale(0f, lifeTime/4);
        targetRenderer.material.DOColor(defaultColour, lifeTime / 4);
        
        targetGhosts[targetIndex].transform.DOScale(0.75f, lifeTime/4);
        // fixationRenderer.material.DOFade(0,lifeTime/4);
        
        ghostHandRightMesh.material.DOFade(0, lifeTime/4);
        ghostHandRight.transform.DOMove(handPosition, lifeTime/4);
        
        lineDestinationTransform.DOMove(handPosition, lifeTime/4);
        lineToTarget = false;
    }
    private void DestroyObjects()
    {
        target.SetActive(false);
        if (target != null)
        {
            Destroy(target);
        }
    }
    
    private void InitialiseGhostTargetArray(){
        if (targetGhosts.Length == 0){
            targetGhosts = new GameObject[GameManager.instance.targetCount];
            for (int i = 0; i < targetGhosts.Length; i++){
                targetGhosts[i] = Instantiate(targetGhostPrefab, originPoint.position, quaternion.identity);
                targetGhosts[i].transform.DOScale(0, 0);
            }
        }
        
        for (int i = 0; i < targetGhosts.Length; i++){
            targetGhosts[i].transform.DOScale(0, 0);
            targetGhosts[i].transform.DOScale(0.75f, GameManager.instance.SpeedCheck(animationDuration));
            targetGhosts[i].transform.DOMove(GetDestination(i), GameManager.instance.SpeedCheck(animationDuration));
        }
        
        targetHomeObject.transform.DOScale(0.05f, GameManager.instance.SpeedCheck(animationDuration));
    }
    private void RemoveGhostTargetArray(){
        for (int i = 0; i < targetGhosts.Length; i++){
            targetGhosts[i].transform.DOScale(0, GameManager.instance.SpeedCheck(animationDuration));
            targetGhosts[i].transform.DOMove(originPoint.transform.position, GameManager.instance.SpeedCheck(animationDuration));
        }
        
        targetHomeObject.transform.DOScale(0.0f, GameManager.instance.SpeedCheck(animationDuration));
        homeTargetRenderer.material.DOColor(defaultColour, lifeTime / 4);
        
        //observation ml events
        if (OnTargetAction!=null){
            OnTargetAction(false, false, targetHomeObject.transform.position);
        }
    }
    
    private Vector3 GetDestination(int tNum)
    {
        Vector3 dest = Vector3.zero;
        switch (tNum) {
            // case 0:
            //     targetDestination = new Vector3(originPoint.position.x, originPoint.position.y, originPoint.position.z);
            //     break;
            case 0:
                targetDestination = new Vector3(originPoint.position.x - targetDistance, originPoint.position.y, originPoint.position.z);
                break;
            case 1:
                targetDestination = new Vector3(originPoint.position.x, originPoint.position.y + targetDistance, originPoint.position.z);
                break;
            case 2:
                targetDestination = new Vector3(originPoint.position.x + targetDistance, originPoint.position.y, originPoint.position.z);
                break;
            case 3:
                targetDestination = new Vector3(originPoint.position.x, originPoint.position.y-targetDistance, originPoint.position.z);
                break;
        }

        // if (randomisePosition)
        // {
        //     //set a random position based on an offset  
        // }

        dest = targetDestination;
        
        return dest;
    }

    private void SetMLAgentGoal(Vector3 goalPosition, bool home){
        //position a trigger at the target point
        agentGoal.transform.position = goalPosition;
        agentGoal.GetComponent<AgentReward>().SetCollider(true);
        if (!home){
            agentGoal.GetComponent<AgentReward>().homePosition = false;
        }
        else{
            agentGoal.GetComponent<AgentReward>().homePosition = true;
        }
    }
}
