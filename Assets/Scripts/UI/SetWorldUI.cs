using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldUI : MonoBehaviour{
    
    public static SetWorldUI instance;

    public GameObject worldUI;
    public GameObject trialRender2D;

    public GameObject outerRing;
    public GameObject outerLines;
    public GameObject innerLinesA;
    public GameObject innerLinesB;
    
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
