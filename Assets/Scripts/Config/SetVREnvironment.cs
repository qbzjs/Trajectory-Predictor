using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVREnvironment : MonoBehaviour{
    
    public static SetVREnvironment instance;
    
    public GameObject environment;
    
    void Awake(){
        instance = this;
    }
    void Start()
    {
        SetEnvironment();
    }

    // Update is called once per frame
    public void SetEnvironment()
    {
        if (Settings.instance)
        {
            environment.SetActive(Settings.instance.environment3D);
        }
        
    }
}
