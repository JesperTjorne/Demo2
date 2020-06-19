using System;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    private Animator m_anim;
    private NavMeshAgent m_navAgent;

    [SerializeField]
    private Transform[] m_checkPoints;
    private int m_checkPointIndex;

    void Start()
    {
        
    }

    void Update()
    {
        var target = GetTarget();
        m_navAgent.SetDestination(target);
    }

    private Vector3 GetTarget()
    {
        var target = m_checkPoints[m_checkPointIndex].position;
        if (Vector3.Distance(transform.position, target) < m_navAgent.stoppingDistance)
        {
            m_checkPointIndex++;
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
