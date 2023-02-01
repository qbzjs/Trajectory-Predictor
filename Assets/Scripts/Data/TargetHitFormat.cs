using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetHitFormat{

    public int totalTargetsPresented;
    
    public int kinTargetsPresented;
    public int totalKin;
    public int t1_kin;
    public int t2_kin;
    public int t3_kin;
    public int t4_kin;

    public int imgTargetsPresented;
    public int totalImg;
    public int t1_img;
    public int t2_img;
    public int t3_img;
    public int t4_img;
    
    public TargetHitFormat(int totalTargetsPresented, int kinTargetsPresented, int imgTargetsPresented,
        int totalKin, int t1_kin,int t2_kin,int t3_kin,int t4_kin,
        int totalImg, int t1_img,int t2_img,int t3_img,int t4_img)
    {
        this.totalTargetsPresented = totalTargetsPresented;
        this.kinTargetsPresented = kinTargetsPresented;
        this.imgTargetsPresented = imgTargetsPresented;
        
        this.totalKin = totalKin;
        this.t1_kin = t1_kin;
        this.t2_kin = t2_kin;
        this.t3_kin = t3_kin;
        this.t4_kin = t4_kin;

        this.totalImg = totalImg;
        this.t1_img = t1_img;
        this.t2_img = t2_img;
        this.t3_img = t3_img;
        this.t4_img = t4_img;
    }
}
