using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ReloadEvent
{
    DetachMagazine,
    ThrowMagazine,
    ReplenishMagazine,
    AttachNewMagazine
}

public class ReloadAnimationEvent : UnityEvent<ReloadEvent>
{

}
public class WeaponAnimationEvents : MonoBehaviour
{
    public ReloadAnimationEvent reloadAnimationEvent = new ReloadAnimationEvent();

    public void DetatchMagazine()
    {
        reloadAnimationEvent.Invoke(ReloadEvent.DetachMagazine);
    }

    public void ThrowMagazine()
    {
        reloadAnimationEvent.Invoke(ReloadEvent.ThrowMagazine);
    }

    public void ReplenishMagazine()
    {
        reloadAnimationEvent.Invoke(ReloadEvent.ReplenishMagazine);
    }

    public void AttachNewMagazine()
    {
        reloadAnimationEvent.Invoke(ReloadEvent.AttachNewMagazine);
    }
}
