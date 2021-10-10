using System.Collections;
using UnityEngine;
using TMPro;
using Enums;

public class SetParadigmSettings : MonoBehaviour
{
    public TextMeshProUGUI paradigmText;
    
    public TrialParadigm paradigm = TrialParadigm.Horizontal;
    
    private int index;

    private TrialParadigm[] paradigms = new TrialParadigm[5];
    private Color defaultColour;
    
    void Start()
    {
        defaultColour = paradigmText.color;
        paradigms[0] = TrialParadigm.Horizontal;
        paradigms[1] = TrialParadigm.Vertical;
        paradigms[2] = TrialParadigm.Circle;
        paradigms[3] = TrialParadigm.CentreOut;
        paradigms[4] = TrialParadigm.RandomPosition;
        Initialise();
    }
    
    void Initialise(){
        paradigm = Settings.instance.trialParadigm;
        SetParadigmDisplay(paradigm);
    }
    private void SetParadigmDisplay(TrialParadigm par)
    {
        string p = par.ToString();
        paradigmText.text = p;
    }
    public void NextParadigm(){
        //move index
        switch (paradigm){
            case TrialParadigm.Horizontal: {
                paradigm = TrialParadigm.Vertical;
                break;
            }
            case TrialParadigm.Vertical: {
                paradigm = TrialParadigm.Circle;
                break;
            }
            case TrialParadigm.Circle: {
                paradigm = TrialParadigm.CentreOut;
                break;
            }
            case TrialParadigm.CentreOut: {
                paradigm = TrialParadigm.RandomPosition;
                break;
            }
            case TrialParadigm.RandomPosition: {
                paradigm = TrialParadigm.Horizontal;
                break;
            }
        }
        //format display
        SetParadigmDisplay(paradigm);
        //colour effect on
        paradigmText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetParadigm(paradigm);
        //colour effect off
        StartCoroutine(DefaultColour());
    }
    public void PreviousParadigm(){
        //move index
        switch (paradigm){
            case TrialParadigm.RandomPosition: {
                paradigm = TrialParadigm.CentreOut;
                break;
            }
            case TrialParadigm.CentreOut: {
                paradigm = TrialParadigm.Circle;
                break;
            }
            case TrialParadigm.Circle: {
                paradigm = TrialParadigm.Vertical;
                break;
            }
            case TrialParadigm.Vertical: {
                paradigm = TrialParadigm.Horizontal;
                break;
            }
            case TrialParadigm.Horizontal: {
                paradigm = TrialParadigm.RandomPosition;
                break;
            }
        }
        //format display
        SetParadigmDisplay(paradigm);
        //colour effect on
        paradigmText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetParadigm(paradigm);
        //colour effect off
        StartCoroutine(DefaultColour());
    }
    private void SetParadigm(TrialParadigm par) {
        Settings.instance.SetTrialParadigm(par);
    }
    private IEnumerator DefaultColour()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        paradigmText.color = defaultColour;
    }
}
