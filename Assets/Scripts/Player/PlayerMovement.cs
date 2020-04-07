using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Basics")]
    public float speed = 7f;
    public float gravity = -30f;
    public float jumpHeight = 3f;
    Vector3 move; // Input
    Vector3 moveDir; // Direction for dashing and WallJumping
    Vector3 velocity; // Only for gravity/ upward force
    float x;
    float z;

    [Header("Dashing")]
    bool dash = false;
    public float dashMaximumTime = 0.4f;
    float timeSpentDashing = 0f;
    public float dashStrength = 60f;

    [Header("Wall Jumping")]
    public bool isStickingToWall;
    public bool canStickToWall = true;
    public float timeNotAbleToWallStick;
    public float maxTimeNotAbleToWallStick = 0.3f;

    void Update()
    {

        x = Input.GetAxis("Horizontal") * Time.deltaTime;
        z = Input.GetAxisRaw("Vertical") * Time.deltaTime;

        isGrounded();

        HandleInput();

        Movement();

        HandleDashing();

        RefuseWallSticking();

    }
    private void FixedUpdate()
    {
        if (!isStickingToWall)
        {
            controller.Move(new Vector3(move.normalized.x * speed * Time.deltaTime, velocity.y * Time.deltaTime, move.normalized.z * speed * Time.deltaTime));
        }
        else
        {
            return;
        }

        if (dash)
        {
            controller.Move(moveDir * Time.deltaTime);
        }      
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Singleton.Instance.RasenShurikenInstatiate();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {               
            canStickToWall = false;
            isStickingToWall = false;
            timeNotAbleToWallStick = 0f;
        }
    }

    void HandleDashing()
    {
        if(x == 0f && z >= 0f) // If the player is not pressing anything, or is only going forward, apply camera upward rotation to its dash direction
        {
            moveDir = move.normalized + Singleton.Instance.playerCamera.transform.forward * dashStrength * 1.34f;
        }
        else
        {
            moveDir = move.normalized * dashStrength;
        }


        timeSpentDashing += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocity.y = 0f;
            timeSpentDashing = 0f;
            dash = true;
        }

        if (timeSpentDashing > dashMaximumTime)
        {
            dash = false;
        }
    }

    void Jump()
    {
        if (isGrounded() && !isStickingToWall)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); 
        }
        else if (isStickingToWall) // If the player is sticking to a wall, just make it dash if it presses jump
        {
            velocity.y = 0f;
            isStickingToWall = false;
            timeSpentDashing = 0f;
            dash = true; // TODO make a better walljump
        }
    }
    // Apply basic movement
    void Movement()
    {
        move = transform.right * x + transform.forward * z;

        velocity.y += gravity * Time.deltaTime;
    }
    public bool isGrounded()
    {
        if (controller.isGrounded) // Default ground check
        {
            velocity.y = 0f;
            
            return true;
        }

        Vector3 bottom = controller.transform.position - new Vector3(0, controller.height / 2, 0); // Sets a point at the bottom of the player model

        RaycastHit hit;
        if (Physics.Raycast(bottom, Vector3.down, out hit, 0.4f) && velocity.y < 0) // Raycasts toward the ground, if it hits, move the player there - for bumpy ground and slopes
        {
            controller.Move(new Vector3(0, -hit.distance, 0));
            return true;
        }

        return false;
    }

    // Check if the player is high enough to walljump, to remove awkwardness
    public bool highEnoughCheck()
    {
        Vector3 bottom = controller.transform.position - new Vector3(0, controller.height / 2, 0);
        
        RaycastHit hit;
        if (Physics.Raycast(bottom, Vector3.down, out hit, 2f))
        {
            return false;
        }
        else
        {
            return true;
        }       
    }

    // If the player presses Ctrl when walljumping, refuse to walljump for a while
    void RefuseWallSticking()
    {
        if (!canStickToWall)
        {
            timeNotAbleToWallStick += Time.deltaTime;
        }
        if(timeNotAbleToWallStick >= maxTimeNotAbleToWallStick)
        {
            canStickToWall = true;
        }
    }
}
