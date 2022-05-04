using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyRayCtrl : Agent
{
    private Transform tr;
    private Rigidbody rb;

    private StageManager stageManager;

    public override void Initialize()
    {
        MaxStep = 5000;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        stageManager = transform.root.GetComponent<StageManager>();
    }

    public override void OnEpisodeBegin()
    {
        stageManager.InitStage();

        // 물리엔진의 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;
        // 에이전트의 위치 변경
        tr.localPosition = new Vector3(Random.Range(-20.0f, 20.0f),
                                       0.05f,
                                       Random.Range(-20.0f, 20.0f));
        // 에이전트의 회전값 변경
        tr.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector 관측
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions; //Discrete (0, 1, 2, 3, ...)

        //Debug.Log($"[0]={action[0]}, [1]={action[1]}");

        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        // Branch 0 => action[0]
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        // Branch 1 => action[1]
        switch (action[1])
        {
            case 1: rot = -tr.up; break; // 왼쪽 회전
            case 2: rot = tr.up; break;  // 오른쪽 회전
        }

        tr.Rotate(rot, Time.fixedDeltaTime * 200.0f);
        rb.AddForce(dir * 1.5f, ForceMode.VelocityChange);

        // 지속적인 움직임을 유도하기 위한 마이너스 페널티
        AddReward(-1 / (float)MaxStep); // -1/5000 = -0.005f
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*
            Branch 0 = 0, 1, 2 => 3개
            Branch 1 = 0, 1, 2 => 3개
        */

        var action = actionsOut.DiscreteActions;
        action.Clear();

        // 전진/후진 이동 - Branch 0 = (0:정지, 1:전진, 2:후진)
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1; // 전진
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2; // 후진
        }

        // 좌/우 회전 - Branch 1 = (0:무회전, 1:왼쪽회전, 2:오른쪽회전)
        if (Input.GetKey(KeyCode.A))
        {
            action[1] = 1; // 왼쪽회전
        }
        if (Input.GetKey(KeyCode.D))
        {
            action[1] = 2; // 오른쪽회전
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("GOOD_ITEM"))
        {
            AddReward(+1.0f);
            rb.velocity = rb.angularVelocity = Vector3.zero;
            Destroy(coll.gameObject);
        }

        if (coll.collider.CompareTag("BAD_ITEM"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            AddReward(-0.01f);
        }
    }
}
