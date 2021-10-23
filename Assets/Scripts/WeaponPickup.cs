using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;


    private void Awake()
    {
        if (weaponPrefab)
        {
            Weapon preview = Instantiate(weaponPrefab);
            preview.transform.parent = this.gameObject.transform;
            preview.transform.localRotation = Quaternion.identity;
            preview.transform.localPosition = Vector3.zero;
        }
    }


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
