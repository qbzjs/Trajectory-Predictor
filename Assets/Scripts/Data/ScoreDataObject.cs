using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreDataObject 
{

    //SESSION DETAILS
    public string name;
    public int sessionNumber;
    
    public string run;
    public string block;
    public string runType;
    
    public int assistancePercentage;
    
    //DISTANCE
    public float distanceAccuracyKinematic;
    public float distanceAccuracyBCI;
    public float distanceAccuracyBCI_Unassisted;
    
    //DIFFERENCE / CORRELATION
    public Vector3 correlationKinematic;
    public Vector3 correlationAssistedKinematic;
    
    public Vector3 correlationBCI;
    public Vector3 correlationAssistedBCI;

    public ScoreDataObject(string name, int sessionNumber, string run, string block, string runType,
        int assistancePercentage, float distanceAccuracyKinematic,float distanceAccuracyBci, float distanceAccuracyBCI_Unassisted,
        Vector3 correlationKinematic, Vector3 correlationAssistedKinematic, Vector3 correlationBCI, Vector3 correlationAssistedBCI)
    {
        this.name = name;
        this.sessionNumber = sessionNumber;

        this.run = run;
        this.block = block;
        this.runType = runType;
        
        this.assistancePercentage = assistancePercentage;
        this.distanceAccuracyKinematic = distanceAccuracyKinematic;
        this.distanceAccuracyBCI = distanceAccuracyBci;
        this.distanceAccuracyBCI_Unassisted = distanceAccuracyBCI_Unassisted;

        this.correlationKinematic = correlationKinematic;
        this.correlationAssistedKinematic = correlationAssistedKinematic;
        this.correlationBCI = correlationBCI;
        this.correlationAssistedBCI = correlationAssistedBCI;
    }
}
