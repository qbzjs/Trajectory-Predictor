using System.Collections;
using System.Collections.Generic;
using Enums;
using Leap.Unity;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BCI_ControlManager : MonoBehaviour
{

    public static BCI_ControlManager instance;
    
    public Rigidbody controlObject; //object moved by the BCI - arm will take position from this object
    
    [Header("HANDS")]
    public HandModelManager leapHandManager;
    public GameObject BCI_leftHand;
    public GameObject BCI_rightHand;

    [SerializeField] private BCI_ControlSignal controlSignal;
    [SerializeField] private ArmReachController armReachController;

    private bool kinematicHands = false;
    private bool BCIHands = false;

    [Header("UI")]
    public Button kinematicBtn;
    public Button BCI_Btn;

    void Awake(){
        instance = this;
        
        if (gameObject.GetComponent<Rigidbody>()){
            controlObject = gameObject.GetComponent<Rigidbody>();
        }
        else{
            controlObject = transform.Find("BCI Control Object").GetComponent<Rigidbody>();
        }

        if (controlObject == null){
            Debug.Log("BCI SIGNAL MISSING CONTROL OBJECT (RIGIDBODY)");
        }

        controlSignal = gameObject.GetComponent<BCI_ControlSignal>();
        armReachController = gameObject.GetComponent<ArmReachController>();
    }
    void Start(){
        DisableKinematic();
        DisableBCI();
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            ResetControlObject();
        }

        if (Input.GetKeyDown(KeyCode.B)){
            controlSignal.brakes = true;
        }
        else{
            controlSignal.brakes = false;
        }
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
        if (Settings.instance.handedness == Handedness.Left){
            controlObject.transform.parent = gameObject.transform;
            controlObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (Settings.instance.handedness == Handedness.Right){
            controlObject.transform.parent = gameObject.transform;
            controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
        }
    }
    private void EnableBCI(){
        BCIHands = true;
        BCI_leftHand.SetActive(true);
        BCI_rightHand.SetActive(true);
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
        if (Settings.instance.handedness == Handedness.Left){
            controlObject.transform.parent = BCI_leftHand.transform;
            controlObject.transform.position = new Vector3(BCI_leftHand.transform.position.x, BCI_leftHand.transform.position.y, BCI_leftHand.transform.position.z);
        }
        if (Settings.instance.handedness == Handedness.Right){
            controlObject.transform.parent = BCI_rightHand.transform;
            controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
        }
    }

    private void ResetControlObject(){
        Vector3 homePos = armReachController.homePosition;
        controlObject.transform.position = new Vector3(homePos.x, homePos.y, homePos.z);
    }
    
    
}
