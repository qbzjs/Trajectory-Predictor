using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using SaveSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public static Settings instance;

	public bool active;
	//	public GameObject settingsPanel;

	[Header("SEQUENCE SETTINGS")]
	public TrialParadigm trialParadigm = TrialParadigm.Avatar3D;
	public TrialType trialType = TrialType.CentreOut;
	public TaskSide taskSide = TaskSide.Left;
	public SequenceType sequenceType = SequenceType.Permutation;

	[Header("TRIAL SETTINGS")] 
	public bool actionObservation = false;
	public bool recordTrajectory = false;
	
	[Range(1, 9)] 
	public int trialBlocks = 8; 
	[Range(1, 50)] 
	public int repetitions = 25; // num of sequences to run
	[Range(10, 60)] 
	public int startDelay = 60;
	[Range(2, 10)] 
	public int targetDuration = 2;
	[Range(10, 60)] 
	public int interBlockRestPeriod = 2;
	[Range(2, 10)] 
	public int restDurationMin = 2;
	[Range(2, 10)] 
	public int restDurationMax = 2;
	
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

	//Object pools
	private UI_DisplayObjects displayObjects;
	private ReachTaskObjects reachTaskObjects;

	private bool initialised = false;

	void Awake () {
		instance = this;
		displayObjects = GetComponent<UI_DisplayObjects>();
		reachTaskObjects = GetComponent<ReachTaskObjects>();
		//check and load save files
		LoadSave();
	}

	private void Start() {
		//initialise display
		InitialiseInterface();
		displayObjects.settingsPanel.SetActive(true);
		displayObjects.TrialUI.SetActive(true);
		displayObjects.trial2D.SetActive(true);
		displayObjects.trial2D_RenderTexture.SetActive(true);
		active = true;
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
//---------TRIAL----------------	

	public void SetTrialType(TrialType type) {
		trialType = type;
		TrialSequence.instance.trialType = trialType;
		TrialSequence.instance.InitialiseTrial();
		if (trialType == TrialType.Four_Targets && initialised) {
			restVisible = true;
			SetRestVisible(restVisible);
			//			settingsPanel.transform.Find("Rest Toggle").transform.Find("Rest Toggle").GetComponent<Toggle>().isOn = restVisible;
			displayObjects.settingsPanel.transform.Find("Interface Objects/Rest Toggle").GetComponent<ToggleUI>().Initialise();
		}
		if (trialType == TrialType.Three_Targets  && initialised) {
			restVisible = false;
			SetRestVisible(restVisible);
			displayObjects.settingsPanel.transform.Find("Interface Objects/Rest Toggle").GetComponent<ToggleUI>().Initialise();
//			settingsPanel.transform.Find("Rest Toggle").transform.Find("Rest Toggle").GetComponent<Toggle>().isOn = restVisible;
		}
		if (trialType == TrialType.CentreOut && initialised)
		{
			restVisible = true;
			SetRestVisible(restVisible);
			displayObjects.settingsPanel.transform.Find("Interface Objects/Rest Toggle").GetComponent<ToggleUI>().Initialise();
			//			settingsPanel.transform.Find("Rest Toggle").transform.Find("Rest Toggle").GetComponent<Toggle>().isOn = restVisible;
		}

		initialised = true;
		SaveState();
	}
	public void SetTrialParadigm(TrialParadigm paradigm)
	{
		trialParadigm = paradigm;
        //set the 2d trial camera to render on or off
        if (trialParadigm == TrialParadigm.Avatar3D)
        {
			displayObjects.trialCameraScreen2D.SetActive(false);
        }
        else
        {
			displayObjects.trialCameraScreen2D.SetActive(true);
		}
		SaveState();
	}

	public void SetReachTaskSide(TaskSide side)
	{
		taskSide = side;
		//contact 3d target manager
		reachTaskObjects.reachTaskObject.GetComponent<ReachTargetManager>().SetReachSide(taskSide);
		SaveState();
	}
	
	public void SetSequenceType(SequenceType seqType)
	{
		sequenceType = seqType;
		TrialSequence.instance.sequenceType = sequenceType;
		SaveState();
	}

	public void SetActionObservation(bool t){
		actionObservation = t;
		SaveState();
	}
	public void SetRecordTrajectory(bool t){
		recordTrajectory = t;
		SaveState();
	}
	
	public void SetTrialBlocks(int num) {
		trialBlocks = num;
		TrialManager.instance.InitialiseTrial(trialBlocks);
		//TrialSequence.instance.InitialiseSequence();
		SaveState();
	}
	public void SetRepetitions(int num) {
		repetitions = num;
		TrialSequence.instance.repetitions = repetitions;
		TrialSequence.instance.InitialiseSequence();
		SaveState();
	}
	public void SetStartDelay(int num) {
		startDelay = num;
		TrialSequence.instance.startDelay = startDelay;
		TrialManager.instance.initialWaitPeriod = startDelay;
		SaveState();
	}
	public void SetInterBlockRestPeriod(int num) {
		interBlockRestPeriod = num;
		//TrialSequence.instance.startDelay = startDelay;
		TrialManager.instance.interBlockRestPeriod = interBlockRestPeriod;
		SaveState();
	}
	public void SetRestDurationMin(int num) {
		restDurationMin = num;
		TrialSequence.instance.restDurationMin = restDurationMin;
		SaveState();
	}
	public void SetRestDurationMax(int num) {
		restDurationMax = num;
		TrialSequence.instance.restDurationMax = restDurationMax;
		SaveState();
	}
	public void SetTargetDuration(int num) {
		targetDuration = num;
		TrialSequence.instance.targetDuration = targetDuration;
		SaveState();
	}
	public void SetInterRunRestPeriod(int num) {
		//TODO set rest period here..
		
		SaveState();
	}
	
//----------INTERFACE----------------	

	public void Set3DEnvironment(bool t){
		environment3D = t;
		//toggle environment here
		SetVREnvironment.instance.SetEnvironment();
		SaveState();
	}
	public void SetInterface3D(bool t){
		interface3D = t;
		//toggle environment here
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetRenderTexture2D(bool t){
		renderTexture2D = t;
		//toggle environment here
		SetWorldUI.instance.SetUI();
		SaveState();
	}

	public void SetLabelsVisible(bool t) {
		labelsVisible = t;
		for (int i = 0; i < displayObjects.labels.Length; i++) {
			displayObjects.labels[i].SetActive(labelsVisible);
		}
		for (int i = 0; i < reachTaskObjects.labels.Length; i++)
		{
			reachTaskObjects.labels[i].SetActive(labelsVisible);
		}
		SaveState();
	}
	public void SetRestVisible(bool t) {
		restVisible = t;
		displayObjects.rest.SetActive(restVisible);
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
		displayObjects.progressDisplayMovement.SetActive(displayProgress);
		reachTaskObjects.progressDisplayMovement.SetActive(displayProgress);
		displayObjects.progressDisplayTrial.SetActive(displayProgress);
		reachTaskObjects.progressDisplayTrial.SetActive(displayProgress);
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
	public void SetLinesOuter(bool t) {
		linesOuter = t;
		displayObjects.outerLines.SetActive(linesOuter);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetLinesInnerA(bool t) {
		linesInnerA = t;
		displayObjects.innerLinesA.SetActive(linesInnerA);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetLinesInnerB(bool t) {
		linesInnerB= t;
		displayObjects.innerLinesB.SetActive(linesInnerB);
		SetWorldUI.instance.SetUI();
		SaveState();
	}
	public void SetRingOuter(bool t) {
		ringOuter= t;
		displayObjects.ringOuter.SetActive(ringOuter);
		SetWorldUI.instance.SetUI();
		SaveState();
	}

//----------FEEDBACK--------------------
	public void SetAnimateTargets(bool t) {
		animateTargets = t;
		TrialSequence.instance.animateTargets = animateTargets;
		SaveState();
	}
	
//----------CHARACTER-------------------	
	public void SetCharacterVisible_C(bool t) {
		characterVisible_C = t;
		displayObjects.controlCharacter_C.SetActive(characterVisible_C);
		SaveState();
	}
	public void SetCharacterVisible_L(bool t) {
		characterVisible_L = t;
		displayObjects.controlCharacter_L.SetActive(characterVisible_L);
		SaveState();
	}
	public void SetCharacterVisible_T(bool t) {
		characterVisible_T = t;
		displayObjects.controlCharacter_T.SetActive(characterVisible_T);
		SaveState();
	}
	public void SetCharacterVisible_R(bool t) {
		characterVisible_R = t;
		displayObjects.controlCharacter_R.SetActive(characterVisible_R);
		SaveState(); 
	}
	public void SetCharacterVisible_B(bool t) {
		characterVisible_B = t;
		displayObjects.controlCharacter_B.SetActive(characterVisible_B);
		SaveState();
	}
	public void SetCharacterColourType(CharacterColourType type) {
		characterColourType = type;
		ControlCharacter.instance.characterColourType = characterColourType;
		SaveState();
	}
	public void SetColourSmoothing(bool t) {
		colourSmoothing = t;
		ControlCharacter.instance.smoothColour = colourSmoothing;
		SaveState();
	}
	public void SetColourSpeed(int s) {
		colourSpeed = s;
		ControlCharacter.instance.colourSpeed = colourSpeed;
		SaveState();
	}
	public void SetCharacterSmoothing(bool t) {
		characterSmoothing = t;
		ControlCharacter.instance.smoothCharacter = characterSmoothing;
		SaveState();
	}
	
	public void SetSmoothSpeed(int s) {
		smoothSpeed = s;
		ControlCharacter.instance.smoothSpeed = smoothSpeed;
		SaveState();
	}

	public GameStatus Status {
		get { return gameStatus;}
		set { gameStatus = value;}
	}
//----INITIALISE  - SAVE & LOAD--------	

	private void SaveState() {
		//ports
		EasySave.Save("ip", ip);
		EasySave.Save("portListen", portListen);
		EasySave.Save("portSend", portSend);
		//trial
		EasySave.Save("trialType", trialType.ToString());
		EasySave.Save("trialParadigm", trialParadigm.ToString());
		EasySave.Save("taskSide", taskSide.ToString());
		EasySave.Save("sequenceType", sequenceType.ToString());
		
		EasySave.Save("actionObservation", actionObservation);
		EasySave.Save("recordTrajectory", recordTrajectory);

		EasySave.Save("trialBlocks", trialBlocks);
		EasySave.Save("repetitions", repetitions);
		EasySave.Save("startDelay", startDelay);
		EasySave.Save("interBlockRestPeriod", interBlockRestPeriod);
		EasySave.Save("restDurationMin", restDurationMin);
		EasySave.Save("restDurationMax", restDurationMax);
		EasySave.Save("targetDuration", targetDuration);
		
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

//		Debug.Log("Settings Saved!");
	}
	private void LoadSave() {
		if (EasySave.HasKey<string>("ip")) {
			//ports
			ip = EasySave.Load<string>("ip");
			portListen = EasySave.Load<int>("portListen");
			portSend = EasySave.Load<int>("portSend");
			
			//trial
			string tt = EasySave.Load<string>("trialType");
			if (tt == "CentreOut")
			{
				trialType = TrialType.CentreOut;
			}
			if (tt == "Four_Targets") {
				trialType = TrialType.Four_Targets;
			}
			if (tt == "Three_Targets")
			{
				trialType = TrialType.Three_Targets;
			}

			string tp = EasySave.Load<string>("trialParadigm");
			if (tp == "Avatar3D")
			{
				trialParadigm = TrialParadigm.Avatar3D;
			}
			if (tp == "Screen2D")
			{
				trialParadigm = TrialParadigm.Screen2D;
			}

			string ts = EasySave.Load<string>("taskSide");
			if (ts == "Left")
			{
				taskSide = TaskSide.Left;
			}
			if (ts == "Right")
			{
				taskSide = TaskSide.Right;
			}
			
			string st = EasySave.Load<string>("sequenceType");
			if (st == "Linear"){
				sequenceType = SequenceType.Linear;
			}
			if (st == "Permutation")
			{
				sequenceType = SequenceType.Permutation;
			}

			actionObservation = EasySave.Load<bool>("actionObservation");
			recordTrajectory = EasySave.Load<bool>("recordTrajectory");
			
			trialBlocks = EasySave.Load<int>("trialBlocks");
			repetitions = EasySave.Load<int>("repetitions");
			startDelay = EasySave.Load<int>("startDelay");
			interBlockRestPeriod = EasySave.Load<int>("interBlockRestPeriod");
			restDurationMin = EasySave.Load<int>("restDurationMin");
			restDurationMax = EasySave.Load<int>("restDurationMax");
			targetDuration = EasySave.Load<int>("targetDuration");

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
			
			//trial
			trialType = TrialType.CentreOut;
			trialParadigm = TrialParadigm.Avatar3D;	
			taskSide = TaskSide.Left;
			sequenceType = SequenceType.Permutation;

			actionObservation = false;
			recordTrajectory = false;

			trialBlocks = 8;
			repetitions = 25;
			startDelay = 60;
			interBlockRestPeriod = 15;
			restDurationMin = 2;
			restDurationMax = 2;
			targetDuration = 2;
			
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
		
		//trial
		SetTrialType(trialType);
		SetTrialParadigm(trialParadigm);
		SetReachTaskSide(taskSide);
		SetSequenceType(sequenceType);

		SetActionObservation(actionObservation);
		SetRecordTrajectory(recordTrajectory);

		SetTrialBlocks(trialBlocks);
		SetRepetitions(repetitions);
		SetStartDelay(startDelay);
		SetInterBlockRestPeriod(interBlockRestPeriod);
		SetRestDurationMin(restDurationMin);
		SetRestDurationMax(restDurationMax);
		SetTargetDuration(targetDuration);
		
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
}
