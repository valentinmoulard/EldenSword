using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    public static Action<Image, Image> OnSendBossHealthUI;

    [SerializeField] private Image m_healthFill = null;
    [SerializeField] private Image m_healthSmoothFill = null;


    private void OnEnable()
    {
        Manager_Level.OnLevelInstantiated += OnLevelInstantiated;
    }

    private void OnDisable()
    {
        Manager_Level.OnLevelInstantiated -= OnLevelInstantiated;
    }


    private void OnLevelInstantiated()
    {
        OnSendBossHealthUI?.Invoke(m_healthFill, m_healthSmoothFill);
    }
}
