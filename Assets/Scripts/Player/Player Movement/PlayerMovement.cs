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

    public AudioSource Src;
    public AudioClip WalkingSFX, JumpingSFX, RunningSFX;


    private void WalkingSFXClip()
    {
        Src.clip = WalkingSFX;
        Src.Play();
    }

    private void JumpingSFXClip()
    {
       
        Src.clip = JumpingSFX;
        Src.PlayDelayed(0.2f);
    }
    private void RunningSFXClip()
    {
        
        Src.clip = RunningSFX;
        Src.Play();
    }


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
        controller.Move(speed * Time.deltaTime * transform.TransformDirection(moveDir));

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        if(!isGrounded && playerVelocity.y >0)
        {
            JumpingSFXClip();
        }
        controller.Move(playerVelocity * Time.deltaTime);
        // WalkingSFXClip();
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
            Src.clip = RunningSFX;
            Src.Play();
        }
        else
        {
            Src.clip = RunningSFX;
            Src.Stop();
            WalkingSFXClip();
            speed = 5;
        }
    }
    
}
