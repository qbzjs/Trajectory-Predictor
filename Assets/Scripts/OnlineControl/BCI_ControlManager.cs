using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BCI_ControlManager : MonoBehaviour{

    public HandModelManager leapHandManager;
    public GameObject BCI_leftHand;
    public GameObject BCI_rightHand;

    public bool kinematicHands = false;
    public bool BCIHands = false;

    public Button kinematicBtn;
    public Button BCI_Btn;

    void Start()
    {
        kinematicHands = false;
        BCIHands = false;
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    
    void Update()
    {
        
    }

    public void ToggleKinematicHands(){
        if (kinematicHands){
            DisableKinematic();
        }
        else{
            EnableKinematic();
        }
    }

    public void ToggleBCIHands(){
        if (BCIHands){
            DisableBCI();
        }
        else{
            EnableBCI();
        }
    }
    
    private void DisableKinematic(){
        kinematicHands = false;
        leapHandManager.enabled = false;
        //find leap hands..?
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    private void EnableKinematic(){
        kinematicHands = true;
        leapHandManager.enabled = true;
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    }
    private void DisableBCI(){
        BCIHands = false;
        BCI_leftHand.SetActive(false);
        BCI_rightHand.SetActive(false);
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    private void EnableBCI(){
        BCIHands = true;
        BCI_leftHand.SetActive(true);
        BCI_rightHand.SetActive(true);
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    }
}
