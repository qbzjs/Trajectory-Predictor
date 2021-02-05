using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

public class SetCharacterType : MonoBehaviour {

	public CharacterColourType colourType;

	public Button dynamicButton;
	public Button solidButton;

	private Color initialColour;
	public Color highlightedColour;
	
	void Start () {
		initialColour = dynamicButton.image.color;
		Initialise();
	}

	private void Initialise() {
		colourType = Settings.instance.characterColourType;
		if (colourType == CharacterColourType.Static) {
			dynamicButton.image.color = initialColour;
			solidButton.image.color = highlightedColour;
		}
		if (colourType == CharacterColourType.Dynamic) {
			dynamicButton.image.color = highlightedColour;
			solidButton.image.color = initialColour;
		}
	}

	public void SetDynamic () {
		solidButton.image.color = initialColour;
		dynamicButton.image.color = highlightedColour;
		Settings.instance.SetCharacterColourType(CharacterColourType.Dynamic);
	}
	public void SetStatic () {
		solidButton.image.color = highlightedColour;
		dynamicButton.image.color = initialColour;
		Settings.instance.SetCharacterColourType(CharacterColourType.Static);
	}
}
