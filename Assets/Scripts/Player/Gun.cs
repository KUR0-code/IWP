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
    bool isMelee = true;
    public bool hasMelee = false;

    [SerializeField]
    int maxAmmo;
    [SerializeField]
    int currentAmmo;

    [SerializeField]
    float reloadTime;
    WaitForSeconds reloadWait;
   
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private ParticleSystem ShootingSystem;


    void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        currentAmmo = maxAmmo;
        reloadWait = new WaitForSeconds(reloadTime);
        cam = Camera.main;
    }

    public void shoot()
    {
        // the weapon the player is holding is a gun
        if(!isMelee)
        {
            currentAmmo--;
            ShootingSystem.Play();

            if (Physics.Raycast(BulletSpawnPoint.position, cam.transform.forward, out RaycastHit hit))
            {
                TrailRenderer trail = Instantiate(trailRenderer, BulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                if (hit.collider.GetComponent<Damageable>() != null)
                {
                    if (hit.collider.tag == "Dragon")
                    {
                        hit.collider.GetComponent<Damageable>().takeDamage(damage, hit.point, hit.normal);
                    }
                }

            }
            StartCoroutine(StopShooting(0.1f));
        }
        else // weapon is a melee
        {
            hasMelee = true;
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
    IEnumerator Reload()
    {
        if(currentAmmo == maxAmmo)
        {
            yield return null;
        }
        print("reloading");
        yield return reloadWait;
        currentAmmo = maxAmmo;
        print("Finish reloading");
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
    }
}
