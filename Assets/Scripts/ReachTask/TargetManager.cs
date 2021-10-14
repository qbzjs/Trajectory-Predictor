using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Enums;
using UnityEngine;
using PathologicalGames;
using DG.Tweening;
using Leap.Unity;

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

    public GameObject ghostHandRight;
    public Renderer ghostHandRightMesh;
    
    private Renderer targetRenderer;
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

    private LineRenderer lineRenderer;

    public int targetIndex;
    public float lifeTime;

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

        }

        if (eventType == GameStatus.CountdownComplete){

        }

        if (eventType == GameStatus.BlockStarted){

        }

        if (eventType == GameStatus.BlockComplete){
            RemoveFixation();
            RemoveGhostTargetArray();
        }

        if (eventType == GameStatus.AllBlocksComplete){

        }

        if (eventType == GameStatus.WaitingForInput){

        }
    }

    private void GameManagerOnTrialAction(TrialEventType eventType, int targetNum, float lifeTime, int index,
        int total){
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
        originPoint = this.transform;
        controller = gameObject.GetComponent<TargetController>();

        ghostHandRightMesh.material.DOFade(0, 0);
        ghostHandRight.transform.DOMove(handPosition, 0);

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
        fixationCross.transform.position = originPoint.position;
        fixationRenderer = fixationCross.transform.Find("Cross").GetComponent<Renderer>();
        fixationCross.transform.DOScale(0, 0);
        fixationRenderer.material.DOFade(0, 0);
    }

    private void Update(){
        if (DAO.instance != null){
            handPosition = DAO.instance.motionDataRightWrist.position;
        }
        lineRenderer.SetPosition(0,originPoint.position);
        lineRenderer.SetPosition(1,lineDestinationTransform.position);
        
    }

    private void InitialiseFixation(){
        // if (fixationCross == null){
        //     fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
        // }
        
        fixationCross.transform.position = originPoint.position;
        //fixation on initialise
        // fixationRenderer.material.DOColor(defaultFixationColour,0);
        fixationCross.SetActive(true);
        fixationCross.transform.DOScale(0, 0);
        fixationRenderer.material.DOFade(0, 0);
        fixationCross.transform.DOScale(0.5f, GameManager.instance.SpeedCheck(animationDuration));
        fixationRenderer.material.DOFade(1, GameManager.instance.SpeedCheck(animationDuration));
    }

    private void RemoveFixation(){
        fixationCross.transform.DOScale(0, GameManager.instance.SpeedCheck(animationDuration));
    }
    private void InitialiseObjects(){
        targetDestination = GetDestination(targetIndex);
        destinationTransform.position = targetDestination;
        
        // fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
        // fixationCross.transform.position = originPoint.position;
        
        // indicatior = Instantiate(indicatorPrefab, originPoint.position, quaternion.identity);
        // indicatior.transform.position = originPoint.position;
        
        // SmoothLookAtConstraint look = indicatior.GetComponent<SmoothLookAtConstraint>();
        // look.target = destinationTransform.transform;
        
        target = Instantiate(targetPrefab, originPoint.position, quaternion.identity);
        target.transform.position = targetDestination;
        targetMesh = target.transform.Find("Target").gameObject;
        targetMesh.transform.DOScale(0, 0);
        
        targetRenderer = targetMesh.GetComponent<Renderer>();

        defaultColour = Settings.instance.defaultColour;
        highlightColour = Settings.instance.highlightColour;
        defaultFixationColour = Settings.instance.defaultFixationColour;
        highlightFixationColour = Settings.instance.highlightFixationColour;
        // defaultIndicatorColour = Settings.instance.defaultIndicatorColour;
        // highlightIndicatorColour = Settings.instance.highlightIndicatorColour;
        

        target.SetActive(false);
        

    }

    private void DisplayFixation(){
        // fixationRenderer.material.DOColor(defaultFixationColour,0);
        fixationCross.SetActive(true);
        fixationCross.transform.DOScale(0.5f, lifeTime/4);
        // fixationRenderer.material.DOColor(highlightFixationColour,lifeTime/4);
    }
    private void DisplayIndication()
    {
        // fixationCross.SetActive(false);
        // fixationRenderer.material.DOColor(defaultFixationColour,lifeTime/4);
        targetGhosts[targetIndex].transform.DOScale(0f, lifeTime/4);
        target.SetActive(true);
        targetMesh.transform.DOScale(0.5f, lifeTime/4);

        

    }
    private void DisplayObservation()
    {
        lineDestinationTransform.DOMove(destinationTransform.position, lifeTime / 2);
        ghostHandRightMesh.material.DOFade(0.25f, lifeTime);
        ghostHandRight.transform.DOMove(destinationTransform.position, lifeTime);

    }
    private void DisplayTarget()
    {
        ghostHandRightMesh.material.DOFade(0.05f, lifeTime/4);
        fixationCross.transform.DOScale(0.5f, lifeTime/4);
        targetMesh.transform.DOScale(1, lifeTime/4);
        targetRenderer.material.DOColor(highlightColour, lifeTime / 4);

    }
    private void RemoveTarget()
    {
        targetMesh.transform.DOScale(0f, lifeTime/4);
        targetRenderer.material.DOColor(defaultColour, lifeTime / 4);
        targetGhosts[targetIndex].transform.DOScale(0.5f, lifeTime/4);
        fixationRenderer.material.DOFade(0,lifeTime/4);
        
        ghostHandRightMesh.material.DOFade(0, lifeTime/4);
        ghostHandRight.transform.DOMove(handPosition, lifeTime / 2);
        
        lineDestinationTransform.DOMove(originPoint.position, lifeTime / 4);
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
            targetGhosts[i].transform.DOScale(0.5f, GameManager.instance.SpeedCheck(animationDuration));
            targetGhosts[i].transform.DOMove(GetDestination(i), GameManager.instance.SpeedCheck(animationDuration));
        }
    }
    private void RemoveGhostTargetArray(){
        for (int i = 0; i < targetGhosts.Length; i++){
            targetGhosts[i].transform.DOScale(0, GameManager.instance.SpeedCheck(animationDuration));
            targetGhosts[i].transform.DOMove(originPoint.transform.position, GameManager.instance.SpeedCheck(animationDuration));
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
}
