using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreDataObject 
{

    public string name;
    public int sessionNumber;
    public int assistancePercentage;
    public float accuracyKinematic;
    public float accuracyBCI;
    public float accuracyBCI_unassisted;

    public ScoreDataObject(string name, int sessionNumber, int assistancePercentage, float accuracyKinematic,float accuracyBCI, float accuracyBCI_Unassisted){
        this.name = name;
        this.sessionNumber = sessionNumber;
        this.assistancePercentage = assistancePercentage;
        this.accuracyKinematic = accuracyKinematic;
        this.accuracyBCI = accuracyBCI;
        this.accuracyBCI_unassisted = accuracyBCI_Unassisted;
    }
}
