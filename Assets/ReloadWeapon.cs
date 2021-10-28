using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    ActiveWeapon activeWeapon;


    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void OnEnable()
    {
        MInputManager.reloadTriggered += HandleReload;
    }

    private void OnDisable()
    {
        MInputManager.reloadTriggered -= HandleReload;
    }

    void Start()
    {

    }

    private void HandleReload()
    {
        var currentWeapon = activeWeapon.GetActiveWeapon();
        Debug.Log(currentWeapon.weaponName);
    }
}
