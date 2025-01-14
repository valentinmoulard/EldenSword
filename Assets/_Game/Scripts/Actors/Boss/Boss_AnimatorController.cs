using System;
using UnityEngine;

public class Boss_AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator m_animator = null;

    private Attack_Module m_attackModule;
    private Jump_Module m_jumpModule;
    private Health_Module m_healthModule;

    private void Awake()
    {
        m_attackModule = GetComponent<Attack_Module>();
        m_jumpModule = GetComponent<Jump_Module>();
        m_healthModule = GetComponent<Health_Module>();
    }


    private void OnEnable()
    {
        m_attackModule.OnAttackStart += OnAttackStart;
        m_attackModule.OnAttackCancel += OnAttackCancel;

        m_jumpModule.OnJump += OnJump;
        m_jumpModule.OnLand += OnLand;

        m_healthModule.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        m_attackModule.OnAttackStart -= OnAttackStart;
        m_attackModule.OnAttackCancel -= OnAttackCancel;
        
        m_jumpModule.OnJump -= OnJump;
        m_jumpModule.OnLand -= OnLand;
        
        m_healthModule.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        m_animator.SetTrigger("Die");
    }
    
    private void OnJump()
    {
        m_animator.SetTrigger("Jump");
    }

    private void OnLand()
    {
        m_animator.SetTrigger("Land");
    }
    
    private void OnAttackStart(WeaponStatsBase weaponStatsBase)
    {
        switch (weaponStatsBase.WeaponType)
        {
            case WeaponType.Sword:
                break;
            case WeaponType.Bow:
                break;
            case WeaponType.Fist:
                m_animator.SetTrigger("Attack");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnAttackCancel(WeaponStatsBase weaponStatsBase)
    {
        m_animator.SetTrigger("CancelAttack");
    }
}