using System;
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
    
    public GameObject controlObject; //object moved by the BCI - arm will take position from this object

    [Header("HANDS")] 
    public GameObject armIK_Male;
    public GameObject armIK_Female;
    public Renderer armIK_Renderer;
    public Material armVisible;
    public Material armInvisible;
    public GameObject leapHandManager;
    
    public HandModelManager leapModelManager;
    public GameObject BCI_leftHand;
    public GameObject BCI_rightHand;

    private BCI_ControlSignal controlSignal;
    private BCI_Controller bciController;
    private TargetedMotionReference targetedMotionReference;

    private bool kinematicHands = false;
    private bool BCIHands = false;

    [Header("UI")]
    public Button kinematicBtn;
    public Button BCI_Btn;

    private Vector3 controlVectorPredicted; //raw input signal
    private Vector3 controlVectorTarget;
    private Vector3 controlVectorAssisted; //descriminated of used predicted
    private Vector3 controlVectorRefined; //final output signal
    
    public delegate void ControlSignal(Vector3 cvPredicted, Vector3 cvTarget, Vector3 cvAssisted, Vector3 cvRefined);
    public static event ControlSignal OnControlSignal;
    
    #region Subscriptions

    private void OnEnable(){
        GameManager.OnGameAction += GameManagerOnGameAction;
        GameManager.OnRunAction += GameManagerOnRunAction;
        GameManager.OnBlockAction += GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnGameAction -= GameManagerOnGameAction;
        GameManager.OnRunAction -= GameManagerOnRunAction;
        GameManager.OnBlockAction -= GameManagerOnBlockAction;
    }
    private void GameManagerOnGameAction(GameStatus eventType){
        if (eventType == GameStatus.Ready){
            
        }
    }
    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal, RunType runType){
        if (eventType == GameStatus.RunComplete){
            EnableKinematic();
            DisableBCI();
            controlSignal.FadeSmoothing(Fade.Out);
            //fade the hand material out
            //enable the leap hand
        }
        if (eventType == GameStatus.AllRunsComplete){
            EnableKinematic();
            DisableBCI();
            controlSignal.FadeSmoothing(Fade.Out);
            //fade the hand material out
            //enable the leap hand
        }
    }
    private void GameManagerOnBlockAction(GameStatus eventType, float lifetime, int blockIndex, int blockTotal){
        if (eventType==GameStatus.BlockComplete){
            EnableKinematic();
            DisableBCI();
            //fade the hand material out
            //enable the leap hand
        }
        if (eventType == GameStatus.Countdown){
            //SWITCH HANDS ON COUNTDOWN
            if (Settings.instance.currentRunType == RunType.Imagined){
                DisableKinematic();
                EnableBCI();
                controlSignal.FadeSmoothing(Fade.In);
                //fade the hand material in
                //disable the leap hand
            }
            if (Settings.instance.currentRunType == RunType.Kinematic){
                EnableKinematic();
                DisableBCI();
                controlSignal.FadeSmoothing(Fade.Out);
                //fade the hand material in
                //disable the leap hand
            }
        }
    }

    #endregion
    

    void Awake(){
        instance = this;

        if (controlObject == null){
            Debug.Log("BCI SIGNAL MISSING CONTROL OBJECT (RIGIDBODY)");
        }

        controlSignal = gameObject.GetComponent<BCI_ControlSignal>();
        bciController = gameObject.GetComponent<BCI_Controller>();
        targetedMotionReference = gameObject.GetComponent<TargetedMotionReference>();
        leapModelManager = leapHandManager.GetComponent<HandModelManager>();
    }
    void Start(){
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        EnableKinematic(); //START WITH LEAP HANDS ACTIVE
        DisableBCI();
    }

    public void SetControlAssistance(int a){
        
    }

    private void Update(){
        if (OnControlSignal != null){
            OnControlSignal(controlSignal.controlVectorPredicted, controlSignal.controlVectorTarget,
                controlSignal.controlVectorAssisted, controlSignal.controlVectorRefined);
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
        leapHandManager.SetActive(false);
        //leapModelManager.enabled = false;
        //find leap hands..?
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    private void EnableKinematic(){
        kinematicHands = true;
        leapHandManager.SetActive(true);
        //leapModelManager.enabled = true;
        kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    }
    private void DisableBCI(){
        BCIHands = false;
        BCI_leftHand.SetActive(false);
        BCI_rightHand.SetActive(false);
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        // if (Settings.instance.handedness == Handedness.Left){
        //     controlObject.transform.parent = gameObject.transform;
        //     controlObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        // }
        // if (Settings.instance.handedness == Handedness.Right){
        //     controlObject.transform.parent = gameObject.transform;
        //     controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
        // }
    }
    private void EnableBCI(){
        BCIHands = true;
        BCI_leftHand.SetActive(true);
        BCI_rightHand.SetActive(true);
        BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
        // if (Settings.instance.handedness == Handedness.Left){
        //     controlObject.transform.parent = BCI_leftHand.transform;
        //     controlObject.transform.position = new Vector3(BCI_leftHand.transform.position.x, BCI_leftHand.transform.position.y, BCI_leftHand.transform.position.z);
        // }
        // if (Settings.instance.handedness == Handedness.Right){
        //     controlObject.transform.parent = BCI_rightHand.transform;
        //     controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
        // }
    }


}
