using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreSessionDataObject 
{

    //SESSION DETAILS
    public string name;
    public int sessionNumber;
    
    public float assistanceDecrease;
    
    //DISTANCE
    public float distanceAccuracyKin;
    public float distanceAccuracyBCI_Assisted;
    public float distanceAccuracyBCI_Unassisted;
    
    //CORRELATION
    public Vector3 correlationKin;
    public float correlationKinAvg;
    public Vector3 correlationBCI_Assisted;
    public float correlationBCIAvg_Assisted;
    public Vector3 correlationBCI_Unassisted;
    public float correlationBCIAvg_Unassisted;
    
    //MEAN SQUARE ERROR
    public Vector3 meanSqErrorSum;
    public Vector3 meanSqErrorSumAssisted;
    public Vector3 meanSquareErrorAverage;
    public Vector3 meanSquareErrorAverageAssisted;
    
    //Performance Average
    public float overallPerformance;
    public int streakBonus;

    public ScoreSessionDataObject(string name, int sessionNumber, int assistanceDecrease, 
        float distanceAccuracyKin,float distanceAccuracyBCI_Assisted,float distanceAccuracyBCI_Unassisted,
        Vector3 correlationKin,float correlationKinAvg,
        Vector3 correlationBCI_Assisted,float correlationBCIAvg_Assisted,
        Vector3 correlationBCI_Unassisted,float correlationBCIAvg_Unassisted,
        Vector3 meanSqErrorSum,Vector3 meanSqErrorSumAssisted,
        Vector3 meanSquareErrorAverage,Vector3 meanSquareErrorAverageAssisted,
        float overallPerformance, int streakBonus)
    {
        this.name = name;
        this.sessionNumber = sessionNumber;
        this.assistanceDecrease = assistanceDecrease;

        this.distanceAccuracyKin = distanceAccuracyKin;
        this.distanceAccuracyBCI_Assisted = distanceAccuracyBCI_Assisted;
        this.distanceAccuracyBCI_Unassisted = distanceAccuracyBCI_Unassisted;

        this.correlationKin = correlationKin;
        this.correlationKinAvg = correlationKinAvg;
        this.correlationBCI_Assisted = correlationBCI_Assisted;
        this.correlationBCIAvg_Assisted = correlationBCIAvg_Assisted;
        this.correlationBCI_Unassisted = correlationBCI_Unassisted;
        this.correlationBCIAvg_Unassisted = this.correlationBCIAvg_Assisted;

        this.meanSqErrorSum = meanSqErrorSum;
        this.meanSqErrorSumAssisted = meanSqErrorSumAssisted;
        this.meanSquareErrorAverage = meanSquareErrorAverage;
        this.meanSquareErrorAverageAssisted = meanSquareErrorAverageAssisted;
        this.overallPerformance = overallPerformance;
        this.streakBonus = streakBonus;
    }
}
