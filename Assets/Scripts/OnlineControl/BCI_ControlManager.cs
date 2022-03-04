using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Leap.Unity;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using DG.Tweening;

public class BCI_ControlManager : MonoBehaviour
{
    public static BCI_ControlManager instance;

    public GameObject controlObject; //object moved by the BCI - arm will take position from this object

    [Header("HANDS")] public Sex sex = Sex.Female;
    public GameObject armIK_Male;
    public GameObject armIK_Female;
    public GameObject armIK;
    public Renderer armIK_Renderer;
    public Material armMaterial;
    public Material armMaterialInvisible;
    public Material armInvisibleMaterialMale;
    public Material armInvisibleMaterialFemale;

    public GameObject leapHandManager;

    public HandModelManager leapModelManager;
    public GameObject BCI_leftHand;
    public GameObject BCI_rightHand;

    private BCI_ControlSignal controlSignal;
    private BCI_Controller bciController;
    private TargetedMotionReference targetedMotionReference;

    private bool leapHands = false;
    private bool ikHands = false;
    private bool kinematicHands = false;
    private bool BCIHands = false;

    [Header("UI")] public Button leapBtn;
    public Button IK_Btn;

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

    private void GameManagerOnRunAction(GameStatus eventType, float lifeTime, int runIndex, int runTotal,
        RunType runType){
        if (eventType == GameStatus.RunComplete){
            //EnableKinematic();
            //DisableBCI();
            controlSignal.FadeSmoothing(Fade.Out);
            //fade the hand material out
            //enable the leap hand

            //FadeArms(false,1f);

            EnableLeap();
            DisableIK();
        }

        if (eventType == GameStatus.AllRunsComplete){
            // EnableKinematic();
            // DisableBCI();
            controlSignal.FadeSmoothing(Fade.Out);
            //fade the hand material out
            //enable the leap hand

            // FadeArms(false,1f);
            EnableLeap();
            DisableIK();
        }
    }

    private void GameManagerOnBlockAction(GameStatus eventType, float lifetime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.BlockComplete){
            // EnableKinematic();
            // DisableBCI();
            //fade the hand material out
            //enable the leap hand

            //FadeArms(false,1f);

            EnableLeap();
            DisableIK();
        }

