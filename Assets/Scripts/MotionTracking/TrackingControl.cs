using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingControl : MonoBehaviour
{
   
    public delegate void SpaceAction(string id);
    public static event SpaceAction OnSpaceBar;
    
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnSpaceBar != null)
            {
                OnSpaceBar(System.Guid.NewGuid().ToString());
            }
        }
    }
}
