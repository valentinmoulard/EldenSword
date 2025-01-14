using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class WeaponStatsDistance 
{
    // [Range(0f, 1f)] public float StartEnableDamagingPart = 0.2f;
    // [Range(0f, 1f)] public float EndEnableDamagingPart = 0.8f;
}

public class Weapon_Distance : WeaponBase
{
    [SerializeField] private WeaponStatsDistance m_weaponStatsDistance = null;
}
