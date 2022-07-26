using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.FinalIK;
using Enums;

public class SetDisplayType : MonoBehaviour
{
    public static SetDisplayType instance;
    
    public DisplayType displayType;
    public GameObject cameraVR;
    public GameObject camera2D;
    public Transform neck;
    public VRIK IK;

    private void Awake(){
        instance = this;
    }

    void Start()
    {
        
        SetDisplay(DisplayType.ScreenVR);
    }

    private void Initialise(){
        
    }
    private void Update(){
        if (Input.GetKeyDown(KeyCode.T)){
            if (displayType == DisplayType.Screen2D){
                SetDisplay(DisplayType.ScreenVR);
            }
            else{
                SetDisplay(DisplayType.Screen2D);
            }
        }
    }

    public void SetDisplay(DisplayType displayType)
    {
        if(displayType == DisplayType.ScreenVR){
            cameraVR.SetActive(true);
            camera2D.SetActive(false);
            neck = cameraVR.transform.Find("Neck");
        }
        if(displayType == DisplayType.Screen2D){
            cameraVR.SetActive(false);
            camera2D.SetActive(true);
            neck = camera2D.transform.Find("Neck");
        }
        IK.solver.spine.headTarget = neck;

        this.displayType = displayType;
    }
}
