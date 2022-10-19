﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using SaveSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Valve.VR;

public class Settings : MonoBehaviour {

	public static Settings instance;

	public bool active;

	[Header("SESSION SETTINGS --SAVED TO FILE-- ")]
	public SettingsDataObject settingsData;
	
	[Header("USER SESSION SETTINGS")] 
	public string sessionName = "Session_";
	public int sessionNumber = 1;
	
	[Header("PARADIGM SETTINGS")]
	[Header("---------SET BY INTERFACE--------------")]
	// public TrialParadigm trialParadigm = TrialParadigm.Avatar3D;
	public TrialParadigm trialParadigm = TrialParadigm.Horizontal;
	public ParadigmTargetCount paradigmTargetCount = ParadigmTargetCount.Two;
	
	public int numberOfTargets = 0; //read only
	public int trialsPerBlock = 0; //read only
	public int trialsPerRun = 0; //read only
	public int trialsPerSession = 0; //read only
	
	public float estTrialDuration = 0;
	public float estBlockDuration = 0; //read only
	public float estRunDuration = 0; //read only
	public float estSessionDuration = 0; //read only
	
	[Range(1, 50)] 
	public int repetitions = 5; // num of sequences to run
	public SequenceType sequenceType = SequenceType.Permutation;
	public Handedness handedness = Handedness.Left;

	[Range(0, 100)] 
	public float BCI_ControlAssistance; //serialise from bci signal for now -TODO link from settings ui and set for bci controller
	[Range(0, 20)] 
	public float assistanceModifier;
	[Range(0, 4)] 
	public float controlMagnitude;
	[Range(0, 4)] 
	public float controlMagnitudeX;
	[Range(0, 4)] 
	public float controlMagnitudeY;
	[Range(0, 4)] 
	public float controlMagnitudeZ;
	[Range(0, 1)] 
	public float smoothingSpeed;
	
	[Header("SESSION SETTINGS")] 
	
	public RunType[] runSequence = new RunType[8];
	public RunType currentRunType; //set at beginning of a run
	
	[Header("---------SET BY INTERFACE--------------")]
	[Range(1, 8)] 
	public int sessionRuns = 2;
	[Range(1, 9)] 
	public int blocksPerRun = 4;
	[Range(0, 120)] 
	public int preBlockCountdown = 10;
	[Range(0, 60)] 
	public int visibleCountdown = 5;
	[Range(10, 60)] 
	public int interRunRestPeriod = 10;


	[Header("TRIAL SETTINGS")] 
	[Range(-5, 5)]
	public float gameSpeedModifier = 0;
	[Range(0, 5)] 
	public float preTrialWaitPeriod = 1f;
	[Range(0, 5)] 
	public float fixationPeriod = 2f;
	[Range(0, 5)] 
	public float indicationPeriod = 2f;
	[Range(1, 10)] 
	public float observationPeriod = 2f; //observation time is the same as target time (ghost hand movement)
	[Range(1, 10)] 
	public float targetPresentationPeriod = 2f;

	[Range(2, 10)] 
	public float restPeriodMin = 2;
	[Range(2, 10)] 
	public float restPeriodMax = 2;
	
	[Range(0, 5)] 
	public float postTrialWaitPeriod = 1;
	[Range(0, 5)] 
	public float postBlockWaitPeriod = 1;
	[Range(0, 5)] 
	public float postRunWaitPeriod = 1;
	
	[Header("ADDITIONAL SETTINGS")] 
	public bool actionObservation = false;
	public bool recordMotionData = false;
	public SampleRate sampleRate = SampleRate.Hz60;

	[Header("STATUS")] 
	public GameStatus gameStatus = GameStatus.Ready;

	[Header("ENVIRONMENT")] 
	public bool environment3D = true;
	
	[Header("INTERFACE 3D")]
	public bool interface3D = false;
	public bool display2D = false;

	[Header("INTERFACE")]
	public bool labelsVisible = true;
	public bool restVisible = false;
	public bool displayScore = false;
	public bool displayStatus = true;
	public bool displayProgress = true;
	public bool displayCountdown = true;
	public bool showFramerate = true;
	public bool linesOuter = true;
	public bool linesInnerA = false;
	public bool linesInnerB = true;
	public bool ringOuter = false;

	[Header("FEEDBACK")]
	public bool animateTargets = false;

	//string folderName = "MotionData" + "_" + Settings.instance.sessionName + "_Trial_" + Settings.instance.sessionNumber.ToString();
	
	[Header("CHARACTER")]
	public CharacterColourType characterColourType = CharacterColourType.Static;
	public bool characterVisible_C = true;
	public bool characterVisible_L = true;
	public bool characterVisible_T = true;
	public bool characterVisible_R = true;
	public bool characterVisible_B = true;
	public bool colourSmoothing = true;
	public int colourSpeed = 15;
	public bool characterSmoothing = true;
	public int smoothSpeed = 15;
	
	[Header("PORTS")]
	public string ip = "127.0.0.1";
	public int portListen = 3002;
	public int portSend = 3010;
	
