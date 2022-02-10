using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonColour : MonoBehaviour
{
    public bool active;
    private Color defaultColour;
    private Color activeColour;

    private Image buttonImg;
    
    void Start(){
        defaultColour = gameObject.GetComponent<Image>().color;
        activeColour = Settings.instance.UI_Orange;
        
        buttonImg = gameObject.GetComponent<Image>();
        
        //set the initial button state based on active status
        if (active){
            buttonImg.color = activeColour;
        }
        else{
            buttonImg.color = defaultColour;
        }
    }

    
    public void SetButton()
    {
        //toggle the button 
        if (active){
            active = false;
            buttonImg.color = defaultColour;
        }
        else{
            active = true;
            buttonImg.color = activeColour;
        }
    }
}
