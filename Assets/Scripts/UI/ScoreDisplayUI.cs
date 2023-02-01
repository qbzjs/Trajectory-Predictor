using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreDisplayUI : MonoBehaviour
{
    [Range(1f, 10f)] 
    public float updateSpeed = 4f; 
    
    //UI
    [Header("OVERALL")]
    public Slider performanceSlider;
    public TextMeshProUGUI performanceText;
    public TextMeshProUGUI streakBonusText;
    
    
    [Header("TARGETS HIT")]
    public Image kinDistanceFill;
    public Slider kinematicDistanceSlider;
    public TextMeshProUGUI kinematicDistanceText;
    public TextMeshProUGUI t1_Kin;
    public TextMeshProUGUI t2_Kin;
    public TextMeshProUGUI t3_Kin;
    public TextMeshProUGUI t4_Kin;
    
    public Image imgDistanceFill;
    public Slider BCI_DistanceSlider;
    public TextMeshProUGUI BCI_DistanceText;
    public TextMeshProUGUI t1_Img;
    public TextMeshProUGUI t2_Img;
    public TextMeshProUGUI t3_Img;
    public TextMeshProUGUI t4_Img;

    
    [Header("CORRELATION")]
    public Slider kinematicCorrelationSliderX;
    public Slider kinematicCorrelationSliderY;
    public Slider kinematicCorrelationSliderZ;
    public TextMeshProUGUI kinematicCorrelationTextX;
    public TextMeshProUGUI kinematicCorrelationTextY;
    public TextMeshProUGUI kinematicCorrelationTextZ;
    public Slider BCI_CorrelationSliderX;
    public Slider BCI_CorrelationSliderY;
    public Slider BCI_CorrelationSliderZ;
    public TextMeshProUGUI BCI_CorrelationTextX;
    public TextMeshProUGUI BCI_CorrelationTextY;
    public TextMeshProUGUI BCI_CorrelationTextZ;
    
    public Slider kinematicCorrelationAvgSliderX;
    public Slider kinematicCorrelationAvgSliderY;
    public Slider kinematicCorrelationAvgSliderZ;
    public TextMeshProUGUI kinematicCorrelationAvgText;

    public Slider BCI_CorrelationAvgSliderX;
    public Slider BCI_CorrelationAvgSliderY;
    public Slider BCI_CorrelationAvgSliderZ;
    public TextMeshProUGUI BCI_CorrelationAvgText;
    
    
    
    [Header("-----------------")]
    //scores
    public float performance;
    private float p;
    public int streakBonus;
    
    public float kinematicDistance;
    private float kd;
    public float BCI_Distance;
    private float bd;
    public TargetHitFormat targetsHit;
    
    
    public Vector3 kinematicCorrelation;
    private Vector3 kc;
    private float kcx;private float kcy;private float kcz;
    public Vector3 BCI_Correlation;
    private Vector3 bc;
    private float bcx;private float bcy;private float bcz;
    public float kinematicCorrelationAvg;
    private float kcavg;
    public float BCI_CorrelationAvg;
    private float bcavg;

    
    #region Subscriptions
    private void OnEnable(){
        ScoreManager.OnScoreSessionObjectAction += ScoreManagerOnScoreSessionObjectAction;
    }
    private void OnDisable(){
        ScoreManager.OnScoreSessionObjectAction -= ScoreManagerOnScoreSessionObjectAction;
    }
    private void ScoreManagerOnScoreSessionObjectAction(ScoreSessionDataObject sessionScoreData){
        // performance = ScoreManager.instance.overallPerformanceSession;
        performance = sessionScoreData.overallPerformance;
        streakBonus = sessionScoreData.streakBonus;
        kinematicDistance = sessionScoreData.distanceAccuracyKin;
        BCI_Distance = sessionScoreData.distanceAccuracyBCI_Assisted;
        kinematicCorrelation = sessionScoreData.correlationKin;
        BCI_Correlation = sessionScoreData.correlationBCI_Assisted;
        kinematicCorrelationAvg = sessionScoreData.correlationKinAvg;
        BCI_CorrelationAvg = sessionScoreData.correlationBCIAvg_Assisted;
//        print("skfjgnslofdgnslodgnsold");
    }
    #endregion
    
    void LateUpdate(){
        // accuracyKin = Mathf.Lerp(accuracyKin, aKin, Time.deltaTime*2);
        // accuracyTextKinematic.text = "Kinematic Distance Accuracy : " + Mathf.RoundToInt(accuracyKin).ToString() + "%";

        //overall
        p = Mathf.Lerp(p, performance, Time.deltaTime * updateSpeed);
        performanceSlider.value = p;
        performanceText.text = Mathf.RoundToInt(p).ToString()+"%";

        streakBonusText.text = "Streak Bonus : " + streakBonus.ToString();

        
        //distance accuracy
        kd = Mathf.Lerp(kd, kinematicDistance, Time.deltaTime * updateSpeed);
        kinematicDistanceSlider.value = kd;
        kinDistanceFill.rectTransform.localScale = new Vector3(kd/100, kd/100, 1); //check this
        kinematicDistanceText.text = Mathf.RoundToInt(kd).ToString()+"%";
        bd = Mathf.Lerp(bd, BCI_Distance, Time.deltaTime * updateSpeed);
        BCI_DistanceSlider.value = bd;
        imgDistanceFill.rectTransform.localScale = new Vector3(bd/100, bd/100, 1); //check this
        BCI_DistanceText.text = Mathf.RoundToInt(bd).ToString()+"%";
        
        //targethit accuracy
        targetsHit = ScoreManager.instance.targetsHitSession; //assign the data format
        t1_Kin.text = targetsHit.t1_kin.ToString();
        t2_Kin.text = targetsHit.t2_kin.ToString();
        t3_Kin.text = targetsHit.t3_kin.ToString();
        t4_Kin.text = targetsHit.t4_kin.ToString();
        t1_Img.text = targetsHit.t1_img.ToString();
        t2_Img.text = targetsHit.t2_img.ToString();
        t3_Img.text = targetsHit.t3_img.ToString();
        t4_Img.text = targetsHit.t4_img.ToString();
        
        //correlation
        kc = Vector3.Lerp(kc,kinematicCorrelation, Time.deltaTime * updateSpeed);
        kinematicCorrelationSliderX.value = kc.x;
        kinematicCorrelationSliderY.value = kc.y;
        kinematicCorrelationSliderZ.value = kc.z;
        kinematicCorrelationTextX.text = Mathf.RoundToInt(kc.x).ToString()+"%";
        kinematicCorrelationTextY.text = Mathf.RoundToInt(kc.y).ToString()+"%";
        kinematicCorrelationTextZ.text = Mathf.RoundToInt(kc.z).ToString()+"%";
        
        bc = Vector3.Lerp(bc,BCI_Correlation, Time.deltaTime * updateSpeed);
        BCI_CorrelationSliderX.value = bc.x;
        BCI_CorrelationSliderY.value = bc.y;
        BCI_CorrelationSliderZ.value = bc.z;
        BCI_CorrelationTextX.text = Mathf.RoundToInt(bc.x).ToString()+"%";
        BCI_CorrelationTextY.text = Mathf.RoundToInt(bc.y).ToString()+"%";
        BCI_CorrelationTextZ.text = Mathf.RoundToInt(bc.z).ToString()+"%";
        
        kcavg = Mathf.Lerp(kcavg, kinematicCorrelationAvg, Time.deltaTime * updateSpeed);
        kinematicCorrelationAvgSliderX.value = kcavg;
        kinematicCorrelationAvgSliderY.value = kcavg;
        kinematicCorrelationAvgSliderZ.value = kcavg;
        kinematicCorrelationAvgText.text = Mathf.RoundToInt(kcavg).ToString()+"%";
        
        bcavg = Mathf.Lerp(bcavg, BCI_CorrelationAvg, Time.deltaTime * updateSpeed);
        BCI_CorrelationAvgSliderX.value = bcavg;
        BCI_CorrelationAvgSliderY.value = bcavg;
        BCI_CorrelationAvgSliderZ.value = bcavg;
        BCI_CorrelationAvgText.text = Mathf.RoundToInt(bcavg).ToString()+"%";
    }
}
