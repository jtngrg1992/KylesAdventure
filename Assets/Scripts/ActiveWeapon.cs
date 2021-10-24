using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        Primary,
        Secondary
    }

    public Transform firingTarget;
    public Transform weaponPoseRelaxed;
    public Transform weaponPoseAiming;
    public Transform leftGrip;
    public Transform rightGrip;
    public Animator rigController;
    public Transform primaryWeaponHolder;
    public Transform secondaryWeaponHolder;

    Weapon weapon;
    int holsterHash;

    void Awake()
    {
        Weapon existingWeapon = GetComponentInChildren<Weapon>();
        holsterHash = Animator.StringToHash("holster_weapon");

        if (existingWeapon)
        {
            Equip(existingWeapon);
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

        switch (weapon.weaponType)
        {
            case WeaponType.Primary:
                weapon.transform.parent = primaryWeaponHolder;
                break;
            case WeaponType.Secondary:
                weapon.transform.parent = secondaryWeaponHolder;
                break;
        }
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        rigController.Play($"equip_{weapon.weaponName.ToLower()}");
    }

    private void OnEnable()
    {
        MInputManager.holsterRequested += HandleHolster;
    }

    private void OnDisable()
    {
        MInputManager.holsterRequested -= HandleHolster;
    }

    public void StartFiring()
    {
        if (weapon)
        {
            weapon.StartFiring();
        }
    }

    public void UpdateFiring(float deltaTime)
    {
        if (weapon)
        {
            weapon.UpdateFiring(deltaTime);
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        if (weapon)
        {
            weapon.UpdateBullets(deltaTime);
        }
    }

    void HandleHolster()
    {
        if (weapon)
        {
            bool isHolstered = rigController.GetBool(holsterHash);
            bool target = !isHolstered;
            rigController.SetBool(holsterHash, target);
        }
    }
}
