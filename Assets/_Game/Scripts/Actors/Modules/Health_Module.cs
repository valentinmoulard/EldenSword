using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health_Module : MonoBehaviour
{
    public Action OnDeath;

    [SerializeField] private float m_initialHP = 100f;

    [SerializeField] private Image m_healthFill = null;
    [SerializeField] private Image m_healthSmoothFill = null;

    [SerializeField] private float m_healthUpdateTime = 0.1f;
    [SerializeField] private float m_smoothedHealthUpdateTime = 0.5f;
    [SerializeField] private float m_delayBeforeUpdatingSmoothedHealth = 0.5f;

    [SerializeField] private bool m_isDebugEnabled;


    private Coroutine m_invincibilityCoroutine;
    private float m_currentHP;
    private float m_smoothedCurrentHP;
    private float m_healthPercent => m_currentHP / m_initialHP;
    private float m_smoothedHealthPercent => m_smoothedCurrentHP / m_initialHP;
    private float m_updateSmoothedCurrentHealthFillTimer;
    private float m_healthUpdateSpeed;
    private float m_smoothedHealthUpdateSpeed;
    private bool m_hasBeenHit;
    private bool m_isInvincible;
    public bool IsAlive { get; private set; }


    private void Update()
    {
        if (m_healthFill == null || m_healthSmoothFill == null)
            return;

        ManageSmoothedHealth();

        UpdateFillVisual();
    }

    public void SetHealthUIReference(Image healthFill, Image healthSmoothFill)
    {
        m_healthFill = healthFill;
        m_healthSmoothFill = healthSmoothFill;
    }
    
    public void Initialize()
    {
        m_currentHP = m_initialHP;

        m_healthFill.fillAmount = m_healthPercent;
        m_healthSmoothFill.fillAmount = m_smoothedHealthPercent;

        m_smoothedCurrentHP = m_currentHP;

        if (m_currentHP > 0)
            IsAlive = true;
    }

    private void ManageSmoothedHealth()
    {
        if (m_hasBeenHit == false)
            return;

        m_updateSmoothedCurrentHealthFillTimer += Time.deltaTime;

        if (m_updateSmoothedCurrentHealthFillTimer > m_delayBeforeUpdatingSmoothedHealth)
        {
            m_smoothedCurrentHP = Mathf.MoveTowards(m_smoothedCurrentHP, m_currentHP,
                m_smoothedHealthUpdateSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(m_currentHP - m_smoothedCurrentHP) < 1f)
        {
            m_hasBeenHit = false;
            m_smoothedCurrentHP = m_currentHP;
        }
    }

    private void UpdateFillVisual()
    {
        m_healthSmoothFill.fillAmount = m_smoothedHealthPercent;

        m_healthFill.fillAmount =
            Mathf.MoveTowards(m_healthFill.fillAmount, m_healthPercent, m_healthUpdateSpeed * Time.deltaTime);
    }

    public void ReduceHealth(float value)
    {
        if (IsAlive == false || m_isInvincible)
            return;

        m_hasBeenHit = true;
        m_updateSmoothedCurrentHealthFillTimer = 0f;

        m_currentHP -= value;
        ActivateInvincibility(0.5f);

        m_healthUpdateSpeed = Mathf.Abs(m_healthFill.fillAmount - m_healthPercent) / m_healthUpdateTime;
        m_smoothedHealthUpdateSpeed = Mathf.Abs(m_currentHP - m_smoothedCurrentHP) / m_smoothedHealthUpdateTime;

        if (m_currentHP <= 0f)
        {
            m_currentHP = 0;
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    public void ActivateInvincibility(float duration)
    {
        if (m_invincibilityCoroutine != null)
            StopCoroutine(m_invincibilityCoroutine);

        m_invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        m_isInvincible = true;
        yield return new WaitForSeconds(duration);
        m_isInvincible = false;
    }
}