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


    public bool isFiring = false;

    private Ray ray;
    private RaycastHit hitInfo;

    public void StartFiring()
    {
        muzzleFlash.Play();
        isFiring = true;
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
        if (!isFiring)
        {
            return;
        }
        muzzleFlash.Stop();
        isFiring = false;
    }
}