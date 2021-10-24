using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private ParticleSystem impactPrefab;
    [SerializeField] private TrailRenderer tracerPrefab;
    [SerializeField] private float fireRate = 25.0f;
    [SerializeField] private float bulletSpeed = 1000.0f;
    [SerializeField] private float bulletDrop = 0.0f;

    public Transform raycastDestination;
    public AnimationClip weaponAnimation;
    public string weaponName;
    public ActiveWeapon.WeaponType weaponType;

    private Ray ray;
    private float accumulatedTime;
    private RaycastHit hitInfo;
    private List<Bullet> bullets = new List<Bullet>();
    private float maxBulletLife = 3.0f;

    public void StartFiring()
    {
        accumulatedTime = 0.0f;
        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1 / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxBulletLife);
    }

    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetBulletPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetBulletPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float rayDistance = direction.magnitude;
        ray.direction = direction;
        ray.origin = start;


        if (Physics.Raycast(ray, out hitInfo, rayDistance))
        {
            ParticleSystem impactParticles = Instantiate(impactPrefab, hitInfo.point, Quaternion.identity);
            impactParticles.transform.forward = hitInfo.normal;
            impactParticles.Emit(1);
            bullet.tracer.transform.position = hitInfo.point;
            Destroy(impactParticles.gameObject, 0.5f);
            bullet.time = maxBulletLife;
        }
        else
        {
            if (bullet != null && bullet.tracer != null)
            {
                bullet.tracer.transform.position = end;
            }

        }
    }

    private void FireBullet()
    {
        muzzleFlash.Emit(1);
        Vector3 fireDirection = raycastDestination.position - raycastOrigin.position;
        Vector3 bulletVelocity = fireDirection.normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, bulletVelocity);
        bullets.Add(bullet);
    }

    private Vector3 GetBulletPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        // s = ut+1/2at^2

        Vector3 bulletPosition = bullet.initialPosition + ((bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time));
        return bulletPosition;
    }


    private Bullet CreateBullet(Vector3 position, Vector3 initialVelocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = initialVelocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerPrefab, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

}