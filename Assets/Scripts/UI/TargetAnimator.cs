using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAnimator : MonoBehaviour {

	private GameObject target;

	private Vector3 initialScale;
	
	void Awake () {
		target = gameObject;
		initialScale = target.transform.localScale;
	}

	public void ScaleTarget () {
		LeanTween.scale( target, new Vector3(initialScale.x/2, initialScale.y/2, initialScale.z/2f), 0).setEase(LeanTweenType.easeInOutSine);
		StartCoroutine(ScaleToDefault());
	}
	private IEnumerator ScaleToDefault() {
//		yield return new WaitForSeconds(0.11f);
		yield return new WaitForEndOfFrame();
		LeanTween.scale( target, new Vector3(initialScale.x, initialScale.y, initialScale.z), Settings.instance.targetDuration).setEase(LeanTweenType.easeInOutSine);
	}
}
