using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;
using PathologicalGames;

//initialise set target
    
//instantiate and get destination
    
//track feedback maginitude
    
//set line from origin to destination
    
//random target - based on sectors 1-5
    
//sequence
//---preparation - 2s
//---cue (arrow to target sequence) - 1.5s
//---feedback - 2s

public class TargetController : MonoBehaviour
{
    //temp variables - move to settings or manager
    private bool randomisePosition = false;
    
    public Transform originPoint;
    [Range(0.05f,0.25f)]
    public float targetDistance = 0.125f;

    public Vector3 offsetFromOrigin;
    public Vector3 targetDestination;
    public Transform destinationTransform;
    
    public int targetNumber;
    
    public GameObject reachTargetPrefab;
    private GameObject reachTarget;
    public GameObject fixationCrossPrefab;
    private GameObject fixationCross;
    public GameObject arrowPrefab;
    private GameObject arrow;
    
    void Awake()
    {
        originPoint = this.transform;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("target centre");
            SetTarget(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("target left");
            SetTarget(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("target top");
            SetTarget(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("target right");
            SetTarget(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("target bottom");
            SetTarget(4);
        }

        // if (reachTarget!= null)
        // {
        //     arrow.transform.LookAt(reachTarget.transform);
        // }
        //
    }

    public void SetTarget(int tNum)
    {
        TargetController controller;
        controller = gameObject.GetComponent<TargetController>();
        controller.InitialiseTarget(tNum);
    }

    
    public void InitialiseTarget(int tNum)
    {
        if (reachTarget != null)
        {
            Destroy(reachTarget);
            Destroy(arrow);
            Destroy(fixationCross);
        }
        
        targetNumber = tNum;
        targetDestination = GetDestination(targetNumber);
        destinationTransform.position = targetDestination;
        
        fixationCross = Instantiate(fixationCrossPrefab, originPoint.position, quaternion.identity);
        fixationCross.transform.position = originPoint.position;
        
        arrow = Instantiate(arrowPrefab, originPoint.position, quaternion.identity);
        arrow.transform.position = originPoint.position;
        
        SmoothLookAtConstraint look = arrow.GetComponent<SmoothLookAtConstraint>();
        look.target = destinationTransform.transform;
        
        reachTarget = Instantiate(reachTargetPrefab, originPoint.position, quaternion.identity);
        reachTarget.transform.position = targetDestination;
        
        fixationCross.SetActive(false);
        arrow.SetActive(false);
        reachTarget.SetActive(false);
    }

    private IEnumerator TargetSequence()
    {
        fixationCross.SetActive(true);
        yield return new WaitForSeconds(2);
        fixationCross.SetActive(false);
        
        arrow.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        arrow.SetActive(false);
        
        reachTarget.SetActive(true);
        yield return new WaitForSeconds(4);
        
    }
    private Vector3 GetDestination(int tNum)
    {
        Vector3 dest = Vector3.zero;
        switch (tNum) {
            case 0:
                targetDestination = new Vector3(originPoint.position.x, originPoint.position.y, originPoint.position.z);
                break;
            case 1:
                targetDestination = new Vector3(originPoint.position.x - targetDistance, originPoint.position.y, originPoint.position.z);
                break;
            case 2:
                targetDestination = new Vector3(originPoint.position.x, originPoint.position.y + targetDistance, originPoint.position.z);
                break;
            case 3:
                targetDestination = new Vector3(originPoint.position.x + targetDistance, originPoint.position.y, originPoint.position.z);
                break;
            case 4:
                targetDestination = new Vector3(originPoint.position.x, originPoint.position.y-targetDistance, originPoint.position.z);
                break;
        }

        if (randomisePosition)
        {
          //set a random position based on an offset  
        }

        dest = targetDestination;
        
        return dest;
    }
}
