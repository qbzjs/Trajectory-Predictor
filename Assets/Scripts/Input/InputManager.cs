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

	public bool allowInput = true;
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
			StartTrial();
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			
		}
	}
	private void SenselTapOnUserInputAction(UserInputType iType, float x, float y, float f)
	{
		Debug.Log("Sensel Input ---> " + "X: " + x + " : " + "Y: " + y + "F: " + f);
		StartTrial();
	}

	#endregion

	#region Actions

	public void StartTrial(){
		Debug.Log("User Pressed Start: Start Event Broadcast");
		if (OnUserInputAction != null){
			OnUserInputAction(UserInputType.Start);
		}
	}

	public void StopBlock(){
		if (OnUserInputAction != null){
			OnUserInputAction(UserInputType.Stop);
		}
	}

	#endregion


	//remember this is record data start
	//OnRecordAction(t, System.Guid.NewGuid().ToString());
}