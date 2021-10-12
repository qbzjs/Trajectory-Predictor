using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_DisplayObjects : MonoBehaviour {

	[Header("Objects")]
	public GameObject TrialUI;
	public GameObject settingsPanel;
	public GameObject trial2D;
	public GameObject trial2D_RenderTexture;
	public GameObject trialCameraScreen2D;

	[Space(10)]
	[Header("UI Objects")]
	public GameObject controlCharacter_C;
	public GameObject controlCharacter_L;
	public GameObject controlCharacter_T;
	public GameObject controlCharacter_R;
	public GameObject controlCharacter_B;
	public GameObject[] labels = new GameObject[3];
	public GameObject scoreDisplay;
	public GameObject statusDisplay;
	public GameObject progressDisplayRun;
	public GameObject progressDisplayBlock;
	public GameObject progressDisplayTrial;
	public GameObject countdownDisplay;
	public GameObject FPS;
	
	//----REDACTED FROM v2
	// public GameObject rest;
	// public GameObject outerLines;
	// public GameObject innerLinesA;
	// public GameObject innerLinesB;
	// public GameObject ringOuter;


	
	// public GameObject environment3D;
	// public GameObject interface3D;
	// public GameObject renderTexture2D;
	// public GameObject actionObservation;
	// public GameObject recordTrajectory;
	// public GameObject particleFX;
	// public GameObject colourLerp;
	// public GameObject vibration;
	// public GameObject audio;
	// public GameObject spatialAudio;

	
	
}
