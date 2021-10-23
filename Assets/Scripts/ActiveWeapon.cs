using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActiveWeapon : MonoBehaviour
{
    public Transform firingTarget;
    public Rig handIKRig;

    Weapon weapon;

    void Awake()
    {
        Weapon existingWeapon = GetComponentInChildren<Weapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    void Update()
    {
        if (weapon == null)
        {
            handIKRig.weight = 0;
        }
    }

    private void Equip(Weapon newWeapon)
    {
        weapon = newWeapon;
        weapon.raycastDestination = firingTarget;
        handIKRig.weight = 1;
    }

    public void StartFiring()
    {
        weapon.StartFiring();
    }

    public void UpdateFiring(float deltaTime)
    {
        weapon.UpdateFiring(deltaTime);
    }

    public void UpdateBullets(float deltaTime)
    {
        weapon.UpdateBullets(deltaTime);
    }
}
