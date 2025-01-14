using System;
using UnityEngine;

public class WeaponMelee_AnimatorColliderController : MonoBehaviour
{
    public Action OnEnableDamagingCollider;
    public Action OnDisableDamagingCollider;

    public void SendEnabledDamagingCollider()
    {
        OnEnableDamagingCollider?.Invoke();
    }
    
    public void SendDisabledDamagingCollider()
    {
        OnDisableDamagingCollider?.Invoke();
    }
}
