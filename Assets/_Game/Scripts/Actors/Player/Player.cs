using UnityEngine;

public class Player : MonoBehaviour
{
    private Health_Module m_healthModule;

    private void Awake()
    {
        m_healthModule = GetComponent<Health_Module>();
    }

    private void OnEnable()
    {
        m_healthModule.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        m_healthModule.OnDeath -= OnDeath;
    }
    
    private void OnDeath()
    {
        GameoverCondition.OnGameover?.Invoke();
    }
}