	[Header("COLOURS")] 
	public Color restColour;
	public Color defaultColour;
	public Color highlightColour;
	public Color defaultFixationColour;
	public Color highlightFixationColour;
	public Color defaultIndicatorColour;
	public Color highlightIndicatorColour;
	public Color controlRestColour;
	public Color controlActiveColour;

	[Header("COLOURS UI")] 
	public Color UI_Orange;
	public Color UI_Blue;
	public Color UI_Disabled;

	[Header("OBJECT POOLS")] 
	public GameObject objectPools;
	private UI_DisplayObjects displayObjects;
	private ReachTaskObjects reachTaskObjects;
	
	//events
	public delegate void SettingsUpdate();
	public static event SettingsUpdate OnSettingsUpdate; //used to inform classes when a settings update happens (ex.sessiontotals in UI)

	private bool initialised = false;

	void Awake () {
		// EasySave.Delete<int>("targetDuration");
		instance = this;
		displayObjects = objectPools.GetComponent<UI_DisplayObjects>();
		reachTaskObjects = objectPools.GetComponent<ReachTaskObjects>();
		//check and load save files
		LoadSave();
		
		
	}

	private void Start() {
		//initialise display
		InitialiseInterface();
		//displayObjects.settingsPanel.SetActive(true);
		displayObjects.TrialUI.SetActive(true);
		
		//DEPRECIATED from V2 - readd in different context in  later version
		// displayObjects.trial2D.SetActive(true);
		// displayObjects.trial2D_RenderTexture.SetActive(true);
		
	}
	//utility method
	private string FormatString(float v){
		// string t = "";
		int m = (int)v / 60;
		int s = (int)v % 60;
		string t = m.ToString() + ":" + ((s < 10) ? ("0") : ("")) + s.ToString();;
		return t;
	}
	public GameStatus Status {
		get { return gameStatus;}
		set { gameStatus = value;}
	}
	
	// public void ToggleSettings() {
	// 	if (active) {
	// 		displayObjects.settingsPanel.SetActive(false);
	// 		active = false;
	// 	}
	// 	else {
	// 		displayObjects.settingsPanel.SetActive(true);
	// 		active = true;
	// 	}
	// }

//---------SESSION INFO---------
	public string GetSessionInfo()
	{
		string s = sessionName + sessionNumber.ToString();
		return s;
	}
	
	//metrics
	private void UpdateSessionTotals(){
		//update totals for UI 
		trialsPerBlock = numberOfTargets * repetitions;
		trialsPerRun = trialsPerBlock * blocksPerRun;
		trialsPerSession = trialsPerRun * sessionRuns;
		
		//update timing totals...
		if (!actionObservation){
			estTrialDuration = preTrialWaitPeriod + fixationPeriod + indicationPeriod + targetPresentationPeriod + restPeriodMax + postTrialWaitPeriod;
		}
		else{
			estTrialDuration = preTrialWaitPeriod + fixationPeriod + indicationPeriod + observationPeriod + targetPresentationPeriod + restPeriodMax + postTrialWaitPeriod;
		}
		estBlockDuration = estTrialDuration * trialsPerBlock;
		estRunDuration = estBlockDuration * blocksPerRun;
		estSessionDuration = estRunDuration * sessionRuns;
		
		//calculate plus rest periods
		int rpTotal = interRunRestPeriod * sessionRuns;
		int cdTotal = (preBlockCountdown * blocksPerRun) * sessionRuns;
		estSessionDuration = estSessionDuration + rpTotal + cdTotal;

		UpdateSettingsEvent();
		SaveState();
	}

	public void UpdateSettingsEvent(){
		if (OnSettingsUpdate != null){
			OnSettingsUpdate();
		}
	}
//---------PORTS----------------
	public void SetIP(string v) {
		ip = v;
		UDPClient.instance.ip = ip;
		SaveState();
	}
	public void SetListen(int v) {
		portListen = v;
		UDPClient.instance.portListen = portListen;
		SaveState();
	}
	public void SetSend(int v) {
		portSend = v;
		UDPClient.instance.portSend = portSend;
		SaveState();
	}

	#region Paradigm Setup

	public void SetTrialParadigm(TrialParadigm paradigm){
		trialParadigm = paradigm;
		GameManager.instance.trialParadigm = trialParadigm;
		GameManager.instance.InitialiseSession();
		SaveState();
	}

	public void SetParadigmTargetCount(ParadigmTargetCount tCount){
		paradigmTargetCount = tCount;
		switch (paradigmTargetCount){
			case ParadigmTargetCount.One: { numberOfTargets = 1; break; }
			case ParadigmTargetCount.Two: { numberOfTargets = 2; break; }
			case ParadigmTargetCount.Three: { numberOfTargets = 3; break; }
			case ParadigmTargetCount.Four: { numberOfTargets = 4; break; }
			case ParadigmTargetCount.Eight: { numberOfTargets = 8; break; }
			case ParadigmTargetCount.Sixteen: { numberOfTargets = 16; break; }
		}
		UpdateSessionTotals();
		GameManager.instance.targetCount = numberOfTargets;
		GameManager.instance.InitialiseSession();
		SaveState();
	}

