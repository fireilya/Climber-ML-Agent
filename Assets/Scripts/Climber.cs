using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Buffers;
using Unity.MLAgents.Actuators;

public class Climber : Agent
{
    Rigidbody rb;
    [SerializeField]
    private float force = 50.0f;

    [SerializeField]
    private float maxVelocity = 3.0f;

    [SerializeField]
    private float jumpPower = 200.0f;

    [SerializeField]
    private Wall wall;

    [SerializeField]
    private Button button;

    [SerializeField]
    private GameObject rewardPlatform;

    [SerializeField]
    private float maxClimbSpeed = 3f;

    private bool isRewarded = false;
    private bool isButtonTouched = false;

    public bool isFloorColided;
    public bool isWallColided;

    private void OnCollisionEnter(Collision collision)
    {
        isRewarded = collision.gameObject.tag == "RewardPlatform";

        if (collision.gameObject.tag == "Button")
        {
            button.LiftWall();
            isButtonTouched = true;
        }
    }

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        transform.localPosition = new Vector3(0, 2, -2.5f);
        wall.transform.localPosition = new Vector3(0, -1.1f, 1.5f);
        button.transform.localPosition = new Vector3(0, 0.1f, 0.9f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(wall.transform.localPosition);
        sensor.AddObservation(button.transform.localPosition);
        sensor.AddObservation(rewardPlatform.transform.localPosition);
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 2, -2.5f);
        wall.transform.localPosition = new Vector3(0, -1.1f, 1.5f);
        button.transform.localPosition = new Vector3(0, 0.1f, 0.9f);
        isRewarded = isButtonTouched = isFloorColided = isWallColided = false;
        wall.StartNewEpisode();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude < maxVelocity)
        {
            if (actions.DiscreteActions[0]==1) rb.AddForce(new Vector3(1, 0, 0) * force);
            if (actions.DiscreteActions[0] == 2) rb.AddForce(new Vector3(1, 0, 0) * -force);
            if (actions.DiscreteActions[0] == 3) rb.AddForce(new Vector3(0, 0, 1) * force);
            if (actions.DiscreteActions[0] == 4) rb.AddForce(new Vector3(0, 0, 1) * -force);
        }
        if (actions.DiscreteActions[0] == 0) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (actions.DiscreteActions[1] == 1 && (isFloorColided | isWallColided && rb.velocity.y < maxClimbSpeed))
            rb.AddForce(new Vector3(0, 1, 0) * jumpPower);
            

        if (isRewarded) 
        {
            SetReward(isButtonTouched ? 0.5f : 1.0f);
            EndEpisode();
        }

        if (transform.position.y<0)
        {
            SetReward(isButtonTouched ? -0.5f : 0);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W)) discreteActions[0] = 1;
        if (Input.GetKey(KeyCode.S)) discreteActions[0] = 2;
        if (Input.GetKey(KeyCode.A)) discreteActions[0] = 3;
        if (Input.GetKey(KeyCode.D)) discreteActions[0] = 4;
        if (Input.GetKey(KeyCode.Space)) discreteActions[1] = 1;
        if(!Input.anyKey) discreteActions[0] = 0;
    }
}
