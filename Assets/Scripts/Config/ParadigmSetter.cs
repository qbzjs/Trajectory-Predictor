using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

/// <summary>
/// sets positions and location for the reach target paradigm
/// </summary>
public class ParadigmSetter : MonoBehaviour {

	public TrialParadigm paradigm;


	private Color initialColour;
	public Color highlightedColour;
	
	void Start () {
		Initialise();
	}

	private void Initialise() {
		paradigm = Settings.instance.trialParadigm;

		if (paradigm == TrialParadigm.CentreOut){
		}

		if (paradigm == TrialParadigm.Vertical){
		}

		if (paradigm == TrialParadigm.Horizontal){
		}

		if (paradigm == TrialParadigm.Circle){
		}

	}
}