	public void SetRepetitions(int num){
		repetitions = num;
		UpdateSessionTotals();
		GameManager.instance.trialRepetitions = repetitions;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	
	//TODO----------------
	public void SetSequenceType(SequenceType seqType){
		sequenceType = seqType;
		GameManager.instance.sequenceType = sequenceType;
		GameManager.instance.InitialiseSession();
		SaveState();
	}

	public void SetHandedness(Handedness side){
		handedness = side;
		//todo update with new reach paradigm from game manager
		GameManager.instance.handedness = handedness;
		GameManager.instance.InitialiseSession();
		SaveState();
	}


	#endregion

	#region Control Setup
	public void SetAssistance(float v){
		BCI_ControlAssistance = v;
		BCI_ControlSignal.instance.controlAssistPercentage = Mathf.RoundToInt(BCI_ControlAssistance);
		SaveState();
	}
	public void SetAssistanceModifier(float v){
		assistanceModifier = v;
		BCI_ControlSignal.instance.assistanceModifier = Mathf.RoundToInt(assistanceModifier);
		//TODO this doen't affect anything yet...
		SaveState();
	}
	public void SetMagnitude(float v){
		controlMagnitude = v;
		controlMagnitude = Mathf.Round(controlMagnitude * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.magnitudeMultiplier = Mathf.Round(controlMagnitude * 10.0f) * 0.1f;
		SaveState();
	}
	public void SetMagnitudeX(float v){
		controlMagnitudeX = v;
		controlMagnitudeX = Mathf.Round(controlMagnitudeX * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.magnitudeMultiplierX = Mathf.Round(controlMagnitudeX * 10.0f) * 0.1f;
		SaveState();
	}
	public void SetMagnitudeY(float v){
		controlMagnitudeY = v;
		controlMagnitudeY = Mathf.Round(controlMagnitudeY * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.magnitudeMultiplierY = Mathf.Round(controlMagnitudeY * 10.0f) * 0.1f;
		SaveState();
	}
	public void SetMagnitudeZ(float v){
		controlMagnitudeZ = v;
		controlMagnitudeZ = Mathf.Round(controlMagnitudeZ * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.magnitudeMultiplierZ = Mathf.Round(controlMagnitudeZ * 10.0f) * 0.1f;
		SaveState();
	}
	public void SetSmoothingSpeed(float v){
		smoothingSpeed = v;
		smoothingSpeed = Mathf.Round(smoothingSpeed * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.defaultSmoothing = Mathf.Round(smoothingSpeed * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.targetSmoothing = Mathf.Round(smoothingSpeed * 100.0f) * 0.01f;
		BCI_ControlSignal.instance.smoothDamping = Mathf.Round(smoothingSpeed * 100.0f) * 0.01f;
		SaveState();
	}

	#endregion

	#region Session Settings

	public void SetRuns(int num) {
		sessionRuns = num;
		UpdateSessionTotals();
		GameManager.instance.runTotal = sessionRuns;
		GameManager.instance.InitialiseSession();
		//ScoreManager.instance.ResetSession(sessionRuns, blocksPerRun);
		SaveState();
	}
	//TODO RUNTYPE SETTINGS
	public RunType GetRunType(int index)
	{
		return runSequence[index]; 
	}

	public bool CheckImaginedRun(int index){
		bool b = false;
		if (runSequence[index] == RunType.Imagined){
			b = true;
		}
		return b;
	}
	//not used yet - potentially for user deciding run type
	public void SetRunType(RunType runType){
		
	}

	public void SetBlocksPerRun(int num) {
		blocksPerRun = num;
		UpdateSessionTotals();
		GameManager.instance.blockTotal = blocksPerRun;
		GameManager.instance.InitialiseSession();
		//ScoreManager.instance.ResetSession(sessionRuns, blocksPerRun);
		SaveState();
	}
	public void SetBlockCountdown(int num) {
		preBlockCountdown = num;
		UpdateSessionTotals();
		GameManager.instance.blockCountdown = preBlockCountdown;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetVisibleCountdown(int num) {
		visibleCountdown = num;
		UpdateSessionTotals();
		GameManager.instance.visibleCountdown = visibleCountdown;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetInterRunRestPeriod(int num) {
		interRunRestPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.interRunRestPeriod = interRunRestPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}

	#endregion

	#region Trial Timings
	
	public void SetSpeedModifier(float s){
		gameSpeedModifier = s;
		GameManager.instance.SetGameSpeed(gameSpeedModifier);
		//trial speed isn't saved..
	}
	public void SetPreTrialWaitPeriod(float num){
		preTrialWaitPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.preTrialWaitPeriod = preTrialWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetFixationPeriod(float num){
		fixationPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.fixationPeriod = fixationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetIndicationPeriod(float num){
		indicationPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.indicationPeriod = indicationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetObservationPeriod(float num) {
		observationPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.observationPeriod = observationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetTargetPeriod(float num) {
		targetPresentationPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.targetPresentationPeriod = targetPresentationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetRestPeriodMin(float num) {
		restPeriodMin = num;
		UpdateSessionTotals();
		GameManager.instance.restPeriodMinimum = restPeriodMin;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetRestPeriodMax(float num) {
		restPeriodMax = num;
		UpdateSessionTotals();
		GameManager.instance.restPeriodMaximum = restPeriodMax;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostTrialWaitPeriod(float num) {
		postTrialWaitPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.postTrialWaitPeriod = postTrialWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostBlockWaitPeriod(float num) {
		postBlockWaitPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.postBlockRestPeriod = postBlockWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostRunWaitPeriod(float num) {
		postRunWaitPeriod = num;
		UpdateSessionTotals();
		GameManager.instance.postRunRestPeriod = postRunWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	#endregion

	#region Additional Settings
	public void SetActionObservation(bool t){
		actionObservation = t;
		GameManager.instance.actionObservation = actionObservation;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetRecordTrajectory(bool t){
		recordMotionData = t;
		SaveState();
	}
	public void SetSampleRate(SampleRate sr){
		sampleRate = sr;
		//SET samplerate in steam VR
		if (SteamVR.instance != null) {
			//Debug.Log(SteamVR.instance);
			switch (sampleRate) {
				case SampleRate.Hz50 : { SteamVR_Render.instance.sampleRateSteamRender = 0.02f; break; }
				case SampleRate.Hz60 : { SteamVR_Render.instance.sampleRateSteamRender = 0.01666667f; break; }
				case SampleRate.Hz75 : { SteamVR_Render.instance.sampleRateSteamRender = 0.01333f; break; }
				case SampleRate.Hz100 : { SteamVR_Render.instance.sampleRateSteamRender = 0.01f; break; }
			}
			//TODO - change sample rate in unity settings as well for non vr
		}
		SaveState();
	}
	#endregion

	#region Interface

	public void Set3DEnvironment(bool t){
		environment3D = t;
		//toggle environment here
		if (SetVREnvironment.instance != null)
		{
			SetVREnvironment.instance.SetEnvironment();
		}
		
		SaveState();
	}
	public void SetInterface3D(bool t){
		interface3D = t;
		//toggle environment here
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	
	//DEPRECIATED from v2 - readd to later version
	public void SetDisplay(bool t){
		display2D = t;
		//toggle environment here
//		SetWorldUI.instance.SetUI();

		if (display2D){
			SetDisplayType.instance.SetDisplay(DisplayType.Screen2D);
		}
		else{
			SetDisplayType.instance.SetDisplay(DisplayType.ScreenVR);
		}
		SaveState();
	}

	//DEPRECIATED from v2 - targets are now created one at a time and unlabeled
	public void SetLabelsVisible(bool t) {
		labelsVisible = t;
		// for (int i = 0; i < displayObjects.labels.Length; i++) {
		// 	displayObjects.labels[i].SetActive(labelsVisible);
		// }
		// for (int i = 0; i < reachTaskObjects.labels.Length; i++)
		// {
		// 	reachTaskObjects.labels[i].SetActive(labelsVisible);
		// }
		SaveState();
	}
	public void SetRestVisible(bool t) {
		restVisible = t;
//		displayObjects.rest.SetActive(restVisible);
		SaveState();
	}
	public void SetDisplayScore(bool t) {
		displayScore = t;
		displayObjects.scoreDisplay.SetActive(displayScore);
		reachTaskObjects.scoreDisplay.SetActive(displayScore);
		SaveState();
	}
	public void SetDisplayStatus(bool t) {
		displayStatus = t;
		displayObjects.statusDisplay.SetActive(displayStatus);
		reachTaskObjects.statusDisplay.SetActive(displayStatus);
		SaveState();
	}
	public void SetDisplayProgress(bool t) {
		displayProgress = t;
		displayObjects.progressDisplayTrial.SetActive(displayProgress);
		//reachTaskObjects.progressDisplayTrial.SetActive(displayProgress);
		displayObjects.progressDisplayBlock.SetActive(displayProgress);
		//reachTaskObjects.progressDisplayBlock.SetActive(displayProgress);
		displayObjects.progressDisplayRun.SetActive(displayProgress);
		//reachTaskObjects.progressDisplayRun.SetActive(displayProgress);

		displayObjects.progressDisplayRun.transform.parent.gameObject.SetActive(t);
		SaveState();
	}
	public void SetDisplayCountdown(bool t) {
		displayCountdown = t;
		displayObjects.countdownDisplay.SetActive(displayCountdown);
		reachTaskObjects.countdownDisplay.SetActive(displayCountdown);
		SaveState();
	}
	public void SetShowFramerate(bool t) {
		showFramerate = t;
		displayObjects.FPS.SetActive(showFramerate);
		SaveState();
	}
	
	//DEPRECIATED from v2 - readd in different context to later version
	public void SetLinesOuter(bool t) {
		linesOuter = t;
//		displayObjects.outerLines.SetActive(linesOuter);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetLinesInnerA(bool t) {
		linesInnerA = t;
//		displayObjects.innerLinesA.SetActive(linesInnerA);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetLinesInnerB(bool t) {
		linesInnerB= t;
//		displayObjects.innerLinesB.SetActive(linesInnerB);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetRingOuter(bool t) {
		ringOuter= t;
//		displayObjects.ringOuter.SetActive(ringOuter);
		SetWorldUI.instance.SetUI();
		SaveState();
	}

	#endregion


	#region Feedback

	public void SetAnimateTargets(bool t) {
		animateTargets = t;
		//this is accessed from settings
		//TrialSequence.instance.animateTargets = animateTargets;
		SaveState();
	}

	#endregion

	
	#region Character (Depreciated)
	
	public void SetCharacterVisible_C(bool t) {
		characterVisible_C = t;
//		displayObjects.controlCharacter_C.SetActive(characterVisible_C);
		SaveState();
	}
	public void SetCharacterVisible_L(bool t) {
		characterVisible_L = t;
//		displayObjects.controlCharacter_L.SetActive(characterVisible_L);
		SaveState();
	}
	public void SetCharacterVisible_T(bool t) {
		characterVisible_T = t;
//		displayObjects.controlCharacter_T.SetActive(characterVisible_T);
		SaveState();
	}
	public void SetCharacterVisible_R(bool t) {
		characterVisible_R = t;
//		displayObjects.controlCharacter_R.SetActive(characterVisible_R);
		SaveState(); 
	}
	public void SetCharacterVisible_B(bool t) {
		characterVisible_B = t;
//		displayObjects.controlCharacter_B.SetActive(characterVisible_B);
		SaveState();
	}
	public void SetCharacterColourType(CharacterColourType type) {
		characterColourType = type;
//		ControlCharacter.instance.characterColourType = characterColourType;
		SaveState();
	}
	public void SetColourSmoothing(bool t) {
		colourSmoothing = t;
//		ControlCharacter.instance.smoothColour = colourSmoothing;
		SaveState();
	}
	public void SetColourSpeed(int s) {
		colourSpeed = s;
//		ControlCharacter.instance.colourSpeed = colourSpeed;
		SaveState();
	}
	public void SetCharacterSmoothing(bool t) {
		characterSmoothing = t;
//		ControlCharacter.instance.smoothCharacter = characterSmoothing;
		SaveState();
	}
	
	public void SetSmoothSpeed(int s) {
		smoothSpeed = s;
//		ControlCharacter.instance.smoothSpeed = smoothSpeed;
		SaveState();
	}

	#endregion



	#region Save & Load

	private void SaveState() {
		//ports
		EasySave.Save("ip", ip);
		EasySave.Save("portListen", portListen);
		EasySave.Save("portSend", portSend);
		
		#region session & trial save
		// EasySave.Save("trialType", trialType.ToString());
		EasySave.Save("trialParadigm", trialParadigm.ToString());
		EasySave.Save("paradigmTargetCount", paradigmTargetCount.ToString());
		EasySave.Save("repetitions", repetitions);
		EasySave.Save("sequenceType", sequenceType.ToString());
		EasySave.Save("handedness", handedness.ToString());

		EasySave.Save("sessionRuns", sessionRuns);
		EasySave.Save("blocksPerRun", blocksPerRun);
		EasySave.Save("preBlockCountdown", preBlockCountdown);
		EasySave.Save("visibleCountdown", visibleCountdown);
		EasySave.Save("interRunRestPeriod", interRunRestPeriod);
		
		EasySave.Save("preTrialWaitPeriod", preTrialWaitPeriod);
		EasySave.Save("fixationPeriod", fixationPeriod);
		EasySave.Save("indicationPeriod", indicationPeriod);
		EasySave.Save("observationPeriod", observationPeriod);
		EasySave.Save("targetPresentationPeriod", targetPresentationPeriod);
		EasySave.Save("restPeriodMin", restPeriodMin);
		EasySave.Save("restPeriodMax", restPeriodMax);
		EasySave.Save("postTrialWaitPeriod", postTrialWaitPeriod);
		EasySave.Save("postBlockWaitPeriod", postBlockWaitPeriod);
		EasySave.Save("postRunWaitPeriod", postRunWaitPeriod);
		
		EasySave.Save("recordTrajectory", recordMotionData);
		EasySave.Save("sampleRate", sampleRate.ToString());
		EasySave.Save("actionObservation", actionObservation);
		
//		Debug.Log(BCI_ControlAssistance);
		
		EasySave.Save("BCI_ControlAssistance", BCI_ControlAssistance);
		EasySave.Save("assistanceModifier", assistanceModifier);
		EasySave.Save("controlMagnitude", controlMagnitude);
		EasySave.Save("controlMagnitudeX", controlMagnitudeX);
		EasySave.Save("controlMagnitudeY", controlMagnitudeY);
		EasySave.Save("controlMagnitudeZ", controlMagnitudeZ);
		EasySave.Save("smoothingSpeed", smoothingSpeed);

		#endregion
		

		//environment
		EasySave.Save("environment3D", environment3D);
			
		//interface 3D
		EasySave.Save("interface3D", interface3D);
		EasySave.Save("display2D", display2D); 

		//interface
		EasySave.Save("animateTargets", animateTargets);
		EasySave.Save("labelsVisible", labelsVisible);
		EasySave.Save("restVisible", restVisible);
		EasySave.Save("displayScore", displayScore);
		EasySave.Save("displayStatus", displayStatus);
		EasySave.Save("displayProgress", displayProgress);
		EasySave.Save("displayCountdown", displayCountdown);
		EasySave.Save("showFramerate", showFramerate);
		EasySave.Save("linesOuter", linesOuter);
		EasySave.Save("linesInnerA", linesInnerA);
		EasySave.Save("linesInnerB", linesInnerB);
		EasySave.Save("ringOuter", ringOuter);
		//character
		EasySave.Save("characterVisible_C", characterVisible_C);
		EasySave.Save("characterVisible_L", characterVisible_L);
		EasySave.Save("characterVisible_T", characterVisible_T);
		EasySave.Save("characterVisible_R", characterVisible_R);
		EasySave.Save("characterVisible_B", characterVisible_B);
		EasySave.Save("characterColourType", characterColourType.ToString());
		EasySave.Save("colourSmoothing", colourSmoothing);
		EasySave.Save("colourSpeed", colourSpeed);
		EasySave.Save("characterSmoothing", characterSmoothing);
		EasySave.Save("smoothSpeed", smoothSpeed);

		#region JSON Session Data Save
		//Save state to settings Data object for writing to file
		// settingsData.trialType = trialType.ToString();
		settingsData.trialParadigm = trialParadigm.ToString();
		settingsData.targetCount = paradigmTargetCount.ToString();
		settingsData.repetitions = repetitions;
		settingsData.handedness = handedness.ToString();

		if (display2D){
			settingsData.displayType = DisplayType.Screen2D.ToString();
		}
		else{
			settingsData.displayType = DisplayType.ScreenVR.ToString();
		}
		

		settingsData.blocks = blocksPerRun;
		settingsData.runs = sessionRuns;

		settingsData.runSequence = runSequence;
		
		settingsData.trialsPerBlock = trialsPerBlock;
		settingsData.trialsPerRun = trialsPerRun;
		settingsData.trialsPerSession = trialsPerSession;

		settingsData.estimatedTrialDuration = estTrialDuration;
		settingsData.estimatedBlockDuration = FormatString(estBlockDuration);
		settingsData.estimatedRunDuration = FormatString(estRunDuration);
		settingsData.estimatedSessionDuration = FormatString(estSessionDuration);
		
		settingsData.preBlockCountdown = preBlockCountdown;
		settingsData.visibleCountdown = visibleCountdown;
		settingsData.interRunRestPeriod = interRunRestPeriod;
		
		settingsData.preTrialWaitPeriod = preTrialWaitPeriod;
		settingsData.fixationDuration = fixationPeriod;
		settingsData.indicationDuration = indicationPeriod;
		settingsData.observationDuration = observationPeriod;
		settingsData.targetDuration = targetPresentationPeriod;
		settingsData.restPeriodMin = restPeriodMin;
		settingsData.restPeriodMax = restPeriodMax;
		settingsData.postTrialWaitPeriod = postTrialWaitPeriod;
		settingsData.postBlockWaitPeriod = postBlockWaitPeriod;
		settingsData.postRunWaitPeriod = postRunWaitPeriod;

		settingsData.actionObservation = actionObservation;
		settingsData.sampleRate = sampleRate.ToString();
		
		//control
		settingsData.BCI_ControlAssistance = BCI_ControlAssistance;
		settingsData.assistanceModifier = assistanceModifier;
		settingsData.controlMagnitude = controlMagnitude;
		settingsData.controlMagnitudeX = controlMagnitudeX;
		settingsData.controlMagnitudeY = controlMagnitudeY;
		settingsData.controlMagnitudeZ = controlMagnitudeZ;
		settingsData.smoothingSpeed = smoothingSpeed;
		
		#endregion


		JSONWriter jWriter = new JSONWriter();
		jWriter.OutputSettingsJSON(settingsData);
		
//		Debug.Log("Settings Saved!");
	}
	
	private void LoadSave() {
		if (EasySave.HasKey<string>("ip")) {
			//ports
			ip = EasySave.Load<string>("ip");
			portListen = EasySave.Load<int>("portListen");
			portSend = EasySave.Load<int>("portSend");

			#region paradigm data load

			string tp = EasySave.Load<string>("trialParadigm");
			if (tp == "Horizontal"){
				trialParadigm = TrialParadigm.Horizontal;
			}
			if (tp == "Vertical"){
				trialParadigm = TrialParadigm.Vertical;
			}
			if (tp == "Circle"){
				trialParadigm = TrialParadigm.Circle;
			}
			if (tp == "CentreOut"){
				trialParadigm = TrialParadigm.CentreOut;
			}

			string ptars = EasySave.Load<string>("paradigmTargetCount");
			if (ptars == "One"){
				paradigmTargetCount = ParadigmTargetCount.One;
			}
			if (ptars == "Two"){
				paradigmTargetCount = ParadigmTargetCount.Two;
			}
			if (ptars == "Three"){
				paradigmTargetCount = ParadigmTargetCount.Three;
			}
			if (ptars == "Four"){
				paradigmTargetCount = ParadigmTargetCount.Four;
			}
			if (ptars == "Eight"){
				paradigmTargetCount = ParadigmTargetCount.Eight;
			}
			if (ptars == "Sixteen"){
				paradigmTargetCount = ParadigmTargetCount.Sixteen;
			}
			
			repetitions = EasySave.Load<int>("repetitions");
			
			string st = EasySave.Load<string>("sequenceType");
			if (st == "Linear"){
				sequenceType = SequenceType.Linear;
			}
			if (st == "Permutation")
			{
				sequenceType = SequenceType.Permutation;
			}
			string hand = EasySave.Load<string>("handedness");
			if (hand == "Left")
			{
				handedness = Handedness.Left;
			}
			if (hand == "Right")
			{
				handedness = Handedness.Right;
			}

			#endregion

			#region Session data load

			sessionRuns = EasySave.Load<int>("sessionRuns");
			blocksPerRun = EasySave.Load<int>("blocksPerRun");
			preBlockCountdown = EasySave.Load<int>("preBlockCountdown");
			visibleCountdown = EasySave.Load<int>("visibleCountdown");
			interRunRestPeriod = EasySave.Load<int>("interRunRestPeriod");
			
			#endregion

			#region trial data load

			preTrialWaitPeriod = EasySave.Load<float>("preTrialWaitPeriod");
			fixationPeriod = EasySave.Load<float>("fixationPeriod");
			indicationPeriod = EasySave.Load<float>("indicationPeriod");
			observationPeriod = EasySave.Load<float>("observationPeriod");
			targetPresentationPeriod = EasySave.Load<float>("targetPresentationPeriod"); //check float
			restPeriodMin = EasySave.Load<float>("restPeriodMin");
			restPeriodMax = EasySave.Load<float>("restPeriodMax");
			postTrialWaitPeriod = EasySave.Load<float>("postTrialWaitPeriod");
			postBlockWaitPeriod = EasySave.Load<float>("postBlockWaitPeriod");
			postRunWaitPeriod = EasySave.Load<float>("postRunWaitPeriod");

			#endregion
			
			#region additional setting data load

			recordMotionData = EasySave.Load<bool>("recordTrajectory");
			string sr = EasySave.Load<string>("sampleRate");
			if (sr == "Hz50") { sampleRate = SampleRate.Hz50; }
			if (sr == "Hz60") { sampleRate = SampleRate.Hz60; }
			if (sr == "Hz75") { sampleRate = SampleRate.Hz75; }
			if (sr == "Hz100") { sampleRate = SampleRate.Hz100; }
						
			actionObservation = EasySave.Load<bool>("actionObservation");

			#endregion

			#region Control Settings

			BCI_ControlAssistance = EasySave.Load<float>("BCI_ControlAssistance");
			assistanceModifier = EasySave.Load<float>("assistanceModifier");
			controlMagnitude = EasySave.Load<float>("controlMagnitude");
			controlMagnitudeX = EasySave.Load<float>("controlMagnitudeX");
			controlMagnitudeY = EasySave.Load<float>("controlMagnitudeY");
			controlMagnitudeZ = EasySave.Load<float>("controlMagnitudeZ");
			smoothingSpeed = EasySave.Load<float>("smoothingSpeed");
			
//			Debug.Log(BCI_ControlAssistance);
			
			#endregion

			
			
			//environment
			environment3D = EasySave.Load<bool>("environment3D");
			
			//interface3D
			interface3D = EasySave.Load<bool>("interface3D");
			display2D = EasySave.Load<bool>("display2D");
			display2D = false; //start in VR always

			//interface
			animateTargets = EasySave.Load<bool>("animateTargets");
			labelsVisible = EasySave.Load<bool>("labelsVisible");
			restVisible = EasySave.Load<bool>("restVisible");
			displayScore = EasySave.Load<bool>("displayScore");
			displayStatus = EasySave.Load<bool>("displayStatus");
			displayProgress = EasySave.Load<bool>("displayProgress");
			displayCountdown = EasySave.Load<bool>("displayCountdown");
			showFramerate = EasySave.Load<bool>("showFramerate");
			linesOuter = EasySave.Load<bool>("linesOuter");
			linesInnerA = EasySave.Load<bool>("linesInnerA");
			linesInnerB = EasySave.Load<bool>("linesInnerB");
			ringOuter = EasySave.Load<bool>("ringOuter");
			
			//character
			characterVisible_C = EasySave.Load<bool>("characterVisible_C");
			characterVisible_L = EasySave.Load<bool>("characterVisible_L");
			characterVisible_T = EasySave.Load<bool>("characterVisible_T");
			characterVisible_R = EasySave.Load<bool>("characterVisible_R");
			characterVisible_B = EasySave.Load<bool>("characterVisible_B");
			string cct = EasySave.Load<string>("characterColourType");
			if (cct == "Solid") {
				characterColourType = CharacterColourType.Static;
			}
			else {
				characterColourType = CharacterColourType.Dynamic;
			}
			colourSmoothing = EasySave.Load<bool>("colourSmoothing");
			colourSpeed = EasySave.Load<int>("colourSpeed");
			characterSmoothing = EasySave.Load<bool>("characterSmoothing");
			smoothSpeed = EasySave.Load<int>("smoothSpeed");
		}
		else {
			//ports
			ip = "127.0.0.1";
			portListen = 3002;
			portSend = 3010;

			#region paradigm
			// trialType = TrialType.CentreOut;
			trialParadigm = TrialParadigm.Horizontal;
			paradigmTargetCount = ParadigmTargetCount.Two;
			repetitions = 8;
			sequenceType = SequenceType.Permutation;
			handedness = Handedness.Right;
			#endregion

			#region session
			sessionRuns = 2;
			blocksPerRun = 4;
			preBlockCountdown = 10;
			visibleCountdown = 5;
			interRunRestPeriod = 20;
			#endregion
			
			#region trial
			preTrialWaitPeriod = 1f;
			fixationPeriod = 2f;
			indicationPeriod = 2f;
			observationPeriod = 2f;
			targetPresentationPeriod = 2f;
			restPeriodMin = 2f;
			restPeriodMax = 4f;
			postTrialWaitPeriod = 1f;
			postBlockWaitPeriod = 1f;
			postRunWaitPeriod = 1f;
			#endregion

			#region additional
			recordMotionData = false;
			sampleRate = SampleRate.Hz60;
			actionObservation = false;
			#endregion
			
			#region Control Settings

			BCI_ControlAssistance = 100f;
			assistanceModifier = 0;
			controlMagnitude = 1f;
			controlMagnitudeX = 1f;
			controlMagnitudeY = 1f;
			controlMagnitudeZ = 1f;
			smoothingSpeed = 0.45f;
			
			#endregion
			
			//environment 3d
			environment3D = false;
			
			//interface 3d
			interface3D = false;
			display2D = false;
			
			//interface
			animateTargets = false;
			labelsVisible = true;
			restVisible = false;
			displayScore = false;
			displayStatus = true;
			displayProgress = true;
			displayCountdown = true;
			showFramerate = true;
			linesOuter = true;
			linesInnerA = false;
			linesInnerB = true;
			ringOuter = false;
			
			//character
			characterVisible_C = true;
			characterVisible_L = true;
			characterVisible_T = true;
			characterVisible_R = true;
			characterVisible_B = true;
			characterColourType = CharacterColourType.Dynamic;
			colourSmoothing = true;
			colourSpeed = 15;
			characterSmoothing = true;
			smoothSpeed = 15;
			
			
		}
	}
	private void InitialiseInterface() {
		Debug.Log("Initialise Interface!");
		//ports
		SetIP(ip);
		SetListen(portListen);
		SetSend(portSend);

		#region paradigm
		SetTrialParadigm(trialParadigm);
		SetParadigmTargetCount(paradigmTargetCount);
		SetRepetitions(repetitions);
		SetSequenceType(sequenceType);
		SetHandedness(handedness);
		#endregion

		#region session
		SetRuns(sessionRuns);
		SetBlocksPerRun(blocksPerRun);
		SetBlockCountdown(preBlockCountdown);
		SetVisibleCountdown(visibleCountdown);
		SetInterRunRestPeriod(interRunRestPeriod);
		#endregion
		
		UpdateSessionTotals();
		
		#region trial
		SetPreTrialWaitPeriod(preTrialWaitPeriod);
		SetFixationPeriod(fixationPeriod);
		SetIndicationPeriod(indicationPeriod);
		SetObservationPeriod(observationPeriod);
		SetTargetPeriod(targetPresentationPeriod);
		SetRestPeriodMin(restPeriodMin);
		SetRestPeriodMax(restPeriodMax);
		SetPostTrialWaitPeriod(postTrialWaitPeriod);
		SetPostBlockWaitPeriod(postBlockWaitPeriod);
		SetPostRunWaitPeriod(postRunWaitPeriod);
		#endregion

		#region additional
		SetRecordTrajectory(recordMotionData);
		SetSampleRate(sampleRate);
		SetActionObservation(actionObservation);
		#endregion
		
		#region Control Settings

//		Debug.Log(BCI_ControlAssistance);
		
		SetAssistance(BCI_ControlAssistance);
		SetAssistanceModifier(assistanceModifier);
		SetMagnitude(controlMagnitude);
		SetMagnitudeX(controlMagnitudeX);
		SetMagnitudeY(controlMagnitudeY);
		SetMagnitudeZ(controlMagnitudeZ);
		SetSmoothingSpeed(smoothingSpeed);

		#endregion

		//Environment 3D
		Set3DEnvironment(environment3D);
		
		//interface 3d
		SetInterface3D(interface3D);
		SetDisplay(display2D);
		
		//interface
		SetAnimateTargets(animateTargets);
		SetLabelsVisible(labelsVisible);
		SetRestVisible(restVisible);
		SetDisplayScore(displayScore);
		SetDisplayStatus(displayStatus);
		SetDisplayProgress(displayProgress);
		SetDisplayCountdown(displayCountdown);
		SetShowFramerate(showFramerate);
		SetLinesOuter(linesOuter);
		SetLinesInnerA(linesInnerA);
		SetLinesInnerB(linesInnerB);
		SetRingOuter(ringOuter);
		
		//character
		SetCharacterVisible_C(characterVisible_C);
		SetCharacterVisible_L(characterVisible_L);
		SetCharacterVisible_T(characterVisible_T);
		SetCharacterVisible_R(characterVisible_R);
		SetCharacterVisible_B(characterVisible_B);
		SetCharacterColourType(characterColourType);
		SetColourSmoothing(colourSmoothing);
		SetColourSpeed(colourSpeed);
		SetCharacterSmoothing(characterSmoothing);
		SetSmoothSpeed(smoothSpeed);
	}

	#endregion

	
}
