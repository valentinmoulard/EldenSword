using System;
using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow,
    Fist
}

[Serializable]
public class WeaponStatsBase
{
    public WeaponType WeaponType;
    public float WeaponRange = 1f;
    public float UseDuration = 1f;
    public float UseCooldown = 2f;
}

public class WeaponBase : MonoBehaviour
{
    public Action<WeaponStatsBase> OnWeaponUseStart;
    public Action<WeaponStatsBase> OnWeaponUseEnd;
    public Action<WeaponStatsBase> OnWeaponUseCancel;

    [SerializeField] private Transform m_actorWielderTransform = null;

    [SerializeField] protected LayerMask m_effectiveLayer = 0;

    [SerializeField] protected WeaponStatsBase m_weaponStatsBase = null;

    [SerializeField] protected bool m_isDebugEnabled = false;

    private Jump_Module m_jumpModule;
    private Attack_Module m_attackModule;
    private Dodge_Module m_dodgeModule;
    protected Target_Module m_targetModule;
    private Coroutine m_weaponUseCoroutine;
    private float m_weaponUseCooldownTimer;
    private bool m_isWeaponUseInCooldown;
    public bool IsUsingWeapon { get; private set; }
    public bool IsInWeaponRange { get; private set; }

    private bool m_canUseWeapon;

    public void InitializeWeapon(Attack_Module attackModule = default,
        Target_Module newTargetModule = default,
        Jump_Module newJumpModule = default,
        Dodge_Module newDodgeModule = default)
    {
        m_attackModule = attackModule;
        m_targetModule = newTargetModule;
        m_jumpModule = newJumpModule;
        m_dodgeModule = newDodgeModule;
        ToggleWeaponUseState(true);
    }

    private void Update()
    {
        if (m_attackModule.enabled == false || !m_canUseWeapon)
            return;

        CheckForWeaponRange();

        ManageWeaponCooldown();
    }

    private void CheckForWeaponRange()
    {
        if (m_targetModule.Target == null)
            return;

        if (m_jumpModule != null && m_jumpModule.IsJumping)
            return;

        if (m_dodgeModule != null && m_dodgeModule.IsDodging)
            return;

        if (Vector3.Distance(m_targetModule.transform.position, m_targetModule.Target.transform.position) <
            m_weaponStatsBase.WeaponRange)
        {
            IsInWeaponRange = true;

            UseWeapon();
        }
        else
        {
            IsInWeaponRange = false;
        }
    }

    private void UseWeapon()
    {
        if (m_isWeaponUseInCooldown)
            return;

        if (IsUsingWeapon)
            return;

        m_isWeaponUseInCooldown = true;
        m_weaponUseCoroutine = StartCoroutine(WeaponUseCoroutine());
    }

    private IEnumerator WeaponUseCoroutine()
    {
        IsUsingWeapon = true;
        OnWeaponUseStart?.Invoke(m_weaponStatsBase);
        TriggerWeaponUseEffect();
        yield return new WaitForSeconds(m_weaponStatsBase.UseDuration);
        IsUsingWeapon = false;
        OnWeaponUseEnd?.Invoke(m_weaponStatsBase);
    }

    protected virtual void TriggerWeaponUseEffect()
    {
    }

    private void ManageWeaponCooldown()
    {
        if (m_isWeaponUseInCooldown == false)
            return;

        m_weaponUseCooldownTimer += Time.deltaTime;

        if (m_weaponUseCooldownTimer > m_weaponStatsBase.UseCooldown)
        {
            m_weaponUseCooldownTimer = 0;
            m_isWeaponUseInCooldown = false;
        }
    }

    public void CancelAttack()
    {
        if (m_weaponUseCoroutine != null)
            StopCoroutine(m_weaponUseCoroutine);

        OnWeaponUseCancel?.Invoke(m_weaponStatsBase);

        IsUsingWeapon = false;
        OnWeaponUseEnd?.Invoke(m_weaponStatsBase);
        m_isWeaponUseInCooldown = true;
    }

    public void ToggleWeaponUseState(bool state)
    {
        m_canUseWeapon = state;

        if (m_canUseWeapon == false)
            CancelAttack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (m_actorWielderTransform == null)
            return;

        Gizmos.DrawWireSphere(m_actorWielderTransform.position, m_weaponStatsBase.WeaponRange);
    }
}