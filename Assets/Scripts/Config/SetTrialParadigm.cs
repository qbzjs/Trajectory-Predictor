using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SetTrialParadigm : MonoBehaviour
{
	public TrialParadigm trialParadigm;

	public Button avatarButton;
	public Button screenButton;

	private Color initialColour;
	public Color highlightedColour;

	void Start()
	{
		initialColour = avatarButton.image.color;
		Initialise();
	}

	private void Initialise()
	{
		trialParadigm = Settings.instance.trialParadigm;

		if (trialParadigm == TrialParadigm.Avatar3D)
		{
			avatarButton.image.color = highlightedColour;
			screenButton.image.color = initialColour;
		}
		if (trialParadigm == TrialParadigm.Screen2D)
		{
			avatarButton.image.color = initialColour;
			screenButton.image.color = highlightedColour;
		}
	}
	public void SetAvatar3D()
	{
		avatarButton.image.color = highlightedColour;
		screenButton.image.color = initialColour;
		Settings.instance.SetTrialParadigm(TrialParadigm.Avatar3D);
	}
	public void SetScreen2D()
	{
		avatarButton.image.color = initialColour;
		screenButton.image.color = highlightedColour;
		Settings.instance.SetTrialParadigm(TrialParadigm.Screen2D);
	}
}
