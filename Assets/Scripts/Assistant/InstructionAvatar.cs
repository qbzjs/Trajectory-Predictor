using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Enums;
using RootMotion.FinalIK;
using Unity.Mathematics;

public class InstructionAvatar : MonoBehaviour
{

    public GameObject avatar;
    public float duration = 1f;
    private Vector3 avatarSize;

    public GameObject popParticle;

    public Vector3 rot = new Vector3(0,180,0);

    public TextMeshProUGUI speechBubble;

    public BipedIK bipedIK;

    private bool enabledFlag;

    private void Awake(){
        avatarSize = avatar.transform.localScale;
        avatar.transform.localScale = Vector3.zero;
        avatar.transform.localScale = new Vector3(0, 0, 0);
    }

    void Start()
    {
        
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.A)){
            EnableAvatar();
        }
        if (Input.GetKeyDown(KeyCode.D)){
            DisableAvatar();
        }
    }
    public void EnableAvatar(){
        avatar.transform.DOScale(avatarSize, duration);
        //avatar.transform.DOPunchRotation(rot, duration*2);
        GameObject par = Instantiate(popParticle, avatar.transform.position, quaternion.identity);

        bipedIK.enabled = true;
        enabledFlag = true;
    }

    public void DisableAvatar(){
        if (enabledFlag){
            avatar.transform.DOScale(Vector3.zero, duration);
            //avatar.transform.DOPunchRotation(rot, duration*2);
            GameObject par = Instantiate(popParticle, avatar.transform.position, quaternion.identity);
        
            speechBubble.text = "...";
        
            bipedIK.enabled = false;
            enabledFlag = false;
        }
    }
    public void DisableAvatar(bool particle){
        if (enabledFlag){
            avatar.transform.DOScale(Vector3.zero, duration);
            //avatar.transform.DOPunchRotation(rot, duration*2);
            if (particle){
                GameObject par = Instantiate(popParticle, avatar.transform.position, quaternion.identity);
            }
        
            speechBubble.text = "...";
        
            bipedIK.enabled = false;
            enabledFlag = false;
        }
    }

    public void SetText(string txt, Color col){
        speechBubble.text = txt;
        speechBubble.color = col;
    }
}
