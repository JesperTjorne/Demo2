using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    private Animator m_anim;
    private NavMeshAgent m_navAgent;
    private LookAt m_lookAt;

    private List<Transform> m_checkPoints;

    [SerializeField]
    private bool m_reverseCheckPointDirection;
    [SerializeField]
    private int m_checkPointIndex;

    void Awake() 
    {
        m_anim = GetComponent<Animator>();
        m_checkPoints = GameObject.Find("NavCheckPoints").GetComponent<NavCheckPoints>().CheckPoints;
        m_navAgent = GetComponent<NavMeshAgent>();
        m_lookAt = GetComponent<LookAt>();
    }

    void Update()
    {
        var target = GetTarget();
        m_navAgent.SetDestination(target);

        // TODO: Look at player
        m_lookAt.lookAtTargetPosition = m_navAgent.steeringTarget + transform.forward;

        transform.rotation = Quaternion.LookRotation(m_navAgent.steeringTarget + transform.forward);

    }

    private Vector3 GetTarget()
    {
        var target = m_checkPoints[m_checkPointIndex].position;

        if (Vector3.Distance(transform.position, target) < m_navAgent.stoppingDistance)
        {
            if (m_reverseCheckPointDirection) 
            {
                m_checkPointIndex--;
                if (m_checkPointIndex < 0) m_checkPointIndex = m_checkPoints.Count - 1;
            }
            else 
            {
                m_checkPointIndex++;
                if (m_checkPointIndex > m_checkPoints.Count - 1) m_checkPointIndex = 0;
            }
            target = m_checkPoints[m_checkPointIndex].position;
        }

        return target;
    }

    void LateUpdate() => SetAnimationVariables();

    private void SetAnimationVariables()
    {
        // var x = Input.GetAxis("Horizontal");
        // var y = Input.GetAxis("Vertical");

        var x = m_navAgent.velocity.x;
        var y = m_navAgent.velocity.z;

        // var moving = Math.Abs(x) + Math.Abs(y) > 0.25 ? true : false;
        var moving = true;

        m_anim.SetFloat("VelocityX", x);
        m_anim.SetFloat("VelocityY", y);

        m_anim.SetBool("Moving", moving);
    }
}
