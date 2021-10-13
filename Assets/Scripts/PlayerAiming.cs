using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    private MultiAimConstraint weaponAimConstraint;
    [SerializeField]
    private MultiPositionConstraint weaponPose;
    [SerializeField]
    private Transform rightHandIKTarget;

    [SerializeField]
    private Vector3 idleWeaponPoseOffset;
    [SerializeField]
    private Vector3 idleRightHandIKRotation;



    private bool isIdle = true;

    void Awake()
    {
        if (isIdle)
        {
            weaponAimConstraint.weight = 0;
            weaponPose.data.offset = idleWeaponPoseOffset;
            rightHandIKTarget.transform.rotation = Quaternion.Euler(idleRightHandIKRotation);
        }
    }

    private void Update()
    {

    }
}
