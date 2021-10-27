using UnityEngine;
using System.Collections.Generic;
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
    public Transform leftGrip;
    public Transform rightGrip;
    public Animator rigController;
    public Transform primaryWeaponHolder;
    public Transform secondaryWeaponHolder;

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
            weaponsOnPlayer[index] = newWeapon;
            activeWeaponIndex = index;

            weaponsOnPlayer[index].raycastDestination = firingTarget;

            switch (weaponsOnPlayer[index].weaponType)
            {
                case WeaponType.Primary:
                    weaponsOnPlayer[index].transform.parent = primaryWeaponHolder;
                    break;
                case WeaponType.Secondary:
                    weaponsOnPlayer[index].transform.parent = secondaryWeaponHolder;
                    break;
            }
            weaponsOnPlayer[index].transform.localPosition = Vector3.zero;
            weaponsOnPlayer[index].transform.localRotation = Quaternion.identity;
            rigController.Play($"equip_{weaponsOnPlayer[index].weaponName.ToLower()}");
        }
    }

    private void OnEnable()
    {
        MInputManager.holsterRequested += HandleHolster;
        MInputManager.switchRequested += HandleSwitch;
    }

    private void OnDisable()
    {
        MInputManager.holsterRequested -= HandleHolster;
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
        Weapon weapon = GetEquippedWeapon(activeWeaponIndex);
        if (weapon)
        {
            bool isHolstered = rigController.GetBool(holsterHash);
            bool target = !isHolstered;
            rigController.SetBool(holsterHash, target);
        }
    }

    void HandleSwitch()
    {
        Debug.Log("yo");
    }
}
