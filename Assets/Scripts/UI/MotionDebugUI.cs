using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MotionDebugUI : MonoBehaviour
{
    
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
        p = DAO.instance.motionDataRightWrist.position;
        r = DAO.instance.motionDataRightWrist.rotation;
        
        pDisplay.text = "x: " + p.x.ToString("F2") + " y: "  + p.y.ToString("F2") + " z: "  + p.z.ToString("F2");
        rDisplay.text = "x: " + r.x.ToString("F2") + " y: "  + r.y.ToString("F2") + " z: "  + r.z.ToString("F2");
        
    }
}
