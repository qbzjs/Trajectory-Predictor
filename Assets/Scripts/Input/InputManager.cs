using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

/// <summary>
/// All input routed through this class
/// some inputs are conditional depending on the scene loaded
/// </summary>
///

public class InputManager : MonoBehaviour{
	
	public static InputManager instance;

	public bool debugInput = false;
	
	public bool inputEnabled = true;
	public bool trialReady;
	public delegate void UserInputAction(UserInputType inputType);
	public static event UserInputAction OnUserInputAction;

	void Awake(){
		instance = this;
	}

	private void Start(){
		trialReady = true;
	}

	#region Subscriptions

	private void OnEnable() {
		SenselTap.OnUserInputAction+= SenselTapOnUserInputAction;
		GameManager.OnProgressAction += GameManagerOnProgressAction;
	}
	private void OnDisable() {
		SenselTap.OnUserInputAction -= SenselTapOnUserInputAction;
		GameManager.OnProgressAction -= GameManagerOnProgressAction;
	}
	private void GameManagerOnProgressAction(GameStatus eventType, float completionPercentage, int run, int runTotal, int block, int blockTotal, int trial, int trialTotal){
		if (eventType == GameStatus.Ready){
			trialReady = true;
		}
		else{
			trialReady = false;
		}
		// if (eventType == GameStatus.BlockComplete){
		// 	trialReady = true;
		// }
	}
	
	#endregion


	#region Inputs

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartTrials();
		}
		if (Input.GetKeyDown(KeyCode.P)){
			PauseGame();
		}
		if (Input.GetKeyDown(KeyCode.R)){
			ResetSession();
		}
		
	}
	private void SenselTapOnUserInputAction(UserInputType iType, float x, float y, float f)
	{
		if (debugInput){
			Debug.Log("Sensel Input ---> " + "X: " + x + " : " + "Y: " + y + "F: " + f);
		}
		StartTrials();
	}

	#endregion

	#region Input Actions

	public void StartTrials(){
		if (GameManager.instance.gameStatus != GameStatus.RunningTrials){
			if (inputEnabled){
				if (OnUserInputAction != null){
					OnUserInputAction(UserInputType.Start);
				}
			}
		}
	}

	public void StopTrial(){
		if (inputEnabled){
			if (OnUserInputAction != null){
				OnUserInputAction(UserInputType.Stop);
			}
		}
	}
	
	public void PauseGame(){
		if (inputEnabled){
			if (OnUserInputAction != null){
				OnUserInputAction(UserInputType.Pause);
			}
		}
	}
	public void ResetSession(){
		if (inputEnabled){
			if (OnUserInputAction != null){
				OnUserInputAction(UserInputType.Reset);
			}
		}
	}

	#endregion


	//remember this is record data start
	//OnRecordAction(t, System.Guid.NewGuid().ToString());
}