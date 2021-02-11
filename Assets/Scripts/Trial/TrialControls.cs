using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class TrialControls : MonoBehaviour
{
    public Button playButton;
    public Button stopButton;

    private Color initialColour;
    public Color highlightedColour;

    void Start () {
        initialColour = playButton.gameObject.GetComponent<Image>().color;
        SetStop();
    }

    public void SetPlay () {
        TrialSequence.instance.StartTrial();
    }
    public void SetStop () {

        TrialSequence.instance.StopTrial();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TrialSequence.instance.StartTrial();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TrialSequence.instance.StopTrial();
        }

        SetControls(Settings.instance.Status);
    }

    private void SetControls(GameStatus status) {
        switch (status) {
            case GameStatus.Ready :
                stopButton.image.color = highlightedColour;
                playButton.image.color = initialColour;
                break;
            case GameStatus.Running :
                stopButton.image.color = initialColour;
                playButton.image.color = highlightedColour;
                break;
            case GameStatus.Complete :
                stopButton.image.color = highlightedColour;
                playButton.image.color = initialColour;
                break;
        }
    }
    
}
