using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
public class ActiveWeapon : MonoBehaviour
{
    public Transform firingTarget;
    public Transform weaponHolder;
    public Transform weaponPoseRelaxed;
    public Transform weaponPoseAiming;
    public Transform leftGrip;
    public Transform rightGrip;
    public Rig handIKRig;

    Weapon weapon;
    Animator animator;
    AnimatorOverrideController animationOverride;

    void Awake()
    {
        Weapon existingWeapon = GetComponentInChildren<Weapon>();
        animator = GetComponent<Animator>();
        animationOverride = animator.runtimeAnimatorController as AnimatorOverrideController;
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
            animator.SetLayerWeight(1, 0.0f);
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
        animator.SetLayerWeight(1, 1.0f);

        Invoke(nameof(SetAnimationOverrideDelayed), 0.001f);
    }

    void SetAnimationOverrideDelayed()
    {
        animationOverride["weapon_anim_empty"] = weapon.weaponAnimationRelaxed;
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

    void GrabClip(Transform targetWeaponPose)
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponHolder.gameObject, false);
        recorder.BindComponentsOfType<Transform>(targetWeaponPose.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimationRelaxed);
    }

    [ContextMenu("Save Relaxed Weapon Pose")]
    void SaveRelaxedWeaponPose()
    {
        GrabClip(weaponPoseRelaxed);
    }

    [ContextMenu("Saved Aiming Weapon Pose")]
    void SaveAimingWeaponPose()
    {
        GrabClip(weaponPoseAiming);
    }

}
