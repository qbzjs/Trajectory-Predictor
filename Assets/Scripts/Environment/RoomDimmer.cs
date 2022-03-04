using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enums;

public class RoomDimmer : MonoBehaviour
{
    private void OnEnable(){
        GameManager.OnBlockAction+= GameManagerOnBlockAction;
    }
    private void OnDisable(){
        GameManager.OnBlockAction-= GameManagerOnBlockAction;
    }

    private void GameManagerOnBlockAction(GameStatus eventType, float lifeTime, int blockIndex, int blockTotal){
        if (eventType == GameStatus.Countdown){
            SetWalls(0.1F);
        }
        if (eventType == GameStatus.BlockComplete){
            SetWalls(1);
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void SetWalls(float fadeValue){
        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in children){
            r.material.DOFade(fadeValue, 1);
        }
    }
}
