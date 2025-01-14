using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Jump_Module : MonoBehaviour
{
    public Action OnJump;
    public Action OnLand;

    [SerializeField] private float m_jumpDuration = 1f;

    [SerializeField] private float m_jumpDistance = 15f;

    [SerializeField] private float m_jumpHeight = 5f;

    [SerializeField] private float m_minTimeBeforeJump = 5f;

    [SerializeField] private float m_maxTimeBeforeJump = 20f;

    private Health_Module m_healthModule;
    private Target_Module m_targetModule;
    private Attack_Module m_attackModule;
    private Quaternion m_startRotation;
    private Vector3 m_targetRotation;
    private Coroutine m_jumpCoroutine;
    private Vector3 m_destination;
    private Vector3 m_startPosition;
    private float m_jumpSpeedProgression;
    private float m_jumpProgression;
    private float m_delayToAllowJump;
    private float m_delayToAllowJumpTimer;
    public bool IsJumping { get; private set; }

    private void Awake()
    {
        m_targetModule = GetComponent<Target_Module>();
        m_attackModule = GetComponent<Attack_Module>();
        m_healthModule = GetComponent<Health_Module>();
    }

    public void Initialize()
    {
        m_delayToAllowJump = Random.Range(m_minTimeBeforeJump, m_maxTimeBeforeJump);
        StartCoroutine(ManageJumpCoroutine());
    }

    private IEnumerator ManageJumpCoroutine()
    {
        while (true)
        {
            if (m_healthModule.IsAlive && !IsJumping && !m_attackModule.IsAttacking && m_targetModule.Target != null)
            {
                if (m_delayToAllowJumpTimer < m_delayToAllowJump)
                {
                    m_delayToAllowJumpTimer += Time.deltaTime;
                }
                else
                {
                    m_delayToAllowJumpTimer = 0f;

                    m_delayToAllowJump = Random.Range(m_minTimeBeforeJump, m_maxTimeBeforeJump);

                    Jump();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void Jump()
    {
        if (IsJumping && m_attackModule.IsAttacking)
            return;

        IsJumping = true;
        m_destination = Random.insideUnitSphere;
        m_destination.y = 0;
        m_destination.Normalize();
        m_destination *= m_jumpDistance;

        if (m_jumpCoroutine != null)
            StopCoroutine(m_jumpCoroutine);

        m_jumpCoroutine = StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        OnJump?.Invoke();

        m_startPosition = transform.position;
        m_startRotation = transform.rotation;
        m_jumpProgression = 0f;
        m_jumpSpeedProgression = 1f / m_jumpDuration;

        m_healthModule.ActivateInvincibility(m_jumpDuration);

        while (m_jumpProgression < 1f)
        {
            m_jumpProgression += m_jumpSpeedProgression * Time.deltaTime;

            transform.position = MathParabola.Parabola(m_startPosition, m_destination, m_jumpHeight, m_jumpProgression);

            if (m_targetModule != null && m_targetModule.Target != null)
            {
                m_targetRotation = m_targetModule.Target.transform.position - transform.position;
                m_targetRotation.y = 0f;
                Quaternion rotation = Quaternion.LookRotation(m_targetRotation, Vector3.up);
                transform.rotation =
                    Quaternion.Lerp(m_startRotation, rotation, m_jumpProgression);
            }

            yield return new WaitForEndOfFrame();
        }

        transform.position = m_destination;
        IsJumping = false;

        OnLand?.Invoke();
    }
}