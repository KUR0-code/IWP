using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5.0f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;
    private float crouchTimer;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float temp = crouchTimer / 1;
            temp *= temp;
            if(crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1.3f, temp);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 1.6f, temp);
            }
            if(temp > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

       
    }

    // Receive the input from the input manager script
    public void ProcessMoving(Vector2 Input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = Input.x;
        moveDir.z = Input.y;
        controller.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y <0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
   
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
        }
        else
        {
            speed = 5;
        }
    }
    
}
