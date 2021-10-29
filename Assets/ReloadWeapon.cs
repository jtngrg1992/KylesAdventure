using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public Transform leftHand;
    public WeaponAnimationEvents weaponAnimationEvents;

    ActiveWeapon activeWeapon;
    int reloadHash;
    GameObject magazineInHand;


    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadHash = Animator.StringToHash("reload");
        weaponAnimationEvents.reloadAnimationEvent.AddListener(HandleReloadAnimationEvent);
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
        if (currentWeapon)
        {
            rigController.SetTrigger(reloadHash);
        }
    }

    private void HandleReloadAnimationEvent(ReloadEvent e)
    {
        var equippedWeapon = activeWeapon.GetActiveWeapon();
        if (equippedWeapon && equippedWeapon.ammoClip)
        {
            switch (e)
            {
                case ReloadEvent.DetachMagazine:
                    DetachMagazine(equippedWeapon.ammoClip);
                    break;
                case ReloadEvent.ThrowMagazine:
                    DropMagazine();
                    break;
                case ReloadEvent.ReplenishMagazine:
                    ReplinishMagazine();
                    break;
                case ReloadEvent.AttachNewMagazine:
                    AttachMagazine(equippedWeapon.ammoClip);
                    break;
            }
        }
    }

    private void DetachMagazine(GameObject clip)
    {
        magazineInHand = Instantiate(clip, leftHand, true);
        clip.SetActive(false);
    }

    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineInHand, magazineInHand.transform.position, magazineInHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineInHand.SetActive(false);
    }

    private void ReplinishMagazine()
    {
        magazineInHand.SetActive(true);
    }

    private void AttachMagazine(GameObject clip)
    {
        clip.SetActive(true);
        Destroy(magazineInHand);
    }
}
