using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

public class ControlCharacter : MonoBehaviour {

	public static ControlCharacter instance;

	public Transform characterCentre;
	public Transform characterLeft;
	public Transform characterTop;
	public Transform characterRight;
	public Transform characterBottom;

	public bool smoothColour = false;
	public bool smoothCharacter = false;
	public CharacterColourType characterColourType = CharacterColourType.Static;
	
	[Range(1,20)]
	public float colourSpeed = 15f;
	[Range(1,20)]
	public float smoothSpeed = 15f;
	
	
	
	public Vector3 XY_Main;
	public Vector3 XY_LR;
	public Vector3 XY_FR;
	public Vector3 XY_LF;
	[Range(-1,1)]
	public float TX;

	private Material controlMaterialMain;
	private Material controlMaterialLR;
	private Material controlMaterialFR;
	private Material controlMaterialLF;
	
	public Color defaultColour;
	public Color positiveColour;
	public Color negativeColour;
	public Color targetColour;
	
	private void OnEnable() {
		UDPClient.OnDataReceivedXY += UdpClientOnDataReceivedXy;
		UDPClient.OnDataReceivedDoubles += UdpClientOnDataReceivedDoubles;
	}
	private void OnDisable() {
		UDPClient.OnDataReceivedXY -= UdpClientOnDataReceivedXy;
		UDPClient.OnDataReceivedDoubles -= UdpClientOnDataReceivedDoubles;
	}
	private void UdpClientOnDataReceivedDoubles(float lr, float fr, float lf, float tx) {
		TX = tx;
	}
	private void UdpClientOnDataReceivedXy(Vector2 xy,Vector2 xyLR,Vector2 xyFR,Vector2 xyLF) {
		XY_Main = new Vector3(xy.x,xy.y,transform.position.z);
		XY_LR = new Vector3(xyLR.x,xyLR.y,transform.position.z);
		XY_FR = new Vector3(xyFR.x,xyFR.y,transform.position.z);
		XY_LF = new Vector3(xyLF.x,xyLF.y,transform.position.z);
	}
	
	void Awake () {
		instance = this;
		
		controlMaterialMain = characterCentre.gameObject.GetComponent<Renderer>().material;
		controlMaterialMain.color = defaultColour;
		targetColour = defaultColour;
	}

	private void Start() {
		characterColourType = Settings.instance.characterColourType;
		colourSpeed = Settings.instance.colourSpeed;
		smoothSpeed = Settings.instance.smoothSpeed;
	}

	void Update (){
		if (!smoothCharacter) {
			characterCentre.position = new Vector3(XY_Main.x,XY_Main.y,XY_Main.z);
			characterLeft.position = new Vector3(XY_LR.x,XY_LR.y,XY_LR.z);
			characterTop.position = new Vector3(XY_FR.x,XY_FR.y,XY_FR.z);
			characterRight.position = new Vector3(XY_LF.x,XY_LF.y,XY_LF.z);
			characterBottom.position = Vector3.zero;
		}
		else {
			characterCentre.position = Vector3.Lerp(characterCentre.position, XY_Main, Time.deltaTime * smoothSpeed);
			characterLeft.position = Vector3.Lerp(characterLeft.position, XY_LR, Time.deltaTime * smoothSpeed);
			characterTop.position = Vector3.Lerp(characterTop.position, XY_FR, Time.deltaTime * smoothSpeed);
			characterRight.position = Vector3.Lerp(characterRight.position, XY_LF, Time.deltaTime * smoothSpeed);
			characterBottom.position = Vector3.Lerp(characterBottom.position, Vector3.zero, Time.deltaTime * smoothSpeed);
		}

		if (characterColourType == CharacterColourType.Dynamic) {
			if (TX < -0.1F) {
				targetColour = negativeColour;
			}
			if (TX > 0.1F) {
				targetColour = positiveColour;
			}

			if (TX >= -0.1F && TX <= 0.1F) {
				targetColour = defaultColour;
			}

			if (smoothColour) {
				controlMaterialMain.color = Color.Lerp(controlMaterialMain.color, targetColour, Time.deltaTime*colourSpeed);
			}
			else {
				controlMaterialMain.color = targetColour;
			}
			
		}
		if (characterColourType == CharacterColourType.Static) {
			targetColour = defaultColour;
			controlMaterialMain.color = Color.Lerp(controlMaterialMain.color, targetColour, Time.deltaTime*colourSpeed);
		}
	}
}
