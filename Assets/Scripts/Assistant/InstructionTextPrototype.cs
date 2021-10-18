using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using TMPro;

public class InstructionTextPrototype : MonoBehaviour
{
    public Color actualColour;
    public Color imaginedColour;
    public TextMeshPro instructionTextActual;
    public TextMeshPro instructionTextImagined;

    private void OnEnable(){
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnRunAction += GameManagerOnRunAction;
    }

    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            DisplayText();
        }
    }

    private void OnDisable(){
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnRunAction -= GameManagerOnRunAction;
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal){
        if (eventType == GameStatus.RunComplete){
            DisplayText();
        }
    }
    

    void Start()
    {
        
    }
    
    void DisplayText()
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
        instructionTextActual.gameObject.SetActive(false);
        instructionTextImagined.gameObject.SetActive(false);
    }
}
