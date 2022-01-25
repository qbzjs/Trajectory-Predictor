using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreDisplayUI : MonoBehaviour{
    
    public TextMeshProUGUI accuracyText;

    private float a;
    public float accuracy;
    
    #region Subscriptions

    private void OnEnable(){
        ScoreManager.OnScoreAction += ScoreManagerOnScoreAction;
    }
    private void OnDisable(){
        ScoreManager.OnScoreAction -= ScoreManagerOnScoreAction;
    }
    private void ScoreManagerOnScoreAction(float accuracy){
        a = accuracy;
    }

    #endregion
    void Start()
    {
        
    }
    
    void LateUpdate(){
        accuracy = Mathf.Lerp(accuracy, a, Time.deltaTime*2);
        accuracyText.text = "Accuracy : " + Mathf.RoundToInt(accuracy).ToString() + "%";
    }
}
