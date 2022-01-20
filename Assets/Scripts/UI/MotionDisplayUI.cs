using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Enums;

public class MotionDisplayUI : MonoBehaviour
{
    public Handedness hand = Handedness.Left;
    
    public TextMeshProUGUI pDisplay;
    public TextMeshProUGUI rDisplay;
    private Vector3 p;
    private Vector3 r;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hand == Handedness.Left)
        {
            p = DAO.instance.motionDataLeftWrist.position;
            r = DAO.instance.motionDataLeftWrist.rotation;
        }
        else
        {
            p = DAO.instance.motionDataRightWrist.position;
            r = DAO.instance.motionDataRightWrist.rotation;
        }

        pDisplay.text = "x: " + p.x.ToString("F2") + " y: "  + p.y.ToString("F2") + " z: "  + p.z.ToString("F2");
        rDisplay.text = "x: " + r.x.ToString("F2") + " y: "  + r.y.ToString("F2") + " z: "  + r.z.ToString("F2");
        
    }
}
