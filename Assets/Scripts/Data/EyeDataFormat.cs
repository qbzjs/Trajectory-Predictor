using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EyeDataFormat 
{
    [Space(5)]
    public string tag;

    [Header("Eye Data")] 
    public bool blinking;
    
    public Vector3 gazeDirectionLeft;
    public Vector3 gazeDirectionRight;

    public float eyeOpennessLeft;
    public float eyeOpennessRight;

    public float pupilDiameterLeft;
    public float pupilDiameterRight;

    public float xCoord2D;
    public float yCoord2D;
    
    public EyeDataFormat(bool blinking, Vector3 gazeDirectionLeft,Vector3 gazeDirectionRight,
        float eyeOpennessLeft, float eyeOpennessRight, float pupilDiameterLeft, float pupilDiameterRight, float xCoord2D, float yCoord2D)
    {
        this.blinking = blinking;
        this.gazeDirectionLeft = gazeDirectionLeft;
        this.gazeDirectionRight = gazeDirectionRight;
        this.eyeOpennessLeft = eyeOpennessLeft;
        this.eyeOpennessRight = eyeOpennessRight;
        this.pupilDiameterLeft = pupilDiameterLeft;
        this.pupilDiameterRight = pupilDiameterRight;
        this.xCoord2D = xCoord2D;
        this.yCoord2D = yCoord2D;
    }
}
