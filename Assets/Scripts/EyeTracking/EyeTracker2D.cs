using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobii.Gaming.Examples.GazePointData
{
    public class EyeTracker2D : MonoBehaviour
    {
        public float xCoord;
        public float yCoord;
        
        void Update(){
            //GazePoint.SetActive(false);

            GazePoint gazePoint = TobiiAPI.GetGazePoint();
            if (gazePoint.IsValid)
            {
                Vector2 gazePosition = gazePoint.Screen;
                Vector2 roundedSampleInput = new Vector2(Mathf.RoundToInt(gazePosition.x), Mathf.RoundToInt(gazePosition.y));

                //Debug.Log("x (in px): " + roundedSampleInput.x + " :: " + "y (in px): " + roundedSampleInput.y);

                xCoord = roundedSampleInput.x;
                yCoord = roundedSampleInput.y;
            }
        }
    }
}
