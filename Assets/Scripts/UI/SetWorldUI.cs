using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class SetWorldUI : MonoBehaviour{
    
    public static SetWorldUI instance;

    public GameObject worldUI;
    public GameObject trialRender2D;

    public GameObject outerRing;
    public GameObject outerLines;
    public GameObject innerLinesA;
    public GameObject innerLinesB;

    private void OnEnable(){
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (Settings.instance.interface3D == true){
            if (eventType == GameStatus.VisibleCountdown){
                worldUI.SetActive(false);
            }
            if (eventType == GameStatus.Ready){
                worldUI.SetActive(true);
            }
            if (eventType == GameStatus.BlockComplete){
                worldUI.SetActive(true);
            }
        }
    }

    void Awake(){
        instance = this;
    }
    void Start()
    {
        SetUI();
    }

    // Update is called once per frame
    public void SetUI()
    {
        worldUI.SetActive(Settings.instance.interface3D);
        trialRender2D.SetActive(Settings.instance.renderTexture2D);
        outerRing.SetActive(Settings.instance.ringOuter);
        outerLines.SetActive(Settings.instance.linesOuter);
        innerLinesA.SetActive(Settings.instance.linesInnerA);
        innerLinesB.SetActive(Settings.instance.linesInnerB);
    }
}
