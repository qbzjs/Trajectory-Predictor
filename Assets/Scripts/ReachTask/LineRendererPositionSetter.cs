using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererPositionSetter : MonoBehaviour{
    
    public bool isEnabled = true;
    private LineRenderer lineRenderer;
    public Transform[] positions;

    void Start()
    {
        lineRenderer = gameObject.transform.GetComponent<LineRenderer>();
        
        // Set the number of points in the line renderer
        lineRenderer.positionCount = positions.Length;

        // Set each point in the line renderer to the corresponding transform position
        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i, positions[i].position);
        }
    }

    private void Update(){
        if (isEnabled){
            for (int i = 0; i < positions.Length; i++){
                lineRenderer.SetPosition(i, positions[i].position);
                // Debug.Log(positions[0].position);
            }
        }
    }
}