using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Transform m_controlledTransform = null;

    [SerializeField] private float m_smoothTime = 0.2f;

    private List<GameObject> m_objectList = new List<GameObject>();
    private Vector3 m_desiredPosition;
    private Vector3 m_smoothedPosition;
    private Vector3 m_velocity;


    private void OnEnable()
    {
        Boss_Initializer.OnBossInitilized += OnBossInitilized;
        Player_Initializer.OnPlayerInitilized += OnPlayerInitilized;
    }

    private void OnDisable()
    {
        Boss_Initializer.OnBossInitilized -= OnBossInitilized;
        Player_Initializer.OnPlayerInitilized -= OnPlayerInitilized;
    }

    private void Update()
    {
        CalculateDesiredPosition();
        MoveToPosition();
    }

    private void OnBossInitilized(GameObject boss)
    {
        m_objectList.Add(boss);
    }

    private void OnPlayerInitilized(GameObject player)
    {
        m_objectList.Add(player);
    }

    private void CalculateDesiredPosition()
    {
        if(m_objectList == null || m_objectList.Count == 0)
            return;
        
        m_desiredPosition = Vector3.zero;
        
        for (int i = 0; i < m_objectList.Count; i++)
        {
            m_desiredPosition += m_objectList[i].transform.position;
        }

        m_desiredPosition /= m_objectList.Count;

        m_smoothedPosition = Vector3.SmoothDamp(m_controlledTransform.position, m_desiredPosition, ref m_velocity,
            m_smoothTime);
    }

    private void MoveToPosition()
    {
        m_controlledTransform.position = m_smoothedPosition;
    }
}
