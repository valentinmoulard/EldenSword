using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class WeaponStatsMelee
{
    [Range(0f, 1f)] public float StartEnableDamagingPart = 0.2f;
    [Range(0f, 1f)] public float EndEnableDamagingPart = 0.8f;
    public float MeleeDamage = 10f;
}

public class Weapon_Melee : WeaponBase
{
    [SerializeField] private WeaponStatsMelee m_weaponStatsMelee = null;

    [FormerlySerializedAs("m__meleeAnimatorColliderController")] [SerializeField] private WeaponMelee_AnimatorColliderController m_meleeAnimatorColliderController = null;
    
    private Coroutine m_manageDamagingPartCoroutine;
    private Collider m_damagingCollider;
    private Health_Module m_targetHealthModule;

    private void Awake()
    {
        m_damagingCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        m_meleeAnimatorColliderController.OnEnableDamagingCollider += OnEnableDamagingCollider;
        m_meleeAnimatorColliderController.OnDisableDamagingCollider += OnDisableDamagingCollider;
    }

    private void OnDisable()
    {
        m_meleeAnimatorColliderController.OnEnableDamagingCollider -= OnEnableDamagingCollider;
        m_meleeAnimatorColliderController.OnDisableDamagingCollider -= OnDisableDamagingCollider;
    }

    private void Start()
    {
        ToggleDamaging(false);
    }

    private void OnEnableDamagingCollider()
    {
        ToggleDamaging(true);
    }

    private void OnDisableDamagingCollider()
    {
        ToggleDamaging(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (m_damagingCollider.enabled == false)
            return;

        if (CompareLayer.IsLayerInLayerMask(other.gameObject.layer, m_effectiveLayer))
        {
            m_targetHealthModule = other.gameObject.GetComponent<Health_Module>();

            if (m_targetHealthModule != null)
            {
                if (m_isDebugEnabled)
                    Debug.Log(other.gameObject, other.gameObject);

                m_targetHealthModule.ReduceHealth(m_weaponStatsMelee.MeleeDamage);
            }
        }
    }


    private void ToggleDamaging(bool state)
    {
        m_damagingCollider.enabled = state;
    }
}