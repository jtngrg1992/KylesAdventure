using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private Transform raycastOrigin;
    [SerializeField]
    private Transform raycastDestination;
    [SerializeField]
    private ParticleSystem impactPrefab;
    [SerializeField]
    private TrailRenderer tracerPrefab;
    [SerializeField]
    private int fireRate = 25;

    public bool isFiring = false;

    private Ray ray;
    private float accumulatedTime;
    private RaycastHit hitInfo;


    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1 / fireRate;

        while (accumulatedTime > 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }


    public void StartFiring()
    {
        accumulatedTime = 0.0f;
        FireBullet();
    }

    private void FireBullet()
    {
        muzzleFlash.Play();
        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        if (Physics.Raycast(ray, out hitInfo))
        {
            ParticleSystem impactParticles = Instantiate(impactPrefab, hitInfo.point, Quaternion.identity);
            var tracer = Instantiate(tracerPrefab, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);
            impactParticles.transform.forward = hitInfo.normal;
            impactParticles.Emit(1);
            Destroy(impactParticles.gameObject, 0.5f);
            tracer.transform.position = hitInfo.point;
        }
    }

    public void StopFiring()
    {
        muzzleFlash.Stop();
    }
}