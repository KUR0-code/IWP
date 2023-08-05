using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int AddRifleAmmo;
    public int AddPistolAmmo;
    public bool collectedRifle;
    public bool collectedPistol;

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
                            inventory.AddItem(item.item, Hit.collider.GetComponent<Medkit>().count);
                    }

                    if (Hit.collider.CompareTag("Ammo_Boxes"))
                    {
                        Hit.collider.GetComponent<Ammo_Chest>().BoxInteracted = true;
                        if (!Hit.collider.GetComponent<Ammo_Chest>().HasOpened)
                        { 
                            inventory.AddItem(item.item, Hit.collider.GetComponent<Ammo_Chest>().count);
                            WeaponHolder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().totalAmmo += Hit.collider.GetComponent<Ammo_Chest>().count;
                        }
                            
                    }

                    if (Hit.collider.CompareTag("Rifle"))
                    {
                        inventory.AddItem(item.item, AddRifleAmmo);
                        Destroy(Hit.collider.GetComponent<Item>().gameObject);
                        collectedRifle = true;

                    }  
                    if (Hit.collider.CompareTag("Pistol"))
                    {
                        inventory.AddItem(item.item, AddPistolAmmo);
                        Destroy(Hit.collider.GetComponent<Item>().gameObject);
                        collectedPistol = true;
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
