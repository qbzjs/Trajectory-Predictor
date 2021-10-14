using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Enums;
using UnityEngine;
using PathologicalGames;

public class TargetManager : MonoBehaviour
{
    private TargetController controller;
    
    public GameObject targetGhostPrefab;
    private GameObject[] targetGhosts = new GameObject[0];
    public GameObject targetPrefab;
    private GameObject target;
    public GameObject fixationCrossPrefab;
    private GameObject fixationCross;
    public GameObject indicatorPrefab;
    private GameObject indication;

    public Transform originPoint;
    [Range(0.05f,0.25f)]
    public float targetDistance = 0.125f;
    
    public Vector3 offsetFromOrigin;
    public Vector3 targetDestination;
    public Transform destinationTransform;
    
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
            InitialiseTargetArray();
        }

        if (eventType == GameStatus.VisibleCountdown){

        }

        if (eventType == GameStatus.CountdownComplete){

        }

        if (eventType == GameStatus.BlockStarted){

        }

        if (eventType == GameStatus.BlockComplete){

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
            DisplayIndication();
        }

        if (eventType == TrialEventType.Observation){
            Debug.Log("Display Observation");
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
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha0)){
            Debug.Log("target centre");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            Debug.Log("target left");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("target top");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)){
            Debug.Log("target right");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)){
            Debug.Log("target bottom");
        }
    }

    private void InitialiseObjects(){
        targetDestination = GetDestination(targetIndex);
        destinationTransform.position = targetDestination;
        
        fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
        fixationCross.transform.position = originPoint.position;
        
        indication = Instantiate(indicatorPrefab, originPoint.position, quaternion.identity);
        indication.transform.position = originPoint.position;
        
        SmoothLookAtConstraint look = indication.GetComponent<SmoothLookAtConstraint>();
        look.target = destinationTransform.transform;
        
        target = Instantiate(targetPrefab, originPoint.position, quaternion.identity);
        target.transform.position = targetDestination;
        
        fixationCross.SetActive(false);
        indication.SetActive(false);
        target.SetActive(false);
    }

    private void DisplayFixation(){
        fixationCross.SetActive(true);
    }
    private void DisplayIndication()
    {
        fixationCross.SetActive(false);
        indication.SetActive(true);
    }
    private void DisplayObservation()
    {
        
    }
    private void DisplayTarget()
    {
        indication.SetActive(false);
        target.SetActive(true);
    }
    private void RemoveTarget()
    {
        target.SetActive(false);
    }
    private void DestroyObjects()
    {
        if (target != null)
        {
            Destroy(target);
            Destroy(indication);
            Destroy(fixationCross);
        }
    }

    private void InitialiseTargetArray(){
        targetGhosts = new GameObject[GameManager.instance.targetCount];
        for (int i = 0; i < targetGhosts.Length; i++){
            targetGhosts[i] = Instantiate(targetGhostPrefab, originPoint.position, quaternion.identity);
            //scale to zero
            //animate to destination
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
