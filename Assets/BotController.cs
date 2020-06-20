using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    private Animator m_anim;
    private NavMeshAgent m_navAgent;

    private List<Transform> m_checkPoints;

    [SerializeField]
    private int m_checkPointIndex;

    void Awake() 
    {
        m_anim = GetComponent<Animator>();
        m_checkPoints = GameObject.Find("NavCheckPoints").GetComponent<NavCheckPoints>().CheckPoints;
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Debug.Log($"CheckpointIndex: {m_checkPointIndex}");
        var target = GetTarget();
        m_navAgent.SetDestination(target);
    }

    private Vector3 GetTarget()
    {
        var target = m_checkPoints[m_checkPointIndex].position;

        Debug.Log($"Target: {m_checkPoints[m_checkPointIndex]}");
        if (Vector3.Distance(transform.position, target) < m_navAgent.stoppingDistance)
        {
            m_checkPointIndex++;
            if (m_checkPointIndex > m_checkPoints.Count) m_checkPointIndex = 0;
            return m_checkPoints[m_checkPointIndex].position;
        }

        return target;
    }

    void LateUpdate() => SetAnimationVariables();

    private void SetAnimationVariables()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var moving = Math.Abs(x) + Math.Abs(y) > 0.25 ? true : false;

        m_anim.SetFloat("VelocityX", x);
        m_anim.SetFloat("VelocityY", y);

        m_anim.SetBool("Moving", moving);
    }
}
