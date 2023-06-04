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
    [SerializeField]
    private ParticleSystem ShootingSystem;

    public GameObject SettingUI;

    public GameObject inventoryUI;
    bool CursorToggle = false;
    bool toggle = false;

    public GameObject player;

    public InventoryObject inventoryObject;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

        SettingUI = GameObject.FindGameObjectWithTag("Settings");
        SettingUI.gameObject.SetActive(false);

        inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
        inventoryUI.gameObject.SetActive(false);

        // ctx is call back context works like a pointer.
        walking.Jump.performed += ctx => movement.Jump();
        walking.Crouch.performed += ctx => movement.Crouch();
        walking.Sprint.performed += ctx => movement.Sprint();
        walking.Shoot.started += _ => startFiring();
        walking.Shoot.canceled += _ => stopFiring();
        walking.Settings.performed += _ => UnlockCursor();
        walking.Inventory.performed += _ => InventoryUI();
        walking.EatFood.performed += _ => HealPlayer();
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
        inventoryObject.RemoveHeal();
    }  

    private void InventoryUI()
    {
        toggle = !toggle;
        if(toggle)
            inventoryUI.gameObject.SetActive(true);
        else
            inventoryUI.gameObject.SetActive(false);

    }

    private void UnlockCursor()
    {
        CursorToggle = !CursorToggle;
        if (CursorToggle)
        {
            SettingUI.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            SettingUI.gameObject.SetActive(false);
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
            //ShootingSystem.Stop();
        }
    }


}
