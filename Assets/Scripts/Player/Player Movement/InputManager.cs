using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.WalkingActions walking;

    private PlayerMovement movement;
    private PlayerLook look;
    [SerializeField]
    Gun gun;
    [SerializeField]
    private GameObject weaponholder;

    Coroutine fireCoroutine;

    // public GameObject SettingUI;

    public GameObject inventoryUI;
    
    bool CursorToggle = false;
    bool toggle = false;
    bool FlashToggle = false;
    public GameObject flashLight;

    public GameObject player;

    public HealingPotion healingPotion;

    public InventoryObject inventory;
    public displayInventory DisplayInventory;


    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

        
        // SettingUI.SetActive(false);

        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        inventoryUI.SetActive(false);

  

        // ctx is call back context works like a pointer.
        walking.Jump.performed += ctx => movement.Jump();
        walking.Crouch.performed += ctx => movement.Crouch();
        walking.Sprint.performed += ctx => movement.Sprint();
        walking.Shoot.started += _ => startFiring();
        walking.Shoot.canceled += _ => stopFiring();
        walking.Settings.performed += _ => UnlockCursor();
        walking.Inventory.performed += _ => InventoryUI();
        walking.EatFood.performed += _ => HealPlayer();
        walking.Reload.performed += _ => StartCoroutine(weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().Reload());
        walking.FlashLight.performed += _ => ToggleFlashLight();

        DisplayInventory.GetComponent<displayInventory>().CreateDisplay();
        inventory.AddItem(
          weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().GetComponent<Item>().item,
          weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().maxAmmo +
          weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().totalAmmo);

        inventory.AddItem(
         weaponholder.GetComponent<WeaponSwitching>().GetNextWeapon().GetComponent<Gun>().GetComponent<Item>().item,
         weaponholder.GetComponent<WeaponSwitching>().GetNextWeapon().GetComponent<Gun>().maxAmmo +
         weaponholder.GetComponent<WeaponSwitching>().GetNextWeapon().GetComponent<Gun>().totalAmmo);
    }
    
    // Update is called once per frame
    void FixedUpdate()  
    {
        // move using the value of movement actions
        movement.ProcessMoving(walking.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        // move using the value of movement actions
        look.ProcessLook(walking.Look.ReadValue<Vector2>()); 
    }

    private void HealPlayer()
    {
        if(inventory.RemoveHeal())
        {
            player.GetComponent<PlayerHealth>().RestoreHealth(healingPotion.restoreHealthValue);
        }
    }  

    private void InventoryUI()
    {
        toggle = !toggle;
        if(toggle)
            inventoryUI.SetActive(true);
        else
            inventoryUI.SetActive(false);

    }

    private void UnlockCursor()
    {
        CursorToggle = !CursorToggle;
        if (CursorToggle)
        {
            // SettingUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            // SettingUI.SetActive(false);
            Cursor.visible = false;
        }
    }

    private void OnEnable()
    {
        walking.Enable();
    }

    private void OnDisable()
    {
        walking.Disable();
    }

    void startFiring()
    {
        gun = weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>();
        fireCoroutine = StartCoroutine(gun.RapidFire());
    }

    void stopFiring()
    {
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    private void ToggleFlashLight()
    {
        FlashToggle = !FlashToggle;
        if (FlashToggle)
        {
            flashLight.GetComponent<Light>().enabled = false;
            // Debug.Log("here");
        }
        else
            flashLight.GetComponent<Light>().enabled = true;

    }
}
