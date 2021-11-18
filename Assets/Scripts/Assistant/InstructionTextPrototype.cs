using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;

public class InstructionTextPrototype : MonoBehaviour
{
    public TextMeshPro instructionTextActual;
    public TextMeshPro instructionTextImagined;
    public TextMeshPro instructionTextBlockComplete;
    public TextMeshPro instructionTextSessionComplete;

    private void Awake(){
        RemoveText();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            RemoveText();
        }
        if (Input.GetKeyDown(KeyCode.H)){
            RemoveText();
        }
    }

    private void OnEnable(){
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnRunAction += GameManagerOnRunAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }

    private void OnDisable(){
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnRunAction -= GameManagerOnRunAction;
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            DisplayNewRunText();
        }
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal){
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
    }

    void DisplayNewBlockText(){
        if (GameManager.instance.blockIndex != GameManager.instance.blockTotal){
            instructionTextBlockComplete.gameObject.SetActive(true);
        }
    }
    
    void DisplayNewRunText()
    {
        if((GameManager.instance.runIndex+1)%2 == 0)
        {
            Debug.Log("EVEN--------------------");
            instructionTextActual.gameObject.SetActive(false);
            instructionTextImagined.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("ODD--------------------");
            instructionTextActual.gameObject.SetActive(true);
            instructionTextImagined.gameObject.SetActive(false);
        }
    }

    public void RemoveText(){
        GetComponent<AudioSource>().Play();
        instructionTextActual.gameObject.SetActive(false);
        instructionTextImagined.gameObject.SetActive(false);
        instructionTextBlockComplete.gameObject.SetActive(false);
        instructionTextSessionComplete.gameObject.SetActive(false);
        
        // instructionTextActual.tr
    }
}
