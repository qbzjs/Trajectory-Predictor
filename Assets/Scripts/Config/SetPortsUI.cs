using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPortsUI : MonoBehaviour {

	public InputField IP;
	public InputField listen;
	public InputField send;
	
	void Start () {
		InitialiseInputFields();
	}
	
	public void SetIP () {
		Settings.instance.SetIP(IP.text);
	}
	public void SetListen () {
		Settings.instance.SetListen(int.Parse(listen.text));
	}
	public void SetSend() {
		Settings.instance.SetSend(int.Parse(send.text));
	}

	private void InitialiseInputFields() {
		IP.text = Settings.instance.ip;
		listen.text = Settings.instance.portListen.ToString();
		send.text = Settings.instance.portSend.ToString();
	}
}
