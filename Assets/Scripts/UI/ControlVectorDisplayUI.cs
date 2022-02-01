using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlVectorDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI pDisplay;
    public TextMeshProUGUI tDisplay;
    public TextMeshProUGUI aDisplay;
    public TextMeshProUGUI cDisplay;
    private Vector3 predicted;
    private Vector3 target;
    private Vector3 control;
    private int assistance;
    
    void Update(){

        predicted = BCI_ControlSignal.instance.controlVectorPredicted;
        target = BCI_ControlSignal.instance.controlVectorTarget;
        control = BCI_ControlSignal.instance.controlVectorRefined;
        assistance = BCI_ControlSignal.instance.controlAssistPercentage;

        pDisplay.text = "Predicted V: " + predicted.x.ToString("F2") + " y: "  + predicted.y.ToString("F2") + " z: "  + predicted.z.ToString("F2");
        tDisplay.text = "Targeted V: " + target.x.ToString("F2") + " y: "  + target.y.ToString("F2") + " z: "  + target.z.ToString("F2");
        aDisplay.text = "Control Assist : " + assistance.ToString() + "%";
        cDisplay.text = "Control V: " + control.x.ToString("F2") + " y: "  + control.y.ToString("F2") + " z: "  + control.z.ToString("F2");
    }
}
