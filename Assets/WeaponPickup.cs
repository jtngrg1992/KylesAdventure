using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon obj = other.gameObject.GetComponent<ActiveWeapon>();

        if (obj)
        {
            Weapon newWeapon = Instantiate(weaponPrefab);
            obj.Equip(newWeapon);
        }
    }

}
