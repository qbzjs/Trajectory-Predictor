using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallTrajectory : MonoBehaviour
{
    
    public GameObject ball;
    public Vector3 forceVector;
    public Vector3 originPoint;
    
    void Start(){
        originPoint = ball.transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F)){
            Debug.Log("call traj....");
            TrajectoryVisualiser.instance.UpdateTrajectory(forceVector, ball.GetComponent<Rigidbody>(), originPoint);
        }
    }
}
