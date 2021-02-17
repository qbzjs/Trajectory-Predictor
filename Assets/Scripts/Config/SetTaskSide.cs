using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SetTaskSide : MonoBehaviour
{
	public TaskSide taskSide;

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
		taskSide = Settings.instance.taskSide;

		if (taskSide == TaskSide.Left)
		{
			leftButton.image.color = highlightedColour;
			rightButton.image.color = initialColour;
		}
		if (taskSide == TaskSide.Right)
		{
			leftButton.image.color = initialColour;
			rightButton.image.color = highlightedColour;
		}
	}
	public void SetLeft()
	{
		leftButton.image.color = highlightedColour;
		rightButton.image.color = initialColour;
		Settings.instance.SetReachTaskSide(TaskSide.Left);
	}
	public void SetRight()
	{
		leftButton.image.color = initialColour;
		rightButton.image.color = highlightedColour;
		Settings.instance.SetReachTaskSide(TaskSide.Right);
	}
}
