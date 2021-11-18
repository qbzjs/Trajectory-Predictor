using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualiser : MonoBehaviour
{
    public static TrajectoryVisualiser instance;
    
    public LineRenderer lineRenderer;
    [Range(3,64)]
    public int lineSegmentCount = 16;

    private List<Vector3> linePoints = new List<Vector3>();

    #region Singleton

    void Awake(){
        instance = this;
    }

    #endregion


    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint){
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;

        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / lineSegmentCount;

        linePoints.Clear();

        for (int i = 0; i < lineSegmentCount; i++){
            float stepTimePassed = stepTime * i; //change in time
        }
    } 
}
