using System;
using UnityEngine;

public class Attack_Module : MonoBehaviour
{
    public Action<WeaponStatsBase> OnAttackStart;
    public Action<WeaponStatsBase> OnAttackCancel;

    [SerializeField] private WeaponBase m_weapon = null;

    private Health_Module m_healthModule;
    
    public bool IsAttacking => m_weapon.IsUsingWeapon;
    public bool IsInAttackRange => m_weapon.IsInWeaponRange;

    private void Awake()
    {
        m_healthModule = GetComponent<Health_Module>();
        m_weapon.InitializeWeapon(this, 
            GetComponent<Target_Module>(), 
            GetComponent<Jump_Module>(),
            GetComponent<Dodge_Module>());
    }

    private void OnEnable()
    {
        m_healthModule.OnDeath += OnDeath;
        m_weapon.OnWeaponUseStart += OnWeaponUseStart;
        m_weapon.OnWeaponUseCancel += OnWeaponUseCancel;
    }

    private void OnDisable()
    {
        m_healthModule.OnDeath -= OnDeath;
        m_weapon.OnWeaponUseStart -= OnWeaponUseStart;
        m_weapon.OnWeaponUseCancel -= OnWeaponUseCancel;
    }

    private void OnDeath()
    {
        m_weapon.ToggleWeaponUseState(false);
    }

    private void OnWeaponUseStart(WeaponStatsBase weaponStatsBase)
    {
        if(!m_healthModule.IsAlive)
            return;
        
        OnAttackStart?.Invoke(weaponStatsBase);
    }

    private void OnWeaponUseCancel(WeaponStatsBase weaponStatsBase)
    {
        if(!m_healthModule.IsAlive)
            return;
        
        OnAttackCancel?.Invoke(weaponStatsBase);
    }
}