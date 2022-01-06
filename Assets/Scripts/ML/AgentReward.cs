using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentReward : MonoBehaviour{
	
	public bool homePosition;
	public Material defaultMaterial;
	public Material rewardMaterial;

	private void Awake(){
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	public void SetCollider(bool t){
		gameObject.GetComponent<Collider>().enabled = t;
	}
	public void DisplayReward(){
		Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		gameObject.GetComponent<MeshRenderer>().material = rewardMaterial;
		StartCoroutine(WaitAndReset());
	}

	public bool HomePosition(){
		return homePosition;
	}

	private IEnumerator WaitAndReset(){
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
	}
}
