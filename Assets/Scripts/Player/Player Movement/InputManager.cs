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


    Coroutine fireCoroutine;
    [SerializeField]
    private ParticleSystem ShootingSystem;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

                                // ctx is call back context works like a pointer.
        walking.Jump.performed += ctx => movement.Jump();
        walking.Crouch.performed += ctx => movement.Crouch();
        walking.Sprint.performed += ctx => movement.Sprint();
        walking.Shoot.started += _ => startFiring();
        walking.Shoot.canceled += _ => stopFiring();
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
        fireCoroutine = StartCoroutine(gun.RapidFire());
    }

    void stopFiring()
    {
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            ShootingSystem.Stop();
        }
    }
}
