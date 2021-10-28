using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Recoil : MonoBehaviour
{
    public float duration = 0.1f;
    public Vector2[] recoilPattern;

    [HideInInspector] public Cinemachine.CinemachineVirtualCamera aimingVirtualCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;

    CinemachinePOV pOV;
    float time = 0.0f;
    float verticalRecoil = 10;
    float horizontalRecoil;
    int index;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }
    public void ResetRecoil()
    {
        pOV = aimingVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        index = 0;
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;
        index = (index + 1) % recoilPattern.Length;
        rigController.Play("recoil_" + weaponName, 1, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            pOV.m_VerticalAxis.Value -= (verticalRecoil / 100 * Time.deltaTime) / duration;
            pOV.m_HorizontalAxis.Value -= (horizontalRecoil / 100 * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
