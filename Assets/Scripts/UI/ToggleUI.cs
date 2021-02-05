using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class ToggleUI : MonoBehaviour {

	public InterfaceElement interfaceElement;
	public Toggle toggle;
	private Color initialColour;
	public Color highlightedColour;
	
	void Start() {
		initialColour = toggle.transform.GetChild(0).GetComponent<Image>().color;
		Initialise();
	}

	public void Initialise() {
		bool b = true;
		//TODO character could be moved to character class!!
		if (interfaceElement == InterfaceElement.CharacterVisible_C) {
			b = Settings.instance.characterVisible_C;
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_L) {
			b = Settings.instance.characterVisible_L;
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_T) {
			b = Settings.instance.characterVisible_T;
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_R) {
			b = Settings.instance.characterVisible_R;
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_B) {
			b = Settings.instance.characterVisible_B;
		}
		if (interfaceElement == InterfaceElement.ColourSmooth) {
			b = Settings.instance.colourSmoothing;
		}
		if (interfaceElement == InterfaceElement.CharacterSmooth) {
			b = Settings.instance.characterSmoothing;
		}
		if (interfaceElement == InterfaceElement.AnimateTargets) {
			b = Settings.instance.animateTargets;
		}
		if (interfaceElement == InterfaceElement.Labels) {
			b = Settings.instance.labelsVisible;
		}
		if (interfaceElement == InterfaceElement.Rest) {
			b = Settings.instance.restVisible;
		}
		if (interfaceElement == InterfaceElement.Score) {
			b = Settings.instance.displayScore;
		}
		if (interfaceElement == InterfaceElement.Status) {
			b = Settings.instance.displayStatus;
		}
		if (interfaceElement == InterfaceElement.Progress) {
			b = Settings.instance.displayProgress;
		}
		if (interfaceElement == InterfaceElement.Framerate) {
			b = Settings.instance.showFramerate;
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

		//TODO character could be moved to character class!!
		if (interfaceElement == InterfaceElement.CharacterVisible_C) {
			Settings.instance.SetCharacterVisible_C(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_L) {
			Settings.instance.SetCharacterVisible_L(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_T) {
			Settings.instance.SetCharacterVisible_T(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_R) {
			Settings.instance.SetCharacterVisible_R(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.CharacterVisible_B) {
			Settings.instance.SetCharacterVisible_B(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.ColourSmooth) {
			Settings.instance.SetColourSmoothing(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.CharacterSmooth) {
			Settings.instance.SetCharacterSmoothing(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.AnimateTargets) {
			Settings.instance.SetAnimateTargets(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Labels) {
			Settings.instance.SetLabelsVisible(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Rest) {
			Settings.instance.SetRestVisible(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Score) {
			Settings.instance.SetDisplayScore(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Status) {
			Settings.instance.SetDisplayStatus(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Progress) {
			Settings.instance.SetDisplayProgress(toggle.isOn);
		}
		if (interfaceElement == InterfaceElement.Framerate) {
			Settings.instance.SetShowFrametrate(toggle.isOn);
		}
	}
}
