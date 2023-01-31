using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EyeDataClassifierFormat
{
    public string tag;
    public int trigger;
    public int phase;
    public string phaseName;
    [Header("Eye Data")] 
    public int lookTarget;
    public string lookTargetName;
    public int blink;
    public bool blinking;

    public EyeDataClassifierFormat(string tag, int trigger, int phase, string phaseName, int lookTarget, string lookTargetName, int blink, bool blinking){
        this.tag = tag;
        this.trigger = trigger;
        //0=null 1=baseline 2=refWindow 3=toTarget 4=toHome
        this.phase = phase;
        this.phaseName = phaseName;
        this.lookTarget = lookTarget;
        this.lookTargetName = lookTargetName;
        this.blink = blink;
        this.blinking = blinking;
    }
}