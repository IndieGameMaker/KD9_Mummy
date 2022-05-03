using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

/*
    주변 환경을 관측(Observations)
    정책에 의해 행동(Actions)
    보상(Reward)
*/

public class MummyCtrl : Agent
{
    private Transform tr;
    private Rigidbody rb;

    public Transform targetTr;

    // 초기화 작업
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        targetTr = tr.parent.Find("Target").GetComponent<Transform>();
    }

    // 학습(에피소드)이 시작될 때 마다 호출되는 콜백
    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 위치 초기화
        tr.localPosition = new Vector3(Random.Range(-4.0f, +4.0f),
                                        0.05f,
                                        Random.Range(-4.0f, 4.0f));

        targetTr.localPosition = new Vector3(Random.Range(-4.0f, +4.0f),
                                        0.55f,
                                        Random.Range(-4.0f, 4.0f));
                               
    }

    // 주변환경을 관측 및 수집정보를 브레인 전달
    public override void CollectObservations(VectorSensor sensor)
    {
        /*
            수치관측 (Vector Observation)

            - 연속 수치 (Continues) : -1.0f ~ 0.0f ~ +1.0f
            - 이산 수치 (Discrete)  : -1.0f, 0.0f, +1.0f 
        */

        // 타겟의 위치 관측
        sensor.AddObservation(targetTr.localPosition);      // 3

        // 자신의 위치를 관측
        sensor.AddObservation(tr.localPosition);            // 3

        // 속도 관측
        sensor.AddObservation(rb.velocity.x);               // 1
        sensor.AddObservation(rb.velocity.z);               // 1
    }

    // 브레인으로 부터 전달 받은 명령
    public override void OnActionReceived(ActionBuffers actions)
    {
        // 전진/후진
        // 왼쪽/오른쪽

        // 전달받은 명령대로 행동(Action)
        var action = actions.ContinuousActions;

        Debug.Log($"[0]={action[0]}, [1]={action[1]}");

        Vector3 dir = (Vector3.forward * action[0]) + (Vector3.right * action[1]);
        rb.AddForce(dir.normalized * 30.0f);

        // 지속적인 움직임을 유도하기 위한 마이너스 페널티
        SetReward(-0.001f);
    }

    // 개발자 테스트용 가상 명령
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;
        // 전진/후진
        action[0] = Input.GetAxis("Vertical"); // Up/Down, W/S  -1.0f ~ 0.0f ~ +1.0f
        // 좌/우
        action[1] = Input.GetAxis("Horizontal"); // Left/Right , A/D
    }
}
