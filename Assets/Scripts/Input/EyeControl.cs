using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EyeControl : MonoBehaviour
{
    //public static EyeControl instance; 

    public bool eyeControlEnabled = false;
    
    public EyeDataFormat eyeData;

    public float blinkTimer;
    public float cooldownTimer;

    public bool longBlinkReady;
    private bool eventSent;
    
    public delegate void UserEyeAction(UserInputType inputType);
    public static event UserEyeAction OnUserEyeAction;
    
    void Awake(){
        //instance = this;
    }
    void Update(){
        eyeData = DAO.instance.eyeData;

        //simulated input
        if (Input.GetKeyDown(KeyCode.I)){
            if (OnUserEyeAction != null){
                OnUserEyeAction(UserInputType.LongBlink);
            }
        }
    }
    void LateUpdate(){
        if (eyeControlEnabled){
            if (eyeData.blinking){
                blinkTimer += Time.deltaTime;
                if (blinkTimer > 1){
                    longBlinkReady = true;
                }
            }
            else{
                blinkTimer = 0;
            }

            if (longBlinkReady && !eventSent){
                longBlinkReady = false;
                eventSent = true;
                if (OnUserEyeAction != null){
                    OnUserEyeAction(UserInputType.LongBlink);
                }
            }
            if (eventSent){
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer > 1){
                    cooldownTimer = 0;
                    eventSent = false;
                }
            }
        }

    }
}
