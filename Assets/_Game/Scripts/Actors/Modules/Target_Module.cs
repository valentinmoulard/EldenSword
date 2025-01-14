using System;
using UnityEngine;

public class Target_Module : MonoBehaviour
{
    public static Action<Target_Module> OnTargetDeath;
    
    [SerializeField] private LayerMask m_targetLayer = 0;

    [SerializeField] private float m_detectionRange = 10f; 
    
    private GameObject m_target;
    private Health_Module m_healthModule;
    
    public GameObject Target
    {
        get => m_target;
    }

    private void Awake()
    {
        m_healthModule = GetComponent<Health_Module>();
    }

    private void OnEnable()
    {
        m_healthModule.OnDeath += OnDeath;
        OnTargetDeath += ManageDeathOfTarget;
    }

    private void OnDisable()
    {
        m_healthModule.OnDeath -= OnDeath;
        OnTargetDeath -= ManageDeathOfTarget;
    }

    public void Initialize()
    {
        if (m_target == null)
        {
            TryFindTarget();
        }
    }

    private void OnDeath()
    {
        OnTargetDeath?.Invoke(this);
    }

    private void ManageDeathOfTarget(Target_Module target)
    {
        if (target == this)
            return;

        if(target.gameObject != m_target)
            return;

        m_target = null;
    }

    private void TryFindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRange, m_targetLayer);
        m_target = colliders.Length > 0 ? colliders[0].gameObject : null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);
    }
}
