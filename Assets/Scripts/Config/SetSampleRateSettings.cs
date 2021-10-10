using System.Collections;
using UnityEngine;
using TMPro;
using Enums;

public class SetSampleRateSettings : MonoBehaviour
{
    public TextMeshProUGUI sampleRateText;

    public SampleRate sampleRate = SampleRate.Hz60;

    private int index;
    
    private SampleRate[] sampleRates = new SampleRate[4];
    private Color defaultColour;
    
    
    void Start()
    {
        defaultColour = sampleRateText.color;
        sampleRates[0] = SampleRate.Hz50;
        sampleRates[1] = SampleRate.Hz60;
        sampleRates[2] = SampleRate.Hz75;
        sampleRates[3] = SampleRate.Hz100;
        Initialise();
    }

    private void Initialise()
    {
        sampleRate = Settings.instance.sampleRate;
        SetSampleRateDisplay(sampleRate);
    }
    private void SetSampleRateDisplay(SampleRate sr)
    {
        string s = sr.ToString();
        s = s.Replace("Hz","");
        s = s + "<size=-10>Hz";
        sampleRateText.text = s;
    }

    public void NextSampleRate(){
        //move index
        switch (sampleRate){
            case SampleRate.Hz50: {
                sampleRate = SampleRate.Hz60;
                break;
            }
            case SampleRate.Hz60: {
                sampleRate = SampleRate.Hz75;
                break;
            }
            case SampleRate.Hz75: {
                sampleRate = SampleRate.Hz100;
                break;
            }
            case SampleRate.Hz100: {
                sampleRate = SampleRate.Hz50;
                break;
            }
        }
        //format display
        SetSampleRateDisplay(sampleRate);
        //colour effect on
        sampleRateText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetSampleRate(sampleRate);
        //colour effect off
        StartCoroutine(DefaultColour());
    }

    public void PreviousSampleRate() {
        //move index
        switch (sampleRate) {
            case SampleRate.Hz100 : {
                sampleRate = SampleRate.Hz75;
                break;
            }
            case SampleRate.Hz75 : {
                sampleRate = SampleRate.Hz60;
                break;
            }
            case SampleRate.Hz60 : {
                sampleRate = SampleRate.Hz50;
                break;
            }
            case SampleRate.Hz50 : {
                sampleRate = SampleRate.Hz100;
                break;
            }
        }
        //format display
        SetSampleRateDisplay(sampleRate);
        //colour effect on
        sampleRateText.color = Settings.instance.highlightColour;
        //set sample rate in settings
        SetSampleRate(sampleRate);
        //colour effect off
        StartCoroutine(DefaultColour());
    }
    private void SetSampleRate(SampleRate sr) {
        Settings.instance.SetSampleRate(sr);
    }
    private IEnumerator DefaultColour()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        sampleRateText.color = defaultColour;
    }
}
