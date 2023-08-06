using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    
    private Vector3 playerVelocity;

    public Vector3 moveDir = Vector3.zero;

    public float speed = 5.0f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;
    private float crouchTimer;

    public AudioSource WalkSrc;
    public AudioSource RunSrc;
    public AudioSource JumpSrc;
    public AudioClip WalkingSFX, JumpingSFX, RunningSFX;
    bool soundPlayed = false;
    bool wasWalking;
    bool wasRunning;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        WalkSrc.clip = WalkingSFX;
        JumpSrc.clip = JumpingSFX;
        RunSrc.clip = RunningSFX;
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
                speed = 2f;
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 1.6f, temp);
                speed = 5f;
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
        moveDir.x = Input.x;
        moveDir.z = Input.y;
        controller.Move(speed * Time.deltaTime * transform.TransformDirection(moveDir));
        playerVelocity.y += gravity * Time.deltaTime;
        if((Input.x > 0 || Input.y > 0) && isGrounded && speed == 5f) // walking
        {
            if(!soundPlayed)
            {
                WalkSrc.Play();
                soundPlayed = true;
            }
        }
        if((Input.x > 0 || Input.y > 0) && isGrounded && speed == 8f) // walking
        {
            if (!soundPlayed)
            {
                RunSrc.Play();
                soundPlayed = true;
            }
        }
       
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        if(!isGrounded && playerVelocity.y >0)
        {
            JumpSrc.PlayDelayed(0.1f);
            if(wasWalking)
            {
                WalkSrc.Play();
            }
            else if (wasRunning)
            {
                RunSrc.Play();
            }
        }
        if(Input.x == 0 && Input.y == 0 && isGrounded)
        {
            WalkSrc.Stop();
            RunSrc.Stop();
            soundPlayed = false;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        wasWalking = false;
        wasRunning = false;
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);

            if(WalkSrc.isPlaying)
            {
                wasWalking = true;
            }
            else if(RunSrc.isPlaying)
            {
                wasRunning = true;
            }
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
            speed = 8f;
            RunSrc.Play();
            WalkSrc.Stop();
        }
        else
        {
            RunSrc.Stop();
            WalkSrc.Play();
            speed = 5f;
        }
    }
    
}