        if (eventType == GameStatus.Countdown){
            //SWITCH HANDS ON COUNTDOWN
            if (Settings.instance.currentRunType == RunType.Imagined){
                // DisableKinematic();
                // EnableBCI();
                controlSignal.FadeSmoothing(Fade.In);
                //fade the hand material in
                //disable the leap hand

                // FadeArms(true,1f);

                DisableLeap();
                EnableIK();
            }

            if (Settings.instance.currentRunType == RunType.Kinematic){
                // EnableKinematic();
                // DisableBCI();
                controlSignal.FadeSmoothing(Fade.Out);
                //fade the hand material in
                //disable the leap hand

                // FadeArms(true, 1f);
                
                DisableLeap();
                EnableIK();
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

        if (sex == Sex.Male){
            armIK_Female.SetActive(false);
            armIK_Male.SetActive(true);
            armIK = armIK_Male;
            armIK_Renderer = armIK.transform.GetChild(0).GetComponent<Renderer>();
            armMaterial = armIK_Renderer.material;
            armMaterialInvisible = armInvisibleMaterialMale;
        }
        else{
            armIK_Female.SetActive(true);
            armIK_Male.SetActive(false);
            armIK = armIK_Female;
            armIK_Renderer = armIK.transform.GetChild(0).GetComponent<Renderer>();
            armMaterial = armIK_Renderer.material;
            armMaterialInvisible = armInvisibleMaterialFemale;
        }

        armIK_Renderer.material = armMaterial;
    }

    void Start(){
        leapBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        IK_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        // kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        // BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
        // EnableKinematic(); //START WITH LEAP HANDS ACTIVE
        // DisableBCI();

        EnableLeap();
        DisableIK();
    }

    //TODO - MAKE THE DECREASE ASSISTANCE PER RUN....
    public void SetControlAssistance(int a){
        
    }
    public void DecreaseControlAssistance(int a){
        
    }


    private void Update(){
        if (OnControlSignal != null){
            OnControlSignal(controlSignal.controlVectorPredicted, controlSignal.controlVectorTarget,
                controlSignal.controlVectorAssisted, controlSignal.controlVectorRefined);
        }

        if (Input.GetKeyDown(KeyCode.N)){
            FadeArms(true, 1f);
        }

        if (Input.GetKeyDown(KeyCode.M)){
            FadeArms(false, 1f);
        }
    }

    public void FadeArms(bool b, float t){
        if (b){
            armIK_Renderer.material = armMaterialInvisible;
            armIK_Renderer.material.DOFade(0, 0);
            armIK_Renderer.material.DOFade(1, t);
            StartCoroutine(PostFadeMaterial(armMaterial, 1f));
        }
        else{
            armIK_Renderer.material = armMaterialInvisible;
            armIK_Renderer.material.DOFade(1, 0);
            armIK_Renderer.material.DOFade(0, t);
        }
    }

    private IEnumerator PostFadeMaterial(Material m, float t){
        yield return new WaitForSeconds(t);
        armIK_Renderer.material = m;
    }

    // public void ToggleKinematicHands(){
    //     if (kinematicHands){
    //         DisableKinematic();
    //     }
    //     else{
    //         EnableKinematic();
    //     }
    // }
    //
    // public void ToggleBCIHands(){
    //     if (BCIHands){
    //         DisableBCI();
    //     }
    //     else{
    //         EnableBCI();
    //     }
    // }
    public void ToggleLeapHands(){
        if (leapHands){
            DisableLeap();
        }
        else{
            EnableLeap();
        }
    }

    public void ToggleIK_Hands(){
        if (ikHands){
            DisableIK();
        }
        else{
            EnableIK();
        }
    }

    private void EnableLeap(){
        leapHands = true;
        leapHandManager.SetActive(true);
        leapBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    }

    private void DisableLeap(){
        leapHands = false;
        leapHandManager.SetActive(false);
        leapBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }

    private void EnableIK(){
        ikHands = true;
        FadeArms(true, 1f);
        IK_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    }

    private void DisableIK(){
        ikHands = false;
        FadeArms(false, 1f);
        IK_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    }
    // private void DisableKinematic(){
    //     kinematicHands = false;
    //     leapHandManager.SetActive(false);
    //     //leapModelManager.enabled = false;
    //     //find leap hands..?
    //     kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    // }
    // private void EnableKinematic(){
    //     kinematicHands = true;
    //     leapHandManager.SetActive(true);
    //     //leapModelManager.enabled = true;
    //     kinematicBtn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    // }
    //
    //
    // private void DisableBCI(){
    //     BCIHands = false;
    //     BCI_leftHand.SetActive(false);
    //     BCI_rightHand.SetActive(false);
    //     BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Disabled;
    //     // if (Settings.instance.handedness == Handedness.Left){
    //     //     controlObject.transform.parent = gameObject.transform;
    //     //     controlObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    //     // }
    //     // if (Settings.instance.handedness == Handedness.Right){
    //     //     controlObject.transform.parent = gameObject.transform;
    //     //     controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
    //     // }
    // }
    // private void EnableBCI(){
    //     BCIHands = true;
    //     BCI_leftHand.SetActive(true);
    //     BCI_rightHand.SetActive(true);
    //     BCI_Btn.transform.GetComponent<Image>().color = Settings.instance.UI_Orange;
    //     // if (Settings.instance.handedness == Handedness.Left){
    //     //     controlObject.transform.parent = BCI_leftHand.transform;
    //     //     controlObject.transform.position = new Vector3(BCI_leftHand.transform.position.x, BCI_leftHand.transform.position.y, BCI_leftHand.transform.position.z);
    //     // }
    //     // if (Settings.instance.handedness == Handedness.Right){
    //     //     controlObject.transform.parent = BCI_rightHand.transform;
    //     //     controlObject.transform.position = new Vector3(BCI_rightHand.transform.position.x, BCI_rightHand.transform.position.y, BCI_rightHand.transform.position.z);
    //     // }
    // }
}