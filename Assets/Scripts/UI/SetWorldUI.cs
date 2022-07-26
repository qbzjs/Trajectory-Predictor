using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using DG.Tweening;

public class SetWorldUI : MonoBehaviour{
    
    public static SetWorldUI instance;

    public GameObject worldUI;
    public GameObject trialRender2D;

    public GameObject outerRing;
    public GameObject outerLines;
    public GameObject innerLinesA;
    public GameObject innerLinesB;

    public Transform progressMenu;

    private void OnEnable(){
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.H)){
            GameManagerOnBlockAction(GameStatus.VisibleCountdown, 0.5F, 0, 0);
        }
    }

    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (Settings.instance.interface3D == true){
            if (eventType == GameStatus.VisibleCountdown){
                progressMenu.DOScaleY(0, 0.5f);
                progressMenu.GetComponent<AudioSource>().Play();
                // worldUI.SetActive(false);
            }
            if (eventType == GameStatus.Ready){
                progressMenu.DOScaleY(1, 0.75f);
                progressMenu.GetComponent<AudioSource>().Play();
                // worldUI.SetActive(true);
            }
            if (eventType == GameStatus.BlockComplete){
                progressMenu.DOScaleY(1, 0.75f);
                progressMenu.GetComponent<AudioSource>().Play();
                // worldUI.SetActive(true);
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
        trialRender2D.SetActive(Settings.instance.display2D);
//        outerRing.SetActive(Settings.instance.ringOuter);
//        outerLines.SetActive(Settings.instance.linesOuter);
//        innerLinesA.SetActive(Settings.instance.linesInnerA);
//        innerLinesB.SetActive(Settings.instance.linesInnerB);
    }
}
