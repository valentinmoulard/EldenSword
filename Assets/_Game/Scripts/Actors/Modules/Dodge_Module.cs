using System;
using System.Collections;
using UnityEngine;

public class Dodge_Module : MonoBehaviour
{
    public Action<float> OnDodgeStart;
    public Action OnDodgeEnd;

    [SerializeField] private float m_dodgeDistance = 2f;
    [SerializeField] private float m_dodgeDuration = 0.5f;
    [SerializeField] private float m_dodgeCooldown = 1f;

    [SerializeField] private float m_iFramesStart = 0.33f;
    [SerializeField] private float m_iFramesEnd = 0.66f;
    
    private Coroutine m_dodgeCoroutine;
    private Coroutine m_iFramesCoroutine;
    private Attack_Module m_attackModule;
    private Health_Module m_healthModule;
    private Vector3 m_dodgeRegisteredDirectionBuffer;
    private float m_dodgeSpeed => m_dodgeDistance / m_dodgeDuration;
    private float m_dodgeCooldownTimer;
    private bool m_canDodge => !m_isDodgeInCooldown && !m_attackModule.IsAttacking && m_dodgeRegisteredDirectionBuffer != Vector3.zero;
    private bool m_isDodgeInCooldown;
    private bool m_isDodging;
    public bool IsDodging => m_isDodging;


    private void Awake()
    {
        m_attackModule = GetComponent<Attack_Module>();
        m_healthModule = GetComponent<Health_Module>();
    }

    private void OnEnable()
    {
        Controller.OnSwipe += RegisterDodgeDirection;
    }

    private void OnDisable()
    {
        Controller.OnSwipe -= RegisterDodgeDirection;
    }

    private void Update()
    {
        if(!m_healthModule.IsAlive)
            return;
        
        ManageDodge();
        ManageDodgeCooldown();
    }

    private void ManageDodge()
    {
        if (m_canDodge)
            TriggerDodge();
    }
    
    private void ManageDodgeCooldown()
    {
        if(m_isDodgeInCooldown == false)
            return;
        
        m_dodgeCooldownTimer += Time.deltaTime;

        if (m_dodgeCooldownTimer > m_dodgeCooldown)
        {
            m_isDodgeInCooldown = false;
            m_dodgeCooldownTimer = 0;
        }
    }
    
    private void RegisterDodgeDirection(Vector3 swipeDirection)
    {
        m_dodgeRegisteredDirectionBuffer = new Vector3(swipeDirection.x, 0f, swipeDirection.y);
    }

    private void TriggerDodge()
    {
        if(m_dodgeCoroutine != null)
            StopCoroutine(m_dodgeCoroutine);
        
        m_dodgeCoroutine = StartCoroutine(DodgeCoroutine(m_dodgeRegisteredDirectionBuffer));
        
        if(m_iFramesCoroutine != null)
            StopCoroutine(m_iFramesCoroutine);
        
        m_iFramesCoroutine = StartCoroutine(ManageIFramesCoroutine());
    }
    
    private IEnumerator DodgeCoroutine(Vector3 dodgeDirection)
    {
        float m_timer = 0;
        
        m_dodgeRegisteredDirectionBuffer = Vector3.zero;
        
        m_isDodging = true;
        
        m_isDodgeInCooldown = true;

        OnDodgeStart?.Invoke(m_dodgeDuration);
        
        transform.forward = dodgeDirection;

        while (m_timer < m_dodgeDuration)
        {
            m_timer += Time.deltaTime;
            transform.position += dodgeDirection.normalized * (m_dodgeSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        m_isDodging = false;
        OnDodgeEnd?.Invoke();
    }

    private IEnumerator ManageIFramesCoroutine()
    {
        float startTime = m_dodgeDuration * m_iFramesStart;
        float endTime = m_dodgeDuration * m_iFramesEnd;

        yield return new WaitForSeconds(startTime);
        m_healthModule.ActivateInvincibility(endTime - startTime);
    }

}
