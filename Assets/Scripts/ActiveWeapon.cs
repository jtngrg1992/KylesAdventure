using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
using Cinemachine;
public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        Primary,
        Secondary
    }

    public Transform firingTarget;
    public Transform leftGrip;
    public Transform rightGrip;
    public Animator rigController;
    public Transform primaryWeaponHolder;
    public Transform secondaryWeaponHolder;
    public Cinemachine.CinemachineVirtualCamera aimCamera;

    Weapon[] weaponsOnPlayer = new Weapon[2];

    int activeWeaponIndex;

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

    public Weapon GetActiveWeapon()
    {
        return weaponsOnPlayer[activeWeaponIndex];
    }

    Weapon GetEquippedWeapon(int index)
    {
        if (index < 0 || index > weaponsOnPlayer.Length)
        {
            return null;
        }
        return weaponsOnPlayer[index];
    }

    int? GetWeaponSlotIndex(Weapon weapon)
    {
        int? weaponIndex = null;

        for (int i = 0; i < weaponsOnPlayer.Length; i++)
        {
            Weapon w = weaponsOnPlayer[i];
            if (w)
            {
                if (w.name == weapon.name && w.weaponType == weapon.weaponType)
                {
                    weaponIndex = i;
                    break;
                }
            }
        }
        return weaponIndex;
    }

    int? GetFirstAvailableSlotForWeapon()
    {
        int? availableSlotIndex = null;

        for (int i = 0; i < weaponsOnPlayer.Length; i++)
        {
            if (weaponsOnPlayer[i] == null)
            {
                availableSlotIndex = i;
                break;
            }
        }
        return availableSlotIndex;
    }

    public Weapon GetWeaponOfType(WeaponType wtype)
    {
        for (int i = 0; i < weaponsOnPlayer.Length; i++)
        {
            var weapon = weaponsOnPlayer[i];
            if (weapon && weapon.weaponType == wtype)
            {
                return weapon;
            }
        }
        return null;
    }

    public void Equip(Weapon newWeapon)
    {
        int? slotIndex = GetWeaponSlotIndex(newWeapon);

        if (slotIndex != null)
        {
            // weapon can't be equipped
            Destroy(newWeapon.gameObject);
            return;
        }

        int? availableSlot = GetFirstAvailableSlotForWeapon();

        if (availableSlot != null)
        {
            int index = (int)availableSlot;

            activeWeaponIndex = index;

            newWeapon.raycastDestination = firingTarget;

            switch (newWeapon.weaponType)
            {
                case WeaponType.Primary:
                    newWeapon.transform.parent = primaryWeaponHolder.transform;
                    break;
                case WeaponType.Secondary:
                    newWeapon.transform.parent = secondaryWeaponHolder.transform;
                    break;
            }
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
            newWeapon.recoil.aimingVirtualCamera = aimCamera;
            newWeapon.recoil.rigController = rigController;
            weaponsOnPlayer[index] = newWeapon;
            rigController.Play($"equip_{newWeapon.weaponName.ToLower()}");
        }
    }

    private void OnEnable()
    {
        MInputManager.holsterRequested += HandleHolster;
        MInputManager.switchPrimaryRequested += HandleSwitchPrimary;
        MInputManager.switchSecondaryRequested += HandleSwitchSecondary;
    }

    private void OnDisable()
    {
        MInputManager.holsterRequested -= HandleHolster;
        MInputManager.switchPrimaryRequested -= HandleSwitchPrimary;
        MInputManager.switchSecondaryRequested -= HandleSwitchSecondary;
    }

    public void StartFiring()
    {
        Weapon weapon = GetEquippedWeapon(activeWeaponIndex);
        if (weapon)
        {
            weapon.StartFiring();
        }
    }

    public void UpdateFiring(float deltaTime)
    {
        Weapon weapon = GetEquippedWeapon(activeWeaponIndex);
        if (weapon)
        {
            weapon.UpdateFiring(deltaTime);
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        Weapon weapon = GetEquippedWeapon(activeWeaponIndex);
        if (weapon)
        {
            weapon.UpdateBullets(deltaTime);
        }
    }

    void HandleHolster()
    {
        StartCoroutine(HolsterWeapon(activeWeaponIndex));
    }

    void HandleSwitchPrimary()
    {
        var currentWeapon = GetEquippedWeapon(activeWeaponIndex);
        if (currentWeapon)
        {
            if (currentWeapon.weaponType == WeaponType.Secondary)
            {
                // need to switch to primary
                var primaryWeapon = GetWeaponOfType(WeaponType.Primary);
                if (primaryWeapon)
                {
                    StartCoroutine(HolsterWeapon(activeWeaponIndex));
                    string animationName = $"equip_{primaryWeapon.weaponName.ToLower()}";
                    rigController.Play(animationName);
                    activeWeaponIndex = (int)GetWeaponSlotIndex(primaryWeapon);
                }
            }
            else
            {
                // player only has secondary weapon
                string animationName = $"equip_{currentWeapon.weaponName.ToLower()}";
                rigController.Play(animationName);
            }
        }
        rigController.SetBool(holsterHash, false);
    }

    void HandleSwitchSecondary()
    {
        var currentWeapon = GetEquippedWeapon(activeWeaponIndex);
        if (currentWeapon)
        {
            if (currentWeapon.weaponType == WeaponType.Primary)
            {
                // need to switch to primary
                var secondaryWeapon = GetWeaponOfType(WeaponType.Secondary);
                if (secondaryWeapon)
                {
                    StartCoroutine(HolsterWeapon(activeWeaponIndex));
                    string animationName = $"equip_{secondaryWeapon.weaponName.ToLower()}";
                    rigController.Play(animationName);
                    activeWeaponIndex = (int)GetWeaponSlotIndex(secondaryWeapon);
                }
            }
            else
            {
                // player only has secondary weapon
                string animationName = $"equip_{currentWeapon.weaponName.ToLower()}";
                rigController.Play(animationName);
            }
        }
        rigController.SetBool(holsterHash, false);
    }

    IEnumerator HolsterWeapon(int index)
    {
        var weapon = GetEquippedWeapon(index);

        if (weapon)
        {
            rigController.SetBool(holsterHash, true);
            yield return new WaitUntil(() => rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
}
