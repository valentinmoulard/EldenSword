using System;
using UnityEngine;

public class Player_AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator m_animator = null;

    private Dodge_Module m_dodgeModule;
    private Player_Movement m_playerMovement;
    private Attack_Module m_attackModule;
    private Health_Module m_healthModule;


    private void Awake()
    {
        m_dodgeModule = GetComponent<Dodge_Module>();
        m_attackModule = GetComponent<Attack_Module>();
        m_playerMovement = GetComponent<Player_Movement>();
        m_healthModule = GetComponent<Health_Module>();
    }

    private void OnEnable()
    {
        m_dodgeModule.OnDodgeStart += OnDodgeStart;
        m_dodgeModule.OnDodgeEnd += OnDodgeEnd;

        m_attackModule.OnAttackStart += OnAttackStart;
        m_attackModule.OnAttackCancel += OnAttackCancel;

        m_playerMovement.OnStartRunning += OnStartRunning;
        m_playerMovement.OnStopRunning += OnStopRunning;

        m_healthModule.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        m_dodgeModule.OnDodgeStart -= OnDodgeStart;
        m_dodgeModule.OnDodgeEnd -= OnDodgeEnd;

        m_attackModule.OnAttackStart -= OnAttackStart;
        m_attackModule.OnAttackCancel -= OnAttackCancel;

        m_playerMovement.OnStartRunning -= OnStartRunning;
        m_playerMovement.OnStopRunning -= OnStopRunning;
        
        m_healthModule.OnDeath -= OnDeath;
    }
    
    private void OnDeath()
    {
        m_animator.SetTrigger("Die");
    }

    private void OnStartRunning()
    {
        m_animator.SetBool("IsRunning", true);
    }

    private void OnStopRunning()
    {
        m_animator.SetBool("IsRunning", false);
    }

    private void OnAttackStart(WeaponStatsBase weaponStatsBase)
    {
        //m_animator.speed = 1f / weaponStatsBase.UseDuration;

        switch (weaponStatsBase.WeaponType)
        {
            case WeaponType.Sword:
                m_animator.SetTrigger("Attack");
                break;
            case WeaponType.Bow:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnAttackCancel(WeaponStatsBase weaponStatsBase)
    {
        m_animator.SetTrigger("CancelAttack");
    }
    
    private void OnDodgeStart(float dodgeDuration)
    {
        m_animator.speed = 1 / dodgeDuration;
        m_animator.SetTrigger("Roll");
    }

    private void OnDodgeEnd()
    {
        m_animator.speed = 1;
    }
}