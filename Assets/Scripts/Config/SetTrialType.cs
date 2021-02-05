using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class SetTrialType : MonoBehaviour {

	public TrialType trialType;

	public Button centreOutButton;
	public Button fourButton;
	public Button threeButton;

	private Color initialColour;
	public Color highlightedColour;
	
	void Start () {
		initialColour = fourButton.image.color;
		Initialise();
	}

	private void Initialise() {
		trialType = Settings.instance.trialType;
		
		if (trialType == TrialType.Four_Targets) {
			centreOutButton.image.color = initialColour;
			threeButton.image.color = initialColour;
			fourButton.image.color = highlightedColour;
		}
		if(trialType == TrialType.Three_Targets){
			threeButton.image.color = highlightedColour;
			fourButton.image.color = initialColour;
			centreOutButton.image.color = initialColour;
		}
		if(trialType == TrialType.CentreOut){
			centreOutButton.image.color = highlightedColour;
			fourButton.image.color = initialColour;
			threeButton.image.color = initialColour;
		}
	}
	public void SetCentreOut () {
		threeButton.image.color = initialColour;
		fourButton.image.color = initialColour;
		centreOutButton.image.color = highlightedColour;
		Settings.instance.SetTrialType(TrialType.CentreOut);
	}
	public void SetFour () {
		threeButton.image.color = initialColour;
		centreOutButton.image.color = initialColour;
		fourButton.image.color = highlightedColour;
		Settings.instance.SetTrialType(TrialType.Four_Targets);
	}
	public void SetThree () {
		threeButton.image.color = highlightedColour;
		fourButton.image.color = initialColour;
		centreOutButton.image.color = initialColour;
		Settings.instance.SetTrialType(TrialType.Three_Targets);
	}
}
