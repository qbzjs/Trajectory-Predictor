using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityRB : MonoBehaviour{
	
	public Rigidbody rb;
	
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
	    //rb.velocity = new Vector3(1, 1, 1);
	    
	    Debug.Log(Input.GetAxisRaw("Horizontal"));
    }
}
