using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Action OnStartRunning;
    public Action OnStopRunning;

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_distanceBeforeStop = 1f;

    private Target_Module m_target;
    private Health_Module m_healthModule;
    private Attack_Module m_attack;
    private Dodge_Module m_dodgeModule;
    private Vector3 m_movementDirection;
    private bool m_isRunning;
    public bool canRotate;

    private bool m_canMove
    {
        get => !m_dodgeModule.IsDodging && !m_attack.IsInAttackRange;
    }

    private void Awake()
    {
        m_target = GetComponent<Target_Module>();
        m_attack = GetComponent<Attack_Module>();
        m_dodgeModule = GetComponent<Dodge_Module>();
        m_healthModule = GetComponent<Health_Module>();
    }

    private void Update()
    {
        if(!m_healthModule.IsAlive)
            return;
        
        Move();
        ManageForwardDirection();
    }

    private void ManageForwardDirection()
    {
        if (CanRotate() == false)
            return;

        m_movementDirection = m_target.Target.transform.position - transform.position;
        m_movementDirection.y = 0f;
        if (Vector3.Angle(transform.forward, m_movementDirection) > 0.1f)
            transform.forward = m_movementDirection;
    }

    private void Move()
    {
        if (CanRun() == false)
            return;

        m_movementDirection = m_target.Target.transform.position - transform.position;
        m_movementDirection.y = 0f;
        transform.position += m_movementDirection.normalized * (m_movementSpeed * Time.deltaTime);
    }

    private bool CanRotate()
    {
        canRotate = (m_target.Target != null && !m_dodgeModule.IsDodging) || m_attack.IsAttacking;

        return canRotate;
    }

    private bool CanRun()
    {
        bool canRun = (m_canMove && m_target.Target != null &&
                       Vector3.Distance(transform.position, m_target.Target.transform.position) > m_distanceBeforeStop);

        if (canRun && !m_isRunning)
        {
            m_isRunning = true;
            OnStartRunning?.Invoke();
        }
        else if (!canRun && m_isRunning)
        {
            m_isRunning = false;
            OnStopRunning?.Invoke();
        }

        return canRun;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_distanceBeforeStop);
    }
}