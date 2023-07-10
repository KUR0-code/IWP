using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    Camera cam;

    [SerializeField]
    float damage = 10f;

    [SerializeField]
    float fireRate = 5f;
    WaitForSeconds rapidFireWait;

    [SerializeField]
    bool rapidFire = false;

    [SerializeField]
    int maxAmmo;
    public int currentAmmo;
  
    public int totalAmmo;

    [SerializeField]
    float reloadTime;
    WaitForSeconds reloadWait;
   
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private ParticleSystem ShootingSystem;

    public Light muzzleFlash;

    GameObject player;
    void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        currentAmmo = maxAmmo;
        reloadWait = new WaitForSeconds(reloadTime);
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        muzzleFlash.enabled = false;
    }

    public void shoot()
    {
        // the weapon the player is holding is a gun
        currentAmmo--;
        ShootingSystem.Play();
        muzzleFlash.enabled = true;

        if (Physics.Raycast(BulletSpawnPoint.position, cam.transform.forward, out RaycastHit hit))
        {
            TrailRenderer trail = Instantiate(trailRenderer, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            Damageable hitPtr = hit.collider.GetComponent<Damageable>();
            if (hitPtr != null)
            {
                if (hitPtr.CompareTag("Dragon"))
                {
                    hitPtr.takeDamage(damage, hit.point, hit.normal);
                    if (hitPtr.CurrentHp <= 0)
                    {
                        player.GetComponent<LevelSystem>().GainExpRate(10);
                    }
                }

                if (hitPtr.CompareTag("Boss"))
                {
                    hitPtr.takeDamage(damage, hit.point, hit.normal);
                    if (hitPtr.CurrentHp <= 0)
                    {
                        player.GetComponent<LevelSystem>().GainExpRate(50);
                    }
                }

                if (hitPtr.CompareTag("Environment"))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    hitPtr.takeDamage(0, hit.point, hit.normal);
                }

                if (hitPtr.CompareTag("Boxes"))
                {
                    hitPtr.takeDamage(0, hit.point, hit.normal);
                }
            }
            StartCoroutine(StopShooting(0.1f));
        }
    }
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }
    public IEnumerator RapidFire()
    {
        if (CanShoot())
        {
            shoot();
            if(rapidFire)
            {
                while(CanShoot())
                {
                    yield return rapidFireWait;
                    shoot();
                }
                StartCoroutine(Reload());
            }
        }
        else
        {
            StartCoroutine(Reload());
        }

    }
    public IEnumerator Reload()
    {
        if (currentAmmo != maxAmmo && totalAmmo >= 0)
        {
            int AmmoBuffer;
           
            print("reloading");
            yield return reloadWait;
            AmmoBuffer = maxAmmo - currentAmmo;
            if (AmmoBuffer > 0 && AmmoBuffer <= totalAmmo)
            {
                currentAmmo += AmmoBuffer;
                totalAmmo -= AmmoBuffer;
            }
           
            print("Finish reloading");
        }
    }

    bool CanShoot()
    {
        if(currentAmmo > 0)
        {
            return true;
        }
        else
        {
            return false;
        } 
    }

    IEnumerator StopShooting(float time)
    {
        yield return new WaitForSeconds(time);
        ShootingSystem.Stop();
        muzzleFlash.enabled = false;
    }
}
