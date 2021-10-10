using System.Collections;
using UnityEngine;
using TMPro;
using Enums;

public class SetTargetCountSettings : MonoBehaviour
{
    public TextMeshProUGUI targetCountText;

    public ParadigmTargetCount targetCount = ParadigmTargetCount.Two;

    private int index;

    private ParadigmTargetCount[] targetCounts = new ParadigmTargetCount[6];
    private Color defaultColour;
    
    void Start()
    {
        defaultColour = targetCountText.color;
        targetCounts[0] = ParadigmTargetCount.One;
        targetCounts[1] = ParadigmTargetCount.Two;
        targetCounts[2] = ParadigmTargetCount.Three;
        targetCounts[3] = ParadigmTargetCount.Four;
        targetCounts[4] = ParadigmTargetCount.Eight;
        targetCounts[5] = ParadigmTargetCount.Sixteen;
        Initialise();
    }
    
    void Initialise(){
        targetCount = Settings.instance.paradigmTargetCount;
        SetTargetCountDisplay(targetCount);
    }
    private void SetTargetCountDisplay(ParadigmTargetCount tars)
    {
        string t = tars.ToString();
        t = t + " Targets";
         if (tars == ParadigmTargetCount.One){
             t = t.Replace(" Targets"," Target");
         }
        
        targetCountText.text = t;
    }
    public void NextTargetCount(){
        //move index
        switch (targetCount){
            case ParadigmTargetCount.One: {
                targetCount = ParadigmTargetCount.Two;
                break;
            }
            case ParadigmTargetCount.Two: {
                targetCount = ParadigmTargetCount.Three;
                break;
            }
            case ParadigmTargetCount.Three: {
                targetCount = ParadigmTargetCount.Four;
                break;
            }
            case ParadigmTargetCount.Four: {
                targetCount = ParadigmTargetCount.Eight;
                break;
            }
            case ParadigmTargetCount.Eight: {
                targetCount = ParadigmTargetCount.Sixteen;
                break;
            }
            case ParadigmTargetCount.Sixteen: {
                targetCount = ParadigmTargetCount.One;
                break;
            }
        }
        //format display
        SetTargetCountDisplay(targetCount);
        //colour effect on
        targetCountText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetTargetCount(targetCount);
        //colour effect off
        StartCoroutine(DefaultColour());
    }
    public void PreviousTargetCount(){
        //move index
        switch (targetCount){
            case ParadigmTargetCount.Sixteen: {
                targetCount = ParadigmTargetCount.Eight;
                break;
            }
            case ParadigmTargetCount.Eight: {
                targetCount = ParadigmTargetCount.Four;
                break;
            }
            case ParadigmTargetCount.Four: {
                targetCount = ParadigmTargetCount.Three;
                break;
            }
            case ParadigmTargetCount.Three: {
                targetCount = ParadigmTargetCount.Two;
                break;
            }
            case ParadigmTargetCount.Two: {
                targetCount = ParadigmTargetCount.One;
                break;
            }
            case ParadigmTargetCount.One: {
                targetCount = ParadigmTargetCount.Sixteen;
                break;
            }
        }
        //format display
        SetTargetCountDisplay(targetCount);
        //colour effect on
        targetCountText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetTargetCount(targetCount);
        //colour effect off
        StartCoroutine(DefaultColour());
    }
    private void SetTargetCount(ParadigmTargetCount tCount) {
        Settings.instance.SetParadigmTargetCount(tCount);
    }
    private IEnumerator DefaultColour()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        targetCountText.color = defaultColour;
    }
}
