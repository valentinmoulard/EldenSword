using System;
using UnityEngine;

public class Player_Initializer : MonoBehaviour
{
    public static Action<GameObject> OnPlayerInitilized;
    
    private Target_Module m_targetModule;
    private Health_Module m_healthModule;


    private void Awake()
    {
        m_targetModule = GetComponent<Target_Module>();
        m_healthModule = GetComponent<Health_Module>();
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
        
    }

    private void Start()
    {
        OnPlayerInitilized?.Invoke(gameObject);
    }
}
