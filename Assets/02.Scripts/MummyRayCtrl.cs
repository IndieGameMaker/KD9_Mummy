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
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
