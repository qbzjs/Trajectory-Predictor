using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using DG.Tweening;

/// <summary>
/// scale the button on each trial finish
/// colour the button on press..
/// </summary>
public class VRStartButtonLogic : MonoBehaviour
{
    public GameObject startButton;
    public GameObject buttonPlinth;
    private float initialHeight;
    
    public Material btnDefault;
    public Material btnPressed;

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
        if (inputType == UserInputType.Start){
            print("REMOVE BUTTON....");
            buttonPlinth.transform.DOScaleY(0, 1f);
            startButton.GetComponent<Renderer>().material = btnPressed;
        }
    }
    
    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            print("DISPLAY BUTTON game....");
            buttonPlinth.transform.DOScaleY(initialHeight, 2f);
            startButton.GetComponent<Renderer>().material = btnDefault;
        }
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal, RunType runType){
        if (eventType == GameStatus.RunComplete){
            print("DISPLAY BUTTON run....");
            buttonPlinth.transform.DOScaleY(initialHeight, 2f);
            startButton.GetComponent<Renderer>().material = btnDefault;
        }
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.BlockComplete){
            print("DISPLAY BUTTON block....");
            buttonPlinth.transform.DOScaleY(initialHeight, 2f);
            startButton.GetComponent<Renderer>().material = btnDefault;
        }
    }

    #endregion
    
    
    void Awake(){
        initialHeight = buttonPlinth.transform.localScale.y;
        buttonPlinth.transform.localScale = new Vector3(buttonPlinth.transform.localScale.x, 0, buttonPlinth.transform.localScale.z);
    }
    
    void Update()
    {
        
    }
}
