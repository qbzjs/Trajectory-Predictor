using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SetTaskSide : MonoBehaviour
{
	public Handedness handedness;

	public Button leftButton;
	public Button rightButton;

	private Color initialColour;
	public Color highlightedColour;

	void Start()
	{
		initialColour = leftButton.image.color;
		Initialise();
	}

	private void Initialise()
	{
		handedness = Settings.instance.handedness;

		if (handedness == Handedness.Left)
		{
			leftButton.image.color = highlightedColour;
			rightButton.image.color = initialColour;
		}
		if (handedness == Handedness.Right)
		{
			leftButton.image.color = initialColour;
			rightButton.image.color = highlightedColour;
		}
	}
	public void SetLeft()
	{
		leftButton.image.color = highlightedColour;
		rightButton.image.color = initialColour;
		Settings.instance.SetHandedness(Handedness.Left);
	}
	public void SetRight()
	{
		leftButton.image.color = initialColour;
		rightButton.image.color = highlightedColour;
		Settings.instance.SetHandedness(Handedness.Right);
	}
}
