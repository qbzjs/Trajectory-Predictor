using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Leap.Unity;

public class InstructionText : MonoBehaviour
{
    private UserInputType userInputType = UserInputType.Start;
    
    public TextMeshPro instructionTextActual;
    public TextMeshPro instructionTextImagined;
    public TextMeshPro instructionTextBlockComplete;
    public TextMeshPro instructionTextSessionComplete;

    public TextMeshPro bigInstruction;
    
    private RunType runType;
    private RunType currentRunType; // for saving run type as index increments when a run is started

    private Color UI_Orange;
    private Color UI_Blue;
    private Color color;

    //avatar
    public InstructionAvatar avatar;
    private string txt = "";
    private GameObject tempTextObj;

    private void Awake(){
        //RemoveText();
        instructionTextActual.gameObject.SetActive(false);
        instructionTextImagined.gameObject.SetActive(false);
        instructionTextBlockComplete.gameObject.SetActive(false);
        instructionTextSessionComplete.gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        avatar.DisableAvatar(false);
    }

    private void Start(){
        runType = Settings.instance.GetRunType(0); //get run type for first index 
        
        UI_Orange = Settings.instance.UI_Orange;
        UI_Blue = Settings.instance.UI_Blue;
        
        UI_Orange = Settings.instance.highlightIndicatorColour;
        UI_Blue = Settings.instance.highlightFixationColour;
        
        bigInstruction.DOFade(0, 0);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            RemoveText();
        }
        if (Input.GetKeyDown(KeyCode.H)){
            RemoveText();
        }
    }

    #region Subscriptions

    private void OnEnable(){
        InputManager.OnUserInputAction += InputManagerOnUserInputAction;
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnRunAction += GameManagerOnRunAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }



    private void OnDisable(){
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnRunAction -= GameManagerOnRunAction;
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    
    private void InputManagerOnUserInputAction(UserInputType inputType){
        if (userInputType == UserInputType.Start){
            RemoveText();
        }
    }
    
    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            DisplayNewRunText();
        }
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal, RunType runType){
        this.runType = runType;
//        Debug.Log(runIndex);
        if (eventType == GameStatus.RunComplete){
            DisplayNewRunText();
        }
        if (eventType == GameStatus.AllRunsComplete){
            RemoveText();
            instructionTextSessionComplete.gameObject.SetActive(true);
        }
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.BlockComplete){
            DisplayNewBlockText();
        }
        if (eventType == GameStatus.Countdown){
            DisplayBigInstructionText();
        }

        if (eventType == GameStatus.BlockStarted){
            bigInstruction.DOFade(0, 1f);
        }
    }

    #endregion


    void DisplayNewBlockText(){
        bigInstruction.DOFade(0, 1f);
        
        if (GameManager.instance.blockIndex != GameManager.instance.blockTotal){
            instructionTextBlockComplete.gameObject.SetActive(true);
            
            //gameObject.GetComponent<BoxCollider>().enabled = true;
            BoxCollider c = gameObject.GetComponent<BoxCollider>();
            StartCoroutine(WaitAndEnable(c));

            //TODO - NEW
            txt = instructionTextBlockComplete.gameObject.GetComponent<TextMeshPro>().text;
            tempTextObj = instructionTextBlockComplete.gameObject;
            color = Color.white;
        }
    }
    
    //using odd even 
    // void DisplayNewRunText()
    // {
    //     if((GameManager.instance.runIndex+1)%2 == 0)
    //     {
    //         Debug.Log("EVEN--------------------");
    //         instructionTextActual.gameObject.SetActive(false);
    //         instructionTextImagined.gameObject.SetActive(true);
    //         gameObject.GetComponent<BoxCollider>().enabled = true;
    //     }
    //     else
    //     {
    //         Debug.Log("ODD--------------------");
    //         instructionTextActual.gameObject.SetActive(true);
    //         instructionTextImagined.gameObject.SetActive(false);
    //         gameObject.GetComponent<BoxCollider>().enabled = true;
    //     }
    // }

    //using enums from settings sent from game manager
    
    void DisplayNewRunText(){
        currentRunType = runType;
        bigInstruction.DOFade(0, 1f);
        
        if(runType == RunType.Imagined)
        {
//            Debug.Log(" Instructions - RUN TYPE : "+ runType);
            instructionTextActual.gameObject.SetActive(false);
            instructionTextImagined.gameObject.SetActive(true);
            // gameObject.GetComponent<BoxCollider>().enabled = true;
            BoxCollider c = gameObject.GetComponent<BoxCollider>();
            StartCoroutine(WaitAndEnable(c));
            
            //TODO - NEW
            txt = instructionTextImagined.gameObject.GetComponent<TextMeshPro>().text;
            tempTextObj = instructionTextImagined.gameObject;
            color = UI_Blue;
        }
        if(runType == RunType.Kinematic)
        {
 //           Debug.Log(" Instructions - RUN TYPE : "+ runType);
            instructionTextActual.gameObject.SetActive(true);
            instructionTextImagined.gameObject.SetActive(false);
            // gameObject.GetComponent<BoxCollider>().enabled = true;
            BoxCollider c = gameObject.GetComponent<BoxCollider>();
            StartCoroutine(WaitAndEnable(c));
            
            //TODO - NEW
            txt = instructionTextActual.gameObject.GetComponent<TextMeshPro>().text;
            tempTextObj = instructionTextActual.gameObject;
            color = UI_Orange;
        }
      
    }
    
    private IEnumerator WaitAndEnable(Collider c){
        yield return new WaitForSeconds(1f);
        avatar.EnableAvatar();
        yield return new WaitForSeconds(1f);
        avatar.SetText(tempTextObj.GetComponent<TextMeshPro>().text, color);
        
        yield return new WaitForSeconds(2f);
        c.enabled = true;
    }
    
    void DisplayBigInstructionText(){
        if(currentRunType == RunType.Imagined){
//            Debug.Log("BIG Instructions - RUN TYPE : "+ runType);
            bigInstruction.text = "Imagined Arm" + "\n" + "Movement";
            bigInstruction.color = UI_Blue;
            bigInstruction.DOFade(0, 0);
            bigInstruction.DOFade(1, 2f);
        }
        if(currentRunType == RunType.Kinematic){
 //           Debug.Log("BIG Instructions - RUN TYPE : "+ runType);
            bigInstruction.text = "Real Arm" + "\n" + "Movement";
            bigInstruction.color = UI_Orange;
            bigInstruction.DOFade(0, 0);
            bigInstruction.DOFade(1, 2f);
        }
        
    }

    public void RemoveText(){
        GetComponent<AudioSource>().Play();
        instructionTextActual.gameObject.SetActive(false);
        instructionTextImagined.gameObject.SetActive(false);
        instructionTextBlockComplete.gameObject.SetActive(false);
        instructionTextSessionComplete.gameObject.SetActive(false);

        gameObject.GetComponent<BoxCollider>().enabled = false;

        avatar.DisableAvatar();
    }
}
