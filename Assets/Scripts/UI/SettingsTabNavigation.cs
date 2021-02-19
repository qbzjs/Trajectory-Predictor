using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTabNavigation : MonoBehaviour
{

    public GameObject[] tabs = new GameObject[0];
    public GameObject[] panels = new GameObject[0];

    private float[] panelPosition = new float[0];

    private Color defaultColour;
    public Color highlightedColour;

    public float scrollTime = 0.5f;
    public int panelOffset = 0;
    private float offset;
    public int index = 0;

    public float initialScale;
    public GameObject panelParent;

    void Awake(){
        defaultColour = tabs[0].GetComponent<Image>().color;
        tabs[0].GetComponent<Image>().color = highlightedColour;

        offset = panelOffset;

        initialScale = panels[0].transform.localScale.x;

        panelParent = new GameObject();
        panelParent.transform.parent = transform;
        panelParent.transform.position = transform.position;
        panelParent.name = "Settings Panels";

        panelPosition = new float[panels.Length];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].transform.parent = panelParent.transform;
        }
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].transform.parent = panelParent.transform;

            LeanTween.moveLocalX(panels[i], offset * i, 0);
            panelPosition[i] = -offset * i;

            //LeanTween.scale(panels[i], new Vector3(initialScale / i, initialScale / i , 1), 1f).setEase(LeanTweenType.easeInOutSine);

            //offset = offset - (offset / 2);
        }
    }

    public void SelectTab(int t){
        index = t;
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].GetComponent<Image>().color = defaultColour;
        }
        tabs[index].GetComponent<Image>().color = highlightedColour;

        LeanTween.moveLocalX(panelParent, panelPosition[index], scrollTime).setEase(LeanTweenType.easeInOutSine);
    }
}
