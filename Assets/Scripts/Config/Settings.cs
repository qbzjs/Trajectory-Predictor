using System;
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
	public int numberOfTargets = 0; //ready only
	public int trialsPerBlock = 0; //ready only
	public int trialsPerRun = 0; //ready only
	public int trialsPerSession = 0; //ready only
	[Range(1, 50)] 
	public int repetitions = 5; // num of sequences to run
	public SequenceType sequenceType = SequenceType.Permutation;
	public Handedness handedness = Handedness.Left;

	[Header("SESSION SETTINGS")] 
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
	public bool renderTexture2D = false;

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
	public Color highlightColour;
	public Color controlRestColour;
	public Color controlActiveColour;

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
		displayObjects.settingsPanel.SetActive(true);
		displayObjects.TrialUI.SetActive(true);
		
		//DEPRECIATED from V2 - readd in different context in  later version
		// displayObjects.trial2D.SetActive(true);
		// displayObjects.trial2D_RenderTexture.SetActive(true);
		
		active = true;
		//ToggleSettings(); // use to start in or out of settings menu
	}
	
	public GameStatus Status {
		get { return gameStatus;}
		set { gameStatus = value;}
	}
	
	public void ToggleSettings() {
		if (active) {
			displayObjects.settingsPanel.SetActive(false);
			active = false;
		}
		else {
			displayObjects.settingsPanel.SetActive(true);
			active = true;
		}
	}

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

	#region Session Settings

	public void SetRuns(int num) {
		sessionRuns = num;
		UpdateSessionTotals();
		GameManager.instance.runTotal = sessionRuns;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetBlocksPerRun(int num) {
		blocksPerRun = num;
		UpdateSessionTotals();
		GameManager.instance.blockTotal = blocksPerRun;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetBlockCountdown(int num) {
		preBlockCountdown = num;
		GameManager.instance.blockCountdown = preBlockCountdown;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetVisibleCountdown(int num) {
		visibleCountdown = num;
		GameManager.instance.visibleCountdown = visibleCountdown;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetInterRunRestPeriod(int num) {
		interRunRestPeriod = num;
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
		GameManager.instance.preTrialWaitPeriod = preTrialWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetFixationPeriod(float num){
		fixationPeriod = num;
		GameManager.instance.fixationPeriod = fixationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetIndicationPeriod(float num){
		indicationPeriod = num;
		GameManager.instance.indicationPeriod = indicationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetObservationPeriod(float num) {
		observationPeriod = num;
		GameManager.instance.observationPeriod = observationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetTargetPeriod(float num) {
		targetPresentationPeriod = num;
		GameManager.instance.targetPresentationPeriod = targetPresentationPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetRestPeriodMin(float num) {
		restPeriodMin = num;
		GameManager.instance.restPeriodMinimum = restPeriodMin;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetRestPeriodMax(float num) {
		restPeriodMax = num;
		GameManager.instance.restPeriodMaximum = restPeriodMax;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostTrialWaitPeriod(float num) {
		postTrialWaitPeriod = num;
		GameManager.instance.postTrialWaitPeriod = postTrialWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostBlockWaitPeriod(float num) {
		postBlockWaitPeriod = num;
		GameManager.instance.postBlockRestPeriod = postBlockWaitPeriod;
		GameManager.instance.InitialiseSession();
		SaveState();
	}
	public void SetPostRunWaitPeriod(float num) {
		postRunWaitPeriod = num;
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
	public void SetRenderTexture2D(bool t){
		renderTexture2D = t;
		//toggle environment here
//		SetWorldUI.instance.SetUI();
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
		#endregion
		

		//environment
		EasySave.Save("environment3D", environment3D);
			
		//interface 3D
		EasySave.Save("interface3D", interface3D);
		EasySave.Save("renderTexture2D", renderTexture2D);

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

		settingsData.runs = sessionRuns;
		settingsData.blocksPerRun = blocksPerRun;
		settingsData.preBlockCountdown = preBlockCountdown;
		settingsData.visibleCountdown = visibleCountdown;
		settingsData.interRunRestPeriod = interRunRestPeriod;
		
		settingsData.preTrialWaitPeriod = preTrialWaitPeriod;
		settingsData.fixationDuration = fixationPeriod;
		settingsData.arrowDuration = indicationPeriod;
		settingsData.observationDuration = observationPeriod;
		settingsData.targetDuration = targetPresentationPeriod;
		settingsData.restPeriodMin = restPeriodMin;
		settingsData.postTrialWaitPeriod = postTrialWaitPeriod;
		settingsData.postBlockWaitPeriod = postBlockWaitPeriod;
		settingsData.postRunWaitPeriod = postRunWaitPeriod;

		settingsData.actionObservation = actionObservation;
		settingsData.sampleRate = sampleRate.ToString();
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

			
			
			//environment
			environment3D = EasySave.Load<bool>("environment3D");
			
			//interface3D
			interface3D = EasySave.Load<bool>("interface3D");
			renderTexture2D = EasySave.Load<bool>("renderTexture2D");

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
			
			//environment 3d
			environment3D = false;
			
			//interface 3d
			interface3D = false;
			renderTexture2D = false;
			
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

		//Environment 3D
		Set3DEnvironment(environment3D);
		
		//interface 3d
		SetInterface3D(interface3D);
		SetRenderTexture2D(renderTexture2D);
		
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
