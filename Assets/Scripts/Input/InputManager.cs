using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
/// <summary>
/// All input routed through this class
/// some inputs are conditional depending on the scene loaded
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public delegate void RecordAction(bool t, string id);
    public static event RecordAction OnRecordAction;
    
    private void OnEnable(){
        GamePadInput.OnGamePad_A += GamePadInput_OnGamePad_A;
        GamePadInput.OnGamePad_B += GamePadInput_OnGamePad_B;
        GamePadInput.OnGamePad_X += GamePadInput_OnGamePad_X;
        GamePadInput.OnGamePad_Y += GamePadInput_OnGamePad_Y;
    }
    private void OnDisable(){
        GamePadInput.OnGamePad_A -= GamePadInput_OnGamePad_A;
        GamePadInput.OnGamePad_B -= GamePadInput_OnGamePad_B;
        GamePadInput.OnGamePad_X -= GamePadInput_OnGamePad_X;
        GamePadInput.OnGamePad_Y -= GamePadInput_OnGamePad_Y;
    }

    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTrajectoryRecording(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetTrajectoryRecording(false);
        }
    }
    private void GamePadInput_OnGamePad_A(UserInput b)
    {
        //START RECORDING DATA
    }
    private void GamePadInput_OnGamePad_B(UserInput b)
    {

    }
    private void GamePadInput_OnGamePad_X(UserInput b)
    {

    }
    private void GamePadInput_OnGamePad_Y(UserInput b)
    {

    }
    public void StartTrialSequence()
    {
        TrialSequence.instance.StartTrial();
    }
    public void StopTrialSequence()
    {
        TrialSequence.instance.StopTrial();
    }
    public void SetTrajectoryRecording(bool t){
        if (OnRecordAction != null)
        {
            OnRecordAction(t, System.Guid.NewGuid().ToString());
        }
    }
}
