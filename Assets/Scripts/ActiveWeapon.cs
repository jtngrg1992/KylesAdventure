using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActiveWeapon : MonoBehaviour
{
    public Transform firingTarget;
    public Transform weaponHolder;
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

    public void Equip(Weapon newWeapon)
    {
        if (this.weapon)
        {
            Destroy(this.weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = firingTarget;
        weapon.transform.parent = weaponHolder;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
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
