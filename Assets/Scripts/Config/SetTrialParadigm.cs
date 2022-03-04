using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// selects the paradigm in the UI
/// </summary>

public class SetTrialParadigm : MonoBehaviour
{
	public TrialParadigm paradigm;

	public TextMeshProUGUI paradigmTextUI;
	public Button fourButton;
	public Button threeButton;

	private Color initialColour;
	public Color highlightedColour;
	
	void Start () {
		// initialColour = centreOutButton.image.color;
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
