using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using UnityEngine.Serialization;

public class ToggleUI : MonoBehaviour {

	[FormerlySerializedAs("interfaceElement")] public VisualInterface visualInterface;
	public Toggle toggle;
	private Color initialColour;
	public Color highlightedColour;
	
	void Start(){
		toggle = GetComponentInChildren<Toggle>();
		initialColour = toggle.transform.GetChild(0).GetComponent<Image>().color;
		Initialise();
	}

	public void Initialise() {
		bool b = true;
		if (visualInterface == VisualInterface.CharacterVisible_C) {
			b = Settings.instance.characterVisible_C;
		}
		if (visualInterface == VisualInterface.CharacterVisible_L) {
			b = Settings.instance.characterVisible_L;
		}
		if (visualInterface == VisualInterface.CharacterVisible_T) {
			b = Settings.instance.characterVisible_T;
		}
		if (visualInterface == VisualInterface.CharacterVisible_R) {
			b = Settings.instance.characterVisible_R;
		}
		if (visualInterface == VisualInterface.CharacterVisible_B) {
			b = Settings.instance.characterVisible_B;
		}
		if (visualInterface == VisualInterface.ColourSmooth) {
			b = Settings.instance.colourSmoothing;
		}
		if (visualInterface == VisualInterface.CharacterSmooth) {
			b = Settings.instance.characterSmoothing;
		}
		if (visualInterface == VisualInterface.Labels) {
			b = Settings.instance.labelsVisible;
		}
		if (visualInterface == VisualInterface.Rest) {
			b = Settings.instance.restVisible;
		}
		if (visualInterface == VisualInterface.Score) {
			b = Settings.instance.displayScore;
		}
		if (visualInterface == VisualInterface.Status) {
			b = Settings.instance.displayStatus;
		}
		if (visualInterface == VisualInterface.Progress) {
			b = Settings.instance.displayProgress;
		}
		if (visualInterface == VisualInterface.Countdown) {
			b = Settings.instance.displayCountdown;
		}
		if (visualInterface == VisualInterface.Framerate) {
			b = Settings.instance.showFramerate;
		}
		if (visualInterface == VisualInterface.AnimateTargets) {
			b = Settings.instance.animateTargets;
		}
		if (visualInterface == VisualInterface.Environment3D) {
			b = Settings.instance.environment3D;
		}
		if (visualInterface == VisualInterface.Interface3D){
			b = Settings.instance.interface3D;
		}
		if (visualInterface == VisualInterface.RenderTexture2D){
			b = Settings.instance.renderTexture2D;
		}
		if (visualInterface == VisualInterface.ActionObservation) {
			b = Settings.instance.actionObservation;
		}
		if (visualInterface == VisualInterface.RecordTrajectory) {
			b = Settings.instance.recordTrajectory;
			//Debug.Log(b);
		}


		toggle.isOn = b;
		
		if (toggle.isOn) {
			toggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
		}
		else {
			toggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
		}
	}

	public void SetToggle() {
		if (toggle.isOn) {
			toggle.transform.GetChild(0).GetComponent<Image>().color = highlightedColour;
		}
		else {
			toggle.transform.GetChild(0).GetComponent<Image>().color = initialColour;
		}
		
		if (visualInterface == VisualInterface.CharacterVisible_C) {
			Settings.instance.SetCharacterVisible_C(toggle.isOn);
		}
		if (visualInterface == VisualInterface.CharacterVisible_L) {
			Settings.instance.SetCharacterVisible_L(toggle.isOn);
		}
		if (visualInterface == VisualInterface.CharacterVisible_T) {
			Settings.instance.SetCharacterVisible_T(toggle.isOn);
		}
		if (visualInterface == VisualInterface.CharacterVisible_R) {
			Settings.instance.SetCharacterVisible_R(toggle.isOn);
		}
		if (visualInterface == VisualInterface.CharacterVisible_B) {
			Settings.instance.SetCharacterVisible_B(toggle.isOn);
		}
		if (visualInterface == VisualInterface.ColourSmooth) {
			Settings.instance.SetColourSmoothing(toggle.isOn);
		}
		if (visualInterface == VisualInterface.CharacterSmooth) {
			Settings.instance.SetCharacterSmoothing(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Labels) {
			Settings.instance.SetLabelsVisible(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Rest) {
			Settings.instance.SetRestVisible(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Score) {
			Settings.instance.SetDisplayScore(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Status) {
			Settings.instance.SetDisplayStatus(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Progress) {
			Settings.instance.SetDisplayProgress(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Countdown) {
			Settings.instance.SetDisplayCountdown(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Framerate) {
			Settings.instance.SetShowFramerate(toggle.isOn);
		}
		if (visualInterface == VisualInterface.AnimateTargets) {
			Settings.instance.SetAnimateTargets(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Environment3D) {
			Settings.instance.Set3DEnvironment(toggle.isOn);
		}
		if (visualInterface == VisualInterface.Interface3D) {
			Settings.instance.SetInterface3D(toggle.isOn);
		}
		if (visualInterface == VisualInterface.RenderTexture2D){
			Settings.instance.SetRenderTexture2D(toggle.isOn);
		}
		if (visualInterface == VisualInterface.ActionObservation) {
			Settings.instance.SetActionObservation(toggle.isOn);
		}
		if (visualInterface == VisualInterface.RecordTrajectory) {
			Settings.instance.SetRecordTrajectory(toggle.isOn);
		}
	}
}
