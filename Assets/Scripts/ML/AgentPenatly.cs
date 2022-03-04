using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPenatly : MonoBehaviour
{
	public Material defaultMaterial;
	public Material penatlyMaterial;

	private void Awake(){
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	public void DisplayPenatly(){
		gameObject.GetComponent<MeshRenderer>().material = penatlyMaterial;
		StartCoroutine(WaitAndReset());
	}

	private IEnumerator WaitAndReset(){
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
	}
}
