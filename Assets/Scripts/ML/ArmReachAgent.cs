using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ArmReachAgent : Agent{

    [Range(1,10)]
    public int moveMagnitude = 1;
    [Range(1,10)]
    public float baseReward = 1;
    [Range(1,50)]
    public float bonusMultiplier = 25;

    [SerializeField] private Transform homePosition;
    
    private ArmReachAgentObservations armReachObservations;

    private Rigidbody agentRigidBody;
    
    void Start(){
        armReachObservations = this.GetComponent<ArmReachAgentObservations>();
        agentRigidBody = this.GetComponent<Rigidbody>();
    }

    //for external events from monobehaviour classes
    public void BeginEpisodeExternal(){
        
    }
    public void EndEpisodeExternal(){
        EndEpisode();
    }
    public override void OnEpisodeBegin(){
        transform.position = homePosition.position;
        DAO dao = DAO.instance;
        if (DAO.instance){
            dao = DAO.instance;
        }
        //transform.position = new Vector3(dao.motionDataRightWrist.position.x, dao.motionDataRightWrist.position.y, dao.motionDataRightWrist.position.z);
    }

    //OBSERVATIONS
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(armReachObservations.TargetPresent() ? 1 : 0);
        sensor.AddObservation(armReachObservations.RestPresent() ? 1 : 0);  
        
        sensor.AddObservation(armReachObservations.targetPosition);
        sensor.AddObservation(transform.position);
        
        // sensor.AddObservation(armReachObservations.position);
        // sensor.AddObservation(armReachObservations.velocity);
        // sensor.AddObservation(armReachObservations.rotation);
        // sensor.AddObservation(armReachObservations.angularVelocity);
    }
    //ACTIONS
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];

        //for motion driven 1:1 movement
        //this.transform.position = new Vector3(moveX, moveY, moveZ);
        
        //for independent movement
        //this.transform.position += new Vector3(moveX,moveY,moveZ) * Time.deltaTime * moveMagnitude;
        
        //for velocity driven rigidbody movement
        agentRigidBody.velocity = new Vector3(moveX, moveY, moveZ);
        //agentRigidBody.velocity = new Vector3(moveX, moveY, moveZ) * moveMagnitude;
        
        //update for angular velocity
        //agentRigidBody.angularVelocity = new Vector3()
    }
    //USER INPUT / HEURISTICS
    public override void Heuristic(in ActionBuffers actionsOut){
        DAO dao = DAO.instance;
        if (DAO.instance){
            dao = DAO.instance;
        }
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        
        // continuousActions[0] = Input.GetAxisRaw("Horizontal");
        // continuousActions[1] = Input.GetAxisRaw("Vertical");
        // continuousActions[2] = Input.GetAxisRaw("Vertical");
        
        // continuousActions[0] = dao.motionDataRightWrist.position.x;
        // continuousActions[1] = dao.motionDataRightWrist.position.y;
        // continuousActions[2] = dao.motionDataRightWrist.position.z;
        
        continuousActions[0] = dao.motionDataRightWrist.velocity.x;
        continuousActions[1] = dao.motionDataRightWrist.velocity.y;
        continuousActions[2] = dao.motionDataRightWrist.velocity.z;
    }
    //GOALS AND REWARDS
    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent<AgentReward>(out AgentReward reward)){
            
            other.GetComponent<AgentReward>().SetCollider(false);
            other.GetComponent<AgentReward>().DisplayReward(); 
            if (other.GetComponent<AgentReward>().HomePosition()){
                
                SetReward(baseReward);
                EndEpisode();
            }
            else{
                SetReward(baseReward * bonusMultiplier);
                
            }

            //Maybe dont end here and let the agent get back to home???
            //EndEpisode();
        }
        if (other.TryGetComponent<AgentPenatly>(out AgentPenatly penatly)){
            SetReward(baseReward-(baseReward*4));
            other.GetComponent<AgentPenatly>().DisplayPenatly(); 
            EndEpisode();
        }

        if (other.gameObject.GetComponent<AgentReward>()){
            Debug.Log(other.name);
        }
    }
}
