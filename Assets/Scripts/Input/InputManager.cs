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
	public delegate void UserInputAction(UserInputType inputType);
	public static event UserInputAction OnUserInputAction;

	void Awake(){
		instance = this;
	}

	#region Subscriptions

	private void OnEnable() {
		SenselTap.OnUserInputAction+= SenselTapOnUserInputAction;
	}
	private void OnDisable() {
		SenselTap.OnUserInputAction+= SenselTapOnUserInputAction;
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
		if (inputEnabled){
			if (OnUserInputAction != null){
				OnUserInputAction(UserInputType.Start);
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