using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    Camera cam;

    public GameObject instantiateGunRifleAmmo;
    public GameObject instantiateGunPistolAmmo;

    [SerializeField]
    float damage = 10f;

    [SerializeField]
    float fireRate = 5f;
    WaitForSeconds rapidFireWait;

    [SerializeField]
    bool rapidFire = false;

    public int maxAmmo;
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

    public float recoilPercent = 0.3f;
    public float recoverPercent = 0.7f;
    public float recoilUp = 0.02f;
    public float recoilBack = 0.08f;

    private Vector3 OriginalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private bool recoiling;
    public bool recovering;

    private float recoilLength;
    private float recoverLength;

    public AudioSource Src;
    public AudioClip SFX1, SFX2;

    int ammoDrop;
    public InventoryObject inventory;

    public PlayerInteract playerInteract;
    int AmmoBuffer;

    bool HasAlreadyReloaded = false;


    private void PlaySFX1()
    {
        // shoot
        Src.clip = SFX1;
        Src.Play();
    }
    private void PlaySFX2()
    {
        // reload
        Src.clip = SFX2;
        Src.Play();
    }

    void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        currentAmmo = maxAmmo;
        reloadWait = new WaitForSeconds(reloadTime);
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        muzzleFlash.enabled = false;

        OriginalPosition = transform.localPosition;


        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    private void Update()
    {
        if (recoiling)
        {
            Recoil();
        }

        if (recovering)
        {
            Recovering();
        }

        if (playerInteract.collectedRifle && rapidFire) //Ar + Ar
        {
            totalAmmo += playerInteract.AddRifleAmmo;
            playerInteract.collectedRifle = false;
        }
        else if (playerInteract.collectedRifle && !rapidFire) // Ar + pistol
        {
            Debug.Log("1");
            GetComponent<WeaponSwitching>().GetPreviousWeapon().GetComponent<Gun>().totalAmmo += playerInteract.AddRifleAmmo;
            playerInteract.collectedRifle = false;
        }
        else if (playerInteract.collectedPistol && rapidFire) // Pistol + Ar
        {
            Debug.Log("2");
            GetComponent<WeaponSwitching>().GetNextWeapon().GetComponent<Gun>().totalAmmo += playerInteract.AddPistolAmmo;
            //totalAmmo += playerInteract.AddPistolAmmo;
            playerInteract.collectedPistol = false;
        }
        else if (playerInteract.collectedPistol && !rapidFire) //Pistol + Pistol
        {
            Debug.Log("3");
            totalAmmo += playerInteract.AddPistolAmmo;
            //totalAmmo += playerInteract.AddPistolAmmo;
            playerInteract.collectedPistol = false;
        }
        
    }

    public void shoot()
    {
        // the weapon the player is holding is a gun
        PlaySFX1();
        currentAmmo--;
        if(rapidFire)
        {
            inventory.RemoveBulletAr();
        }
        else
        {
            inventory.RemoveBulletPistol();
        }

        ShootingSystem.Play();
        muzzleFlash.enabled = true;
        recoiling = true;
        recovering = false;
        CameraShake.Invoke();
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
                        AmmoDropRange(hitPtr.gameObject.transform.position);
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
                    // Debug.Log(hit.collider.gameObject.name);
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

    private void AmmoDropRange(Vector3 HitPosition)
    {
        ammoDrop = Random.Range(1, 10);
        switch (ammoDrop)
        {
            case 1:
                Instantiate(instantiateGunRifleAmmo, HitPosition, Quaternion.identity);
                Debug.Log("Here");
                break;
            case 2:
                Instantiate(instantiateGunPistolAmmo, HitPosition, Quaternion.identity);
                break;
            case 3:
                break;

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
            if (rapidFire)
            {
                while (CanShoot())
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
        if(HasAlreadyReloaded == false)
        {
            PlaySFX2();
            muzzleFlash.enabled = false;
            AmmoBuffer = 0;

            AmmoBuffer = maxAmmo - currentAmmo;

            if (AmmoBuffer > 0 && totalAmmo >= AmmoBuffer)
            {
                HasAlreadyReloaded = true;
                print("reloading");
                yield return reloadWait;
                currentAmmo += AmmoBuffer;
                totalAmmo -= AmmoBuffer;
                print("Finish reloading");
                HasAlreadyReloaded = false;
            }
            else if (AmmoBuffer > 0 && totalAmmo <= AmmoBuffer)
            {
                HasAlreadyReloaded = true;
                print("reloading");
                yield return reloadWait;
                currentAmmo += totalAmmo;
                totalAmmo = 0;
                print("Finish reloading");
                HasAlreadyReloaded = false;
            }
        }
        Debug.Log(HasAlreadyReloaded);
    }

    bool CanShoot()
    {
        if (currentAmmo > 0)
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

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(OriginalPosition.x, OriginalPosition.y + recoilUp, OriginalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recovering()
    {
        Vector3 finalPosition = OriginalPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
