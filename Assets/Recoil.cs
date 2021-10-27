using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Recoil : MonoBehaviour
{
    public float verticalRecoil = 10;
    public float horizontalRecoil;
    public float duration = 0.1f;

    [HideInInspector] public Cinemachine.CinemachineVirtualCamera aimingVirtualCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;


    CinemachinePOV pOV;
    float time = 0.0f;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }
    public void ResetRecoil()
    {
        pOV = aimingVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void GenerateRecoil()
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            pOV.m_VerticalAxis.Value -= (verticalRecoil / 1000 * Time.deltaTime) / duration;
            pOV.m_HorizontalAxis.Value -= (horizontalRecoil / 1000 * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
