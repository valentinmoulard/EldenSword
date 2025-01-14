using System;
using UnityEngine;

public class Boss_Initializer : MonoBehaviour
{
    public static Action<GameObject> OnBossInitilized;

    private Target_Module m_targetModule;
    private Health_Module m_healthModule;
    private Jump_Module m_jumpModule;

    private void Awake()
    {
        m_targetModule = GetComponent<Target_Module>();
        m_healthModule = GetComponent<Health_Module>();
        m_jumpModule = GetComponent<Jump_Module>();
    }

    private void OnEnable()
    {
        Manager_Level.OnLevelInstantiated += Initialize;
    }

    private void OnDisable()
    {
        Manager_Level.OnLevelInstantiated -= Initialize;
    }


    private void Initialize()
    {
        m_targetModule.Initialize();
        m_healthModule.Initialize();
        m_jumpModule.Initialize();
    }

    private void Start()
    {
        OnBossInitilized?.Invoke(gameObject);
    }
}