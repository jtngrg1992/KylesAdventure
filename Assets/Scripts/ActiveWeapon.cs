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
    public Animator rigController;


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
        weapon.transform.parent = weaponHolder;
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

    void GrabClip(Transform targetWeaponPose)
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponHolder.gameObject, false);
        recorder.BindComponentsOfType<Transform>(targetWeaponPose.gameObject, false);
        recorder.BindComponentsOfType<Transform>(leftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(rightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
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
