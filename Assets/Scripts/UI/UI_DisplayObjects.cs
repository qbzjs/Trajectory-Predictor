﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DisplayObjects : MonoBehaviour {

	[Header("Objects")]
	public GameObject TrialUI;
	public GameObject settingsPanel;
	public GameObject trial2D;
	public GameObject trial2D_RenderTexture;

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
	public GameObject progressDisplay;
	public GameObject FPS;
	public GameObject rest;
	public GameObject outerLines;
	public GameObject innerLinesA;
	public GameObject innerLinesB;
	public GameObject ringOuter;
	

}
