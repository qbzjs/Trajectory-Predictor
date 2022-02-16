using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreBlockDataObject 
{

    //SESSION DETAILS
    public string name;
    public int sessionNumber;
    
    public string run;
    public string block;
    public string runType;
    
    public float assistancePercentage;
    
    //DISTANCE
    public float distanceAccuracyKinematic;
    public float distanceAccuracyBCI_Assisted;
    public float distanceAccuracyBCI_Unassisted;
    
    //DIFFERENCE / CORRELATION
    public Vector3 correlationUnassisted;
    public Vector3 correlationAssisted;
    
    public Vector3 correlationDisplay; //limited to ui
    public Vector3 correlationAssistedDisplay; //limited to ui
 
    public ScoreBlockDataObject(string name, int sessionNumber, string run, string block, string runType,
        int assistancePercentage, float distanceAccuracyKinematic,float distanceAccuracyBCI_Assisted, float distanceAccuracyBCI_Unassisted,
        Vector3 correlationUnassisted, Vector3 correlationAssisted, Vector3 correlationDisplay, Vector3 correlationAssistedDisplay)
    {
        this.name = name;
        this.sessionNumber = sessionNumber;

        this.run = run;
        this.block = block;
        this.runType = runType;
        
        this.assistancePercentage = assistancePercentage;
        
        this.distanceAccuracyKinematic = distanceAccuracyKinematic;
        this.distanceAccuracyBCI_Assisted = distanceAccuracyBCI_Assisted;
        this.distanceAccuracyBCI_Unassisted = distanceAccuracyBCI_Unassisted;

        this.correlationUnassisted = correlationUnassisted;
        this.correlationAssisted = correlationAssisted;
        this.correlationDisplay = correlationDisplay;
        this.correlationAssistedDisplay = correlationAssistedDisplay;
    }
}
