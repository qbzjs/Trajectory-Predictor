using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class SetSequenceType : MonoBehaviour
{
    public SequenceType sequenceType;

    public Button linearButton;
    public Button permutationButton;

    private Color initialColour;
    public Color highlightedColour;

    void Start()
    {
        initialColour = linearButton.image.color;
        Initialise();
    }

    private void Initialise()
    {
        sequenceType = Settings.instance.sequenceType;

        if (sequenceType == SequenceType.Linear)
        {
            linearButton.image.color = highlightedColour;
            permutationButton.image.color = initialColour;
        }
        if (sequenceType == SequenceType.Permutation)
        {
            linearButton.image.color = initialColour;
            permutationButton.image.color = highlightedColour;
        }
    }
    public void SetLinear()
    {
        linearButton.image.color = highlightedColour;
        permutationButton.image.color = initialColour;
        Settings.instance.SetSequenceType(SequenceType.Linear);
    }
    public void SetPermutation()
    {
        linearButton.image.color = initialColour;
        permutationButton.image.color = highlightedColour;
        Settings.instance.SetSequenceType(SequenceType.Permutation);
    }
}

