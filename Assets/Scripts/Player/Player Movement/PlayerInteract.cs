using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    public InventoryObject inventory;
    public GameObject WeaponHolder;

    public Slider Scavange_Bar;

    public int AddRifleAmmo;
    public int AddPistolAmmo;
    public bool collectedRifle;
    public bool collectedPistol;

    public bool Scavange_Rifle_Ammo;
    public bool Scavange_Pistol_Ammo;
    public int RandomArNum;
    public int RandomPistolNum;

    public TextMeshProUGUI textDisplay;
    float disappearTime;
    bool DoneCollecting;

    int tempHeal;
    int tempAmmobox;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();

        AddRifleAmmo = 15;
        AddPistolAmmo = 5;
        collectedRifle = false;
        collectedPistol = false;
        Scavange_Rifle_Ammo = false;
        Scavange_Pistol_Ammo = false;
        Scavange_Bar.gameObject.SetActive(false);
        DoneCollecting = false;
        tempHeal = 0;
        tempAmmobox = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty); 
        // raycasting
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        // variable to store the collision
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, distance, mask))
        {
            if (Hit.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = Hit.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.PromptMessage);
                if (inputManager.walking.Interact.triggered)
                {
                    interactable.BaseInteract();
                    var item = Hit.collider.GetComponent<Item>();

                    if (Hit.collider.CompareTag("Boxes"))
                    {
                        Hit.collider.GetComponent<Medkit>().BoxInteracted = true;
                        if(!Hit.collider.GetComponent<Medkit>().HasOpened)
                        {
                            DoneCollecting = true;
                            tempHeal = Hit.collider.GetComponent<Medkit>().count;
                            inventory.AddItem(item.item, Hit.collider.GetComponent<Medkit>().count);
                            Collected_Heal_Text();
                        }
                    }

                    if (Hit.collider.CompareTag("Ammo_Boxes"))
                    {
                        Hit.collider.GetComponent<Ammo_Chest>().BoxInteracted = true;
                        if (!Hit.collider.GetComponent<Ammo_Chest>().HasOpened)
                        {
                            tempAmmobox = Hit.collider.GetComponent<Ammo_Chest>().count;
                            inventory.AddItem(item.item, Hit.collider.GetComponent<Ammo_Chest>().count);
                            WeaponHolder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().totalAmmo += Hit.collider.GetComponent<Ammo_Chest>().count;
                            DoneCollecting = true;
                            Collected_RifleBox_Ammo();
                        }
                    }

                    if (Hit.collider.CompareTag("Rifle"))
                    {
                        inventory.AddItem(item.item, AddRifleAmmo);
                        Destroy(Hit.collider.GetComponent<Item>().gameObject);
                        collectedRifle = true;
                        DoneCollecting = true;
                        Collected_Rifle_Ammo();
                    }

                    if (Hit.collider.CompareTag("Ar_Scavange"))
                    {
                        Debug.Log("hit");
                        RandomArNum = Random.Range(0, 10);
                        inventory.AddItem(item.item, RandomArNum);
                        Scavange_Rifle_Ammo = true;
                        Scavange_Bar.gameObject.SetActive(true);
                        Hit.collider.gameObject.GetComponent<Collider>().enabled = false;
                    } 
                    
                    if (Hit.collider.CompareTag("Pistol_Scavange"))
                    {
                        Debug.Log("hit2");

                        RandomPistolNum = Random.Range(0, 5);
                        inventory.AddItem(item.item, RandomPistolNum);
                        Scavange_Pistol_Ammo = true;
                        Scavange_Bar.gameObject.SetActive(true);
                        Hit.collider.gameObject.GetComponent<Collider>().enabled = false;
                    }

                    if (Hit.collider.CompareTag("Pistol"))
                    {
                        inventory.AddItem(item.item, AddPistolAmmo);
                        Destroy(Hit.collider.GetComponent<Item>().gameObject);
                        collectedPistol = true;
                        DoneCollecting = true;
                        Collected_Pistol_Ammo();
                    }
                }
            }
        }
    }
    void Collected_Heal_Text()
    {
        if (DoneCollecting)
        {
            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + tempHeal.ToString() + " Medical Potions";
                DoneCollecting = false;
            }
            FadeOut();
        }
    }

    void Collected_Pistol_Ammo()
    {
        if (DoneCollecting)
        {
            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + AddPistolAmmo.ToString() + " Pistol ammo";
                DoneCollecting = false;
            }
            FadeOut();
        }
    }
    void Collected_Rifle_Ammo()
    {
        if (DoneCollecting)
        {
            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + AddRifleAmmo.ToString() + " Rifle ammo";
                DoneCollecting = false;
            }
            FadeOut();
        }
    }
    void Collected_RifleBox_Ammo()
    {
        if (DoneCollecting)
        {
            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + tempAmmobox.ToString() + " Rifle ammo";
                DoneCollecting = false;
            }
            FadeOut();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCR());
    }


    public IEnumerator FadeOutCR()
    {
        float duration = 2.5f;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
