using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreDisplayUI : MonoBehaviour{
    
    public TextMeshProUGUI accuracyTextKinematic;
    public TextMeshProUGUI accuracyTextBCI;

    private float aKin;
    public float accuracyKin;
    private float aBCI;
    public float accuracyBCI;
    
    #region Subscriptions

    private void OnEnable(){
        ScoreManager.OnScoreAction += ScoreManagerOnScoreAction;
    }
    private void OnDisable(){
        ScoreManager.OnScoreAction -= ScoreManagerOnScoreAction;
    }
    private void ScoreManagerOnScoreAction(float accuracyKin, float accuracyBCI){
        aKin = accuracyKin;
        aBCI = accuracyBCI;
    }

    #endregion
    void Start()
    {
        
    }
    
    void LateUpdate(){
        accuracyKin = Mathf.Lerp(accuracyKin, aKin, Time.deltaTime*2);
        accuracyTextKinematic.text = "Motion Accuracy : " + Mathf.RoundToInt(accuracyKin).ToString() + "%";
        accuracyBCI = Mathf.Lerp(accuracyBCI, aBCI, Time.deltaTime*2);
        accuracyTextBCI.text = "BCI Accuracy : " + Mathf.RoundToInt(accuracyBCI).ToString() + "%";
    }
}
