using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class TrialControls : MonoBehaviour
{
    public static TrialControls instance;
    public Button playButton;
    public Button stopButton;

    private Color initialColour;
    public Color highlightedColour;

    private void Awake()
    {
        instance = this;
    }
    void Start () {
        initialColour = playButton.gameObject.GetComponent<Image>().color;
        //SetStop();
    }

    public void SetPlay () {
        InputManager.instance.SetTrajectoryRecording(true);
        InputManager.instance.StartTrialSequence();
        
        SetControls(Settings.instance.Status);

        //SendUDP_byte(101);
        //SendUDP_byte(0);
    }
    public void SetStop () {
        InputManager.instance.StopTrialSequence();
        InputManager.instance.SetTrajectoryRecording(false);
        
        SetControls(Settings.instance.Status);

        //SendUDP_byte(102);
        //SendUDP_byte(0);
    }
    private void SendUDP_byte(int t)
    {
        Debug.Log("Trigger Sent: " + t);
        UDPClient.instance.SendData((byte)t);

    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
//            SetPlay();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
 //           SetStop();
        }

  //      SetControls(Settings.instance.Status);
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
