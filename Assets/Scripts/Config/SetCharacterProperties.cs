using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;

public class SetCharacterProperties : MonoBehaviour {

	public Slider colourSlider;
	public Slider smoothSlider;
	public TextMeshProUGUI colourText;
	public TextMeshProUGUI smoothText;

	public int colourSpeed;
	public int smoothSpeed;
	
	void Start () {
		InitialiseSliders();
	}
	
	public void SetColourSpeed () {
		colourSpeed = Mathf.RoundToInt(colourSlider.value);
		colourText.text = colourSlider.value.ToString();
		Settings.instance.SetColourSpeed(colourSpeed);
	}
	public void SetSmoothSpeed () {
		smoothSpeed = Mathf.RoundToInt(smoothSlider.value);
		smoothText.text = smoothSlider.value.ToString();
		Settings.instance.SetSmoothSpeed(smoothSpeed);
	}

	public void InitialiseSliders() {
		colourSlider.value = Settings.instance.colourSpeed;
		smoothSlider.value = Settings.instance.smoothSpeed;
		colourText.text = colourSlider.value.ToString();
		smoothText.text = smoothSlider.value.ToString();
	}
}
