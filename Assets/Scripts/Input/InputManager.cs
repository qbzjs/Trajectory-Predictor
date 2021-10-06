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

	//needs to be somewhere else
	public delegate void UserInputAction(UserInputType inputType);
	public static event UserInputAction OnUserInputAction;

	void Awake(){
		instance = this;
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Space)){
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
		}
	}

	public void StartBlock(){
		if (OnUserInputAction != null){
			OnUserInputAction(UserInputType.Start);
		}
	}

	public void StopBlock(){
		if (OnUserInputAction != null){
			OnUserInputAction(UserInputType.Stop);
		}
	}

	//remember this is record data start
	//OnRecordAction(t, System.Guid.NewGuid().ToString());
}