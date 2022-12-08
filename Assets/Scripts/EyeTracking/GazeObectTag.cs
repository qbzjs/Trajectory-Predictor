using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeObectTag : MonoBehaviour
{
    public string tag = "";

    public bool detachFromParent = true; // detach the object from the parent when tagged
    
    private void OnEnable(){
        if (transform.parent.GetComponent<Tag>()){
            tag = transform.parent.GetComponent<Tag>().tag; 
        }

        StartCoroutine(GetTag());
    }

    void Start()
    {
        
    }

    private IEnumerator GetTag(){
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (transform.parent.GetComponent<Tag>()){
            tag = transform.parent.GetComponent<Tag>().tag; 
        }

        yield return new WaitForSeconds(2f);
        if (tag != ""){
            transform.name = transform.name + tag.ToString();
            transform.parent = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
//        Debug.DrawRay(Camera.main.transform.position, Vector3.forward, Color.green);  
    }
}
