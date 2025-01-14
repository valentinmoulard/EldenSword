using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Health_Module m_healthModule;

    private void Awake()
    {
        m_healthModule = GetComponent<Health_Module>();
    }

    private void OnEnable()
    {
        UI_BossHealth.OnSendBossHealthUI += OnSendBossHealthUI;
        m_healthModule.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        UI_BossHealth.OnSendBossHealthUI -= OnSendBossHealthUI;
        m_healthModule.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        VictoryCondition.OnVictoryConditionMet?.Invoke();
    }


    private void OnSendBossHealthUI(Image healthFill, Image healthSmoothFill)
    {
        m_healthModule.SetHealthUIReference(healthFill, healthSmoothFill);
    }
}
