using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    public Camera cam;
    private InputManager inputManager;

    [SerializeField]
    float range = 50f;
    [SerializeField]
    float damage = 10f;

    [SerializeField]
    float fireRate = 5f;
    WaitForSeconds rapidFireWait;

    [SerializeField]
    bool rapidFire = false;

    [SerializeField]
    int maxAmmo;
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
        inputManager = GetComponent<InputManager>();
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        currentAmmo = maxAmmo;
        reloadWait = new WaitForSeconds(reloadTime);
    }

    public void shoot()
    {
        currentAmmo--;
        ShootingSystem.Play();

        //Vector3 direction = GetDirection();

        if (Physics.Raycast(BulletSpawnPoint.position, cam.transform.forward, out RaycastHit hit, (int) range))
        {
            TrailRenderer trail = Instantiate(trailRenderer, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            if (hit.collider.GetComponent<Damageable>() != null)
            { 
                if(hit.collider.tag == "Dragon")
                {
                    hit.collider.GetComponent<Damageable>().takeDamage(damage, hit.point, hit.normal);
                }
            }
          
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
            ShootingSystem.Stop();
            return false;
        } 
    }
}
