using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BotController : MonoBehaviour
{
    private Animator m_anim;
    private NavMeshAgent m_navAgent;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;

    public int StartCheckpoint;
    
    private List<Transform> m_checkPoints;
    private int m_checkPointIndex;

    void Awake()
    {
        m_checkPoints = GameObject.Find("NavCheckPoints").GetComponent<NavCheckPoints>().CheckPoints;
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        var target = GetTarget();
        m_navAgent.SetDestination(target);
        Move();
    }

    private void Move()
    {
        
    }

    private Vector3 GetTarget()
    {
        var target = m_checkPoints[m_checkPointIndex].position;

        
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
        //var rigid = GetComponent<Rigidbody>();
        //var x = rigid.velocity.x;
        //var y = rigid.velocity.y;

        //var moving = Math.Abs(x) + Math.Abs(y) > 0.25 ? true : false;

        //var x = 1.0f;
        //var y = 1.0f;
        //var moving = true;

        //m_anim.SetFloat("VelocityX", x);
        //m_anim.SetFloat("VelocityY", y);

        //m_anim.SetBool("Moving", moving);

        Vector3 worldDeltaPosition = m_navAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && m_navAgent.remainingDistance > m_navAgent.radius;

        // Update animation parameters
        m_anim.SetBool("Moving", shouldMove);
        m_anim.SetFloat("VelocityX", velocity.x);
        m_anim.SetFloat("VelocityY", velocity.y);

        GetComponent<LookAt>().lookAtTargetPosition = m_navAgent.steeringTarget + transform.forward;
    }
}
